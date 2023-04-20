﻿using MultiPrecision;
using MultiPrecisionAlgebra;
using PadeApproximation;
using static MultiPrecision.Pow2;

namespace PadeApproximate {
    class CDFbackward {
        static void Main_() {

            List<(MultiPrecision<N16> lambda, MultiPrecision<N16> scaled_pdf)> expecteds = ReadExpacted();

            List<(Sign sign, double min, double max, double u0, int dir)> ranges = new() {
                (Sign.Plus, 0, 1, 0, +1),
                (Sign.Plus, 1, 2, 1, +1),
                (Sign.Plus, 2, 4, 2, +1),
                (Sign.Plus, 4, 6, 4, +1),
                (Sign.Plus, 6, 8, 6, +1),
                (Sign.Plus, 8, 16, 8, +1),
                (Sign.Plus, 16, 32, 16, +1),
                (Sign.Plus, 32, 64, 32, +1),
                (Sign.Plus, 64, 128, 64, +1),
            };

            foreach ((Sign sign, double min, double max, double u0, int dir) in ranges) {
                List<(MultiPrecision<N16> lambda, MultiPrecision<N16> scaled_pdf)> expecteds_range =
                    sign == Sign.Plus
                        ? expecteds.Where(item => item.lambda.Sign == sign && item.lambda >= min && item.lambda <= max).ToList()
                        : expecteds.Where(item => item.lambda.Sign == sign && item.lambda >= -max && item.lambda <= -min).ToList();

                MultiPrecision<N16>[] xs = expecteds_range.Select(item => sign == Sign.Plus ? item.lambda - u0 : -item.lambda - u0).ToArray();
                MultiPrecision<N16>[] ys = expecteds_range.Select(item => item.scaled_pdf).ToArray();

                if (dir < 0) {
                    xs = -(Vector<N16>)xs;
                }

                Vector<N32> parameter, approx;
                bool success = false;

                for (int k = 8; k <= 64 && k * 2 + 1 < xs.Length; k++) {
                    Console.WriteLine($"k {k}");
                    (parameter, approx, success) = PadeApproximate<N32>(xs, ys, k, k);

                    if (success) {
                        using StreamWriter sw = new($"../../../../results_disused/padecoef_cdf_precision82_{sign}_{min}_{max}_{u0}_{((dir > 0) ? "+1" : "-1")}.csv");
                        PlotResult(sw, expecteds_range, u0, k, dir, parameter, approx);
                        break;
                    }
                }
            }
        }

        private static void PlotResult<N>(StreamWriter sw, List<(MultiPrecision<N16> u, MultiPrecision<N16> v)> expecteds, MultiPrecision<N16> u0, int numer, int dir, Vector<N> parameter, Vector<N> approx) where N : struct, IConstant {
            sw.WriteLine($"u0 = {u0}");
            sw.WriteLine($"dir = {((dir > 0) ? "+1" : "-1")}");
            sw.WriteLine($"numers: {numer}");
            foreach ((_, MultiPrecision<N> v) in parameter[..numer]) {
                sw.WriteLine(v);
            }

            sw.WriteLine($"denoms: {(parameter.Dim - numer)}");
            foreach ((_, MultiPrecision<N> v) in parameter[numer..]) {
                sw.WriteLine(v);
            }

            sw.WriteLine("u,expected,approx,error");
            for (int i = 0; i < expecteds.Count; i++) {
                sw.WriteLine($"{expecteds[i].u},{expecteds[i].v},{approx[i]},{(expecteds[i].v - approx[i].Convert<N16>()):e10}");
            }
        }

        private static List<(MultiPrecision<N16> lambda, MultiPrecision<N16> scaled_cdf)> ReadExpacted() {

            List<(MultiPrecision<N16> lambda, MultiPrecision<N16> scaled_cdf)> expecteds = new();
            StreamReader stream = new("../../../../results_disused/cdf_backward_precision82.csv");
            stream.ReadLine();

            while (!stream.EndOfStream) {
                string? line = stream.ReadLine();

                if (string.IsNullOrWhiteSpace(line)) {
                    break;
                }

                string[] item = line.Split(',');

                MultiPrecision<N16> lambda = item[0], cdf = item[1];

                MultiPrecision<N16> scaled_cdf = cdf * (lambda + 4);

                expecteds.Add((lambda, scaled_cdf));
            }

            return expecteds;
        }

        static (Vector<N> parameter, Vector<N> approx, bool success) PadeApproximate<N>(MultiPrecision<N16>[] xs, MultiPrecision<N16>[] ys, int numer, int denom) where N : struct, IConstant {
            static bool needs_increase_weight(MultiPrecision<N> error) {
                return error.Exponent >= -275;
            }

            Vector<N> weights = Vector<N>.Fill(xs.Length, 1);

            AdaptivePadeFitter<N> fitter = new(((Vector<N16>)xs).Convert<N>(), ((Vector<N16>)ys).Convert<N>(), numer, denom);

            (Vector<N> parameter, bool success) = fitter.ExecuteFitting(weights, needs_increase_weight);
            Vector<N> approx = fitter.FittingValue(((Vector<N16>)xs).Convert<N>(), parameter);

            return (parameter, approx, success);
        }
    }
}
