using MultiPrecision;
using MultiPrecisionSpline;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QuantileApproximation {
    class QuinticSpline<N> : QuinticHermiteSpline<N> where N : struct, IConstant {
        public QuinticSpline(CubicHermiteSpline<N> cublic_spline) :
            base(cublic_spline.Xs.ToArray(), cublic_spline.Ys.ToArray(), cublic_spline.Grads.ToArray()) { }

        protected override MultiPrecision<N>[] ComputeSecondGrads() {
            return SplineUtil<N>.FiniteDifference(Xs, Grads);
        }
    }

    class Program {
        static readonly string results_dir = "../../../../results_disused/";

        static void Main(string[] args) {
            (MultiPrecision<Pow2.N4> x, MultiPrecision<Pow2.N4> pdf, MultiPrecision<Pow2.N4> cdf, MultiPrecision<Pow2.N4> ccdf)[] table = ReadCsv();

            foreach ((var x, var pdf, var cdf, var ccdf) in table) {
                Console.WriteLine($"{x},{pdf},{cdf},{ccdf}");
            }

            CubicHermiteSpline<Pow2.N4> under_cdf_spline = UnderCDF(table);
            CubicHermiteSpline<Pow2.N4> center_cdf_spline = CenterCDF(table);
            CubicHermiteSpline<Pow2.N4> upper_cdf_spline = UpperCDF(table);

            QuinticSpline<Pow2.N4> under_cdf_spline_q = new(under_cdf_spline);
            QuinticSpline<Pow2.N4> center_cdf_spline_q = new(center_cdf_spline);
            QuinticSpline<Pow2.N4> upper_cdf_spline_q = new(upper_cdf_spline);

            using (StreamWriter sw = new(results_dir + "under_cdf.csv")) {
                sw.WriteLine("u = -log2(cdf),v = pow2(-x),g = log(2)^2 * cdf * pow2(-x) / pdf");

                for (int i = 0; i < under_cdf_spline.Length; i++) {
                    var u = under_cdf_spline.Xs[i];
                    var v = under_cdf_spline.Ys[i];
                    var g = under_cdf_spline.Grads[i];

                    sw.WriteLine($"{u:e19},{v:e19},{g:e19}");
                }
            }

            using (StreamWriter sw = new(results_dir + "under_cdf_cubic_interp.csv")) {
                sw.WriteLine("u = -log2(cdf),v = pow2(-x),g = log(2)^2 * cdf * pow2(-x) / pdf");

                for (decimal u = 1.0000m; u <= 65; u += 1 / 16m) {
                    var v = under_cdf_spline.Value(u);
                    var g = under_cdf_spline.Grad(u);

                    sw.WriteLine($"{u},{v:e9},{g:e9}");
                }
            }

            using (StreamWriter sw = new(results_dir + "under_cdf_quintic_interp.csv")) {
                sw.WriteLine("u = -log2(cdf),v = pow2(-x),g,gg");

                for (decimal u = 1.0000m; u <= 65; u += 1 / 16m) {
                    var v = under_cdf_spline_q.Value(u);
                    var g = under_cdf_spline_q.Grad(u);
                    var gg = under_cdf_spline_q.SecondGrad(u);

                    sw.WriteLine($"{u},{v:e19},{g:e19},{gg:e19}");
                }
            }

            using (StreamWriter sw = new(results_dir + "center_cdf.csv")) {
                sw.WriteLine("u = cdf,v = x,g = 1 / pdf");

                for (int i = 0; i < center_cdf_spline.Length; i++) {
                    var u = center_cdf_spline.Xs[i];
                    var v = center_cdf_spline.Ys[i];
                    var g = center_cdf_spline.Grads[i];

                    sw.WriteLine($"{u:e19},{v},{g:e19}");
                }
            }

            using (StreamWriter sw = new(results_dir + "center_cdf_cubic_interp.csv")) {
                sw.WriteLine("u = cdf,v = x,g = 1 / pdf");

                for (decimal u = 0.250000m; u <= 3 / 4m; u += 1 / 16m / 20m) {
                    var v = center_cdf_spline.Value(u);
                    var g = center_cdf_spline.Grad(u);

                    sw.WriteLine($"{u},{v:e9},{g:e9}");
                }
            }

            using (StreamWriter sw = new(results_dir + "center_cdf_quintic_interp.csv")) {
                sw.WriteLine("u = cdf,v = x,g,gg");

                for (decimal u = 0.250000m; u <= 3 / 4m; u += 1 / 16m / 20m) {
                    var v = center_cdf_spline_q.Value(u);
                    var g = center_cdf_spline_q.Grad(u);
                    var gg = center_cdf_spline_q.SecondGrad(u);

                    sw.WriteLine($"{u},{v:e19},{g:e19},{gg:e19}");
                }
            }

            using (StreamWriter sw = new(results_dir + "upper_cdf.csv")) {
                sw.WriteLine("u = -log2(ccdf),v = log2(x),g = ccdf / x / pdf");

                for (int i = 0; i < upper_cdf_spline.Length; i++) {
                    var u = upper_cdf_spline.Xs[i];
                    var v = upper_cdf_spline.Ys[i];
                    var g = upper_cdf_spline.Grads[i];

                    sw.WriteLine($"{u:e19},{v:e19},{g:e19}");
                }
            }

            using (StreamWriter sw = new(results_dir + "upper_cdf_cubic_interp.csv")) {
                sw.WriteLine("u = -log2(ccdf),v = log2(x),g = ccdf / x / pdf");

                for (decimal u = 1.0000m; u <= 65; u += 1 / 16m) {
                    var v = upper_cdf_spline.Value(u);
                    var g = upper_cdf_spline.Grad(u);

                    sw.WriteLine($"{u},{v:e9},{g:e9}");
                }
            }

            using (StreamWriter sw = new(results_dir + "upper_cdf_quintic_interp.csv")) {
                sw.WriteLine("u = -log2(ccdf),v = log2(x),g,gg");

                for (decimal u = 1.0000m; u <= 65; u += 1 / 16m) {
                    var v = upper_cdf_spline_q.Value(u);
                    var g = upper_cdf_spline_q.Grad(u);
                    var gg = upper_cdf_spline_q.SecondGrad(u);

                    sw.WriteLine($"{u},{v:e19},{g:e19},{gg:e19}");
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }

        static (MultiPrecision<Pow2.N4> x, MultiPrecision<Pow2.N4> pdf, MultiPrecision<Pow2.N4> cdf, MultiPrecision<Pow2.N4> ccdf)[] ReadCsv() {
            List<(MultiPrecision<Pow2.N4> x, MultiPrecision<Pow2.N4> pdf, MultiPrecision<Pow2.N4> cdf, MultiPrecision<Pow2.N4> ccdf)> table = new();

            using (StreamReader sr = new StreamReader(results_dir + "table.csv")) {
                while (!sr.EndOfStream) {
                    string line = sr.ReadLine();

                    if (string.IsNullOrWhiteSpace(line)) {
                        break;
                    }

                    string[] items = line.Split(',');

                    if (items.Length < 4) {
                        break;
                    }

                    string xstr = items[0];
                    MultiPrecision<Pow2.N4> x, pdf = items[1], cdf = items[2], ccdf = items[3];

                    if (!xstr.Contains('^')) {
                        x = xstr;
                    }
                    else {
                        string[] x_split = xstr.Split('^');

                        if (x_split[0] != "2") {
                            throw new FormatException();
                        }

                        x = MultiPrecision<Pow2.N4>.Ldexp(1, int.Parse(x_split[1]));
                    }

                    table.Add((x, pdf, cdf, ccdf));
                }
            }

            return table.ToArray();
        }

        static CubicHermiteSpline<Pow2.N4> UnderCDF((MultiPrecision<Pow2.N4> x, MultiPrecision<Pow2.N4> pdf, MultiPrecision<Pow2.N4> cdf, MultiPrecision<Pow2.N4> ccdf)[] table) {
            List<MultiPrecision<Pow2.N4>> xs = new(), ys = new(), gs = new();

            foreach ((var x, var pdf, var cdf, _) in table.OrderByDescending((items) => items.x)) {
                if (cdf <= 0 || cdf > 0.6) {
                    continue;
                }

                MultiPrecision<Pow2.N4> dydx = MultiPrecision<Pow2.N4>.Square(MultiPrecision<Pow2.N4>.Ln2) * cdf * MultiPrecision<Pow2.N4>.Pow2(-x);

                MultiPrecision<Pow2.N4> g = dydx / pdf;

                xs.Add(-MultiPrecision<Pow2.N4>.Log2(cdf));
                ys.Add(MultiPrecision<Pow2.N4>.Pow2(-x));
                gs.Add(g);
            }

            CubicHermiteSpline<Pow2.N4> spline = new(xs.ToArray(), ys.ToArray(), gs.ToArray());

            return spline;
        }

        static CubicHermiteSpline<Pow2.N4> CenterCDF((MultiPrecision<Pow2.N4> x, MultiPrecision<Pow2.N4> pdf, MultiPrecision<Pow2.N4> cdf, MultiPrecision<Pow2.N4> ccdf)[] table) {
            List<MultiPrecision<Pow2.N4>> xs = new(), ys = new(), gs = new();

            foreach ((var x, var pdf, var cdf, _) in table.OrderBy((items) => items.x)) {
                if (cdf < 0.2 || cdf > 0.8) {
                    continue;
                }

                MultiPrecision<Pow2.N4> g = 1 / pdf;

                xs.Add(cdf);
                ys.Add(x);
                gs.Add(g);
            }

            CubicHermiteSpline<Pow2.N4> spline = new(xs.ToArray(), ys.ToArray(), gs.ToArray());

            return spline;
        }

        static CubicHermiteSpline<Pow2.N4> UpperCDF((MultiPrecision<Pow2.N4> x, MultiPrecision<Pow2.N4> pdf, MultiPrecision<Pow2.N4> cdf, MultiPrecision<Pow2.N4> ccdf)[] table) {
            List<MultiPrecision<Pow2.N4>> xs = new(), ys = new(), gs = new();

            foreach ((var x, var pdf, _, var ccdf) in table.OrderBy((items) => items.x)) {
                if (ccdf <= 0 || ccdf > 0.6) {
                    continue;
                }

                MultiPrecision<Pow2.N4> dydx = ccdf / x;

                MultiPrecision<Pow2.N4> g = dydx / pdf;

                xs.Add(-MultiPrecision<Pow2.N4>.Log2(ccdf));
                ys.Add(MultiPrecision<Pow2.N4>.Log2(x));
                gs.Add(g);
            }

            xs.Add(128);
            ys.Add(128);
            gs.Add(1);

            CubicHermiteSpline<Pow2.N4> spline = new(xs.ToArray(), ys.ToArray(), gs.ToArray());

            return spline;
        }
    }
}
