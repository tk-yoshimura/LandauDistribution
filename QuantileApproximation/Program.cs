using MultiPrecision;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QuantileApproximation {

    class Program {
        static readonly string results_dir = "../../../../results/";
        static readonly string resultsout_dir = "../../../../results_disused/";

        static void Main(string[] args) {
            (MultiPrecision<Pow2.N8> x, MultiPrecision<Pow2.N8> pdf, MultiPrecision<Pow2.N8> cdf, MultiPrecision<Pow2.N8> ccdf, MultiPrecision<Pow2.N8> pdfdiff)[] table = ReadCsv();

            QuinticHermiteSpline<Pow2.N8> under_cdf_spline = UnderCDF(table);
            QuinticHermiteSpline<Pow2.N8> center_cdf_spline = CenterCDF(table);
            QuinticHermiteSpline<Pow2.N8> upper_cdf_spline = UpperCDF(table);

            using (StreamWriter sw = new(resultsout_dir + "under_cdf.csv")) {
                sw.WriteLine("u = -log2(cdf),v = pow2(-x),g,gg");

                for (int i = 0; i < under_cdf_spline.Length; i++) {
                    var u = under_cdf_spline.Xs[i];
                    var v = under_cdf_spline.Ys[i];
                    var g = under_cdf_spline.Grads[i];
                    var gg = under_cdf_spline.SecondGrads[i];

                    sw.WriteLine($"{u:e19},{v:e19},{g:e19},{gg:e19}");
                }
            }

            using (StreamWriter sw = new(resultsout_dir + "under_cdf_quintic_interp.csv")) {
                sw.WriteLine("u = -log2(cdf),v = pow2(-x),g,gg");

                for (decimal u = 1.0000m; u <= 65; u += 1 / 16m) {
                    var v = under_cdf_spline.Value(u);
                    var g = under_cdf_spline.Grad(u);
                    var gg = under_cdf_spline.SecondGrad(u);

                    sw.WriteLine($"{u},{v:e19},{g:e19},{gg:e19}");
                }
            }

            using (StreamWriter sw = new(resultsout_dir + "center_cdf.csv")) {
                sw.WriteLine("u = cdf,v = x,g,gg");

                for (int i = 0; i < center_cdf_spline.Length; i++) {
                    var u = center_cdf_spline.Xs[i];
                    var v = center_cdf_spline.Ys[i];
                    var g = center_cdf_spline.Grads[i];
                    var gg = center_cdf_spline.SecondGrads[i];

                    sw.WriteLine($"{u:e19},{v},{g:e19},{gg:e19}");
                }
            }

            using (StreamWriter sw = new(resultsout_dir + "center_cdf_quintic_interp.csv")) {
                sw.WriteLine("u = cdf,v = x,g,gg");

                for (decimal u = 0.250000m; u <= 3 / 4m; u += 1 / 16m / 20m) {
                    var v = center_cdf_spline.Value(u);
                    var g = center_cdf_spline.Grad(u);
                    var gg = center_cdf_spline.SecondGrad(u);

                    sw.WriteLine($"{u},{v:e19},{g:e19},{gg:e19}");
                }
            }

            using (StreamWriter sw = new(resultsout_dir + "upper_cdf.csv")) {
                sw.WriteLine("u = -log2(ccdf),v = log2(x),g,gg");

                for (int i = 0; i < upper_cdf_spline.Length; i++) {
                    var u = upper_cdf_spline.Xs[i];
                    var v = upper_cdf_spline.Ys[i];
                    var g = upper_cdf_spline.Grads[i];
                    var gg = upper_cdf_spline.SecondGrads[i];

                    sw.WriteLine($"{u:e19},{v:e19},{g:e19},{gg:e19}");
                }
            }

            using (StreamWriter sw = new(resultsout_dir + "upper_cdf_quintic_interp.csv")) {
                sw.WriteLine("u = -log2(ccdf),v = log2(x),g,gg");

                for (decimal u = 1.0000m; u <= 65; u += 1 / 16m) {
                    var v = upper_cdf_spline.Value(u);
                    var g = upper_cdf_spline.Grad(u);
                    var gg = upper_cdf_spline.SecondGrad(u);

                    sw.WriteLine($"{u},{v:e19},{g:e19},{gg:e19}");
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }

        static (MultiPrecision<Pow2.N8> x, MultiPrecision<Pow2.N8> pdf, MultiPrecision<Pow2.N8> cdf, MultiPrecision<Pow2.N8> ccdf, MultiPrecision<Pow2.N8> pdfdiff)[] ReadCsv() {
            List<(MultiPrecision<Pow2.N8> x, MultiPrecision<Pow2.N8> pdf, MultiPrecision<Pow2.N8> cdf, MultiPrecision<Pow2.N8> ccdf, MultiPrecision<Pow2.N8> pdfdiff)> table = new();

            using (StreamReader sr = new StreamReader(results_dir + "table.csv")) {
                //skip header
                sr.ReadLine();
                sr.ReadLine();

                while (!sr.EndOfStream) {
                    string line = sr.ReadLine();

                    if (string.IsNullOrWhiteSpace(line)) {
                        break;
                    }

                    string[] items = line.Split(',');

                    if (items.Length < 5) {
                        break;
                    }

                    string xstr = items[0];
                    MultiPrecision<Pow2.N8> x, pdf = items[1], cdf = items[2], ccdf = items[3], pdfdiff = items[4];

                    if (!xstr.Contains('^')) {
                        x = xstr;
                    }
                    else {
                        string[] x_split = xstr.Split('^');

                        if (x_split[0] != "2") {
                            throw new FormatException();
                        }

                        x = MultiPrecision<Pow2.N8>.Ldexp(1, int.Parse(x_split[1]));
                    }

                    table.Add((x, pdf, cdf, ccdf, pdfdiff));
                }
            }

            return table.ToArray();
        }

        static QuinticHermiteSpline<Pow2.N8> UnderCDF((MultiPrecision<Pow2.N8> x, MultiPrecision<Pow2.N8> pdf, MultiPrecision<Pow2.N8> cdf, MultiPrecision<Pow2.N8> ccdf, MultiPrecision<Pow2.N8> pdfdiff)[] table) {
            List<MultiPrecision<Pow2.N8>> us = new(), vs = new(), gs = new(), ggs = new();

            foreach ((var x, var pdf, var cdf, _, var pdfdiff) in table.OrderByDescending((items) => items.x)) {
                if (cdf <= 0 || cdf > 0.6) {
                    continue;
                }

                MultiPrecision<Pow2.N8> u = -MultiPrecision<Pow2.N8>.Log2(cdf);
                MultiPrecision<Pow2.N8> v = MultiPrecision<Pow2.N8>.Pow2(-x);
                MultiPrecision<Pow2.N8> g = 1 / pdf, gg = -pdfdiff * g * g * g;

                MultiPrecision<Pow2.N8> g_trans = MultiPrecision<Pow2.N8>.Ln2 * MultiPrecision<Pow2.N8>.Ln2 * g * cdf * v;
                MultiPrecision<Pow2.N8> gg_trans = -MultiPrecision<Pow2.N8>.Ln2 * MultiPrecision<Pow2.N8>.Ln2 * MultiPrecision<Pow2.N8>.Ln2 * v *
                                                    (g * cdf - MultiPrecision<Pow2.N8>.Ln2 * g * g * cdf * cdf + gg * cdf * cdf);

                us.Add(u);
                vs.Add(v);
                gs.Add(g_trans);
                ggs.Add(gg_trans);
            }

            QuinticHermiteSpline<Pow2.N8> spline = new(us.ToArray(), vs.ToArray(), gs.ToArray(), ggs.ToArray());

            return spline;
        }

        static QuinticHermiteSpline<Pow2.N8> CenterCDF((MultiPrecision<Pow2.N8> x, MultiPrecision<Pow2.N8> pdf, MultiPrecision<Pow2.N8> cdf, MultiPrecision<Pow2.N8> ccdf, MultiPrecision<Pow2.N8> pdfdiff)[] table) {
            List<MultiPrecision<Pow2.N8>> us = new(), vs = new(), gs = new(), ggs = new();

            foreach ((var x, var pdf, var cdf, _, var pdfdiff) in table.OrderBy((items) => items.x)) {
                if (cdf < 0.2 || cdf > 0.8) {
                    continue;
                }

                MultiPrecision<Pow2.N8> u = cdf;
                MultiPrecision<Pow2.N8> v = x;
                MultiPrecision<Pow2.N8> g = 1 / pdf, gg = -pdfdiff * g * g * g;

                us.Add(u);
                vs.Add(v);
                gs.Add(g);
                ggs.Add(gg);
            }

            QuinticHermiteSpline<Pow2.N8> spline = new(us.ToArray(), vs.ToArray(), gs.ToArray(), ggs.ToArray());

            return spline;
        }

        static QuinticHermiteSpline<Pow2.N8> UpperCDF((MultiPrecision<Pow2.N8> x, MultiPrecision<Pow2.N8> pdf, MultiPrecision<Pow2.N8> cdf, MultiPrecision<Pow2.N8> ccdf, MultiPrecision<Pow2.N8> pdfdiff)[] table) {
            List<MultiPrecision<Pow2.N8>> us = new(), vs = new(), gs = new(), ggs = new();

            foreach ((var x, var pdf, _, var ccdf, var pdfdiff) in table.OrderBy((items) => items.x)) {
                if (ccdf <= 0 || ccdf > 0.6) {
                    continue;
                }

                MultiPrecision<Pow2.N8> u = -MultiPrecision<Pow2.N8>.Log2(ccdf);
                MultiPrecision<Pow2.N8> v = MultiPrecision<Pow2.N8>.Log2(x);
                MultiPrecision<Pow2.N8> g = 1 / -pdf, gg = pdfdiff / (-pdf * -pdf * -pdf);

                MultiPrecision<Pow2.N8> g_trans = -g * ccdf / x;
                MultiPrecision<Pow2.N8> gg_trans = MultiPrecision<Pow2.N8>.Ln2 * (-g_trans * (1 + g_trans) + gg * ccdf * ccdf / x);

                us.Add(u);
                vs.Add(v);
                gs.Add(g_trans);
                ggs.Add(gg_trans);
            }

            us.Add(128);
            vs.Add(128);
            gs.Add(1);
            ggs.Add(0);

            QuinticHermiteSpline<Pow2.N8> spline = new(us.ToArray(), vs.ToArray(), gs.ToArray(), ggs.ToArray());

            return spline;
        }
    }
}
