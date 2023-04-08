using MultiPrecision;
using MultiPrecisionAlgebra;
using PadeApproximation;
using System;
using static MultiPrecision.Pow2;

namespace MathieuPadeApproximate {
    class Program {
        static void Main() {

            List<(MultiPrecision<N8> lambda, MultiPrecision<N8> scaled_pdf)> expecteds = ReadExpacted();

            List<(Sign sign, double min, double max, double u0, int dir)> ranges = new() {
                (Sign.Minus, 0.0, 1.0, 1.0, -1),
                (Sign.Minus, 1.0, 2.0, 2.0, -1),
                (Sign.Minus, 0.0, 2.0, 2.0, -1),

                (Sign.Minus, 0.0, 1.0, 0.0, +1),
                (Sign.Minus, 1.0, 2.0, 1.0, +1),
                (Sign.Minus, 2.0, 3.0, 2.0, +1),
                (Sign.Minus, 3.0, 4.0, 3.0, +1),
                (Sign.Minus, 4.0, 5.0, 4.0, +1),
                (Sign.Minus, 5.0, 6.0, 5.0, +1),
                (Sign.Minus, 6.0, 7.0, 6.0, +1),

                (Sign.Minus, 0.0, 0.5, 0.0, +1),
                (Sign.Minus, 0.5, 1.0, 0.5, +1),

                (Sign.Minus, 2.0, 4.0  , 2.0, +1),
                (Sign.Minus, 4.0, 8.0  , 4.0, +1),
                (Sign.Minus, 0.0, 4.0  , 0.0, +1),
                (Sign.Minus, 0.0, 6.625, 0.0, +1),

                (Sign.Plus, 0.0, 1.0   , 0.0, +1),
                (Sign.Plus, 1.0, 2.0   , 1.0, +1),
                (Sign.Plus, 2.0, 4.0   , 2.0, +1),
                (Sign.Plus, 4.0, 8.0   , 4.0, +1),
                (Sign.Plus, 8.0, 16.0  , 8.0, +1),
                (Sign.Plus, 16.0, 32.0 , 16.0, +1),
                (Sign.Plus, 32.0, 66.5 , 32.0, +1),

                (Sign.Plus, 0.0, 2.0, 0.0, +1),
                (Sign.Plus, 0.0, 4.0, 0.0, +1),
                (Sign.Plus, 0.0, 8.0, 0.0, +1),
            };

            foreach ((Sign sign, double min, double max, double u0, int dir) in ranges) {
                List<(MultiPrecision<N8> lambda, MultiPrecision<N8> scaled_pdf)> expecteds_range =
                    sign == Sign.Plus
                        ? expecteds.Where(item => item.lambda.Sign == sign && item.lambda >= min && item.lambda <= max).ToList()
                        : expecteds.Where(item => item.lambda.Sign == sign && item.lambda >= -max && item.lambda <= -min).ToList();

                MultiPrecision<N8>[] xs = expecteds_range.Select(item => sign == Sign.Plus ? item.lambda - u0 : -item.lambda - u0).ToArray();
                MultiPrecision<N8>[] ys = expecteds_range.Select(item => item.scaled_pdf).ToArray();

                if (dir < 0) {
                    xs = -(Vector<N8>)xs;
                }

                Vector<N32> parameter, approx;
                bool success = false;

                for (int k = 8; k <= 64 && k * 2 + 1 < xs.Length; k++) {
                    Console.WriteLine($"k {k}");
                    (parameter, approx, success) = PadeApproximate<N32>(xs, ys, k, k);

                    if (success) {
                        using StreamWriter sw = new($"../../../../results_disused/padecoef_pdf_{sign}_{min}_{max}_{u0}_{((dir > 0) ? "+1" : "-1")}.csv");
                        PlotResult(sw, expecteds_range, u0, k, dir, parameter, approx);
                        break;
                    }
                }
            }
        }

        private static void PlotResult<N>(StreamWriter sw, List<(MultiPrecision<N8> u, MultiPrecision<N8> v)> expecteds, MultiPrecision<N8> u0, int numer, int dir, Vector<N> parameter, Vector<N> approx) where N : struct, IConstant {
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
                sw.WriteLine($"{expecteds[i].u},{expecteds[i].v},{approx[i]},{(expecteds[i].v - approx[i].Convert<N8>()):e10}");
            }
        }

        private static List<(MultiPrecision<N8> lambda, MultiPrecision<N8> scaled_pdf)> ReadExpacted() {

            List<(MultiPrecision<N8> lambda, MultiPrecision<N8> scaled_pdf)> expecteds = new();
            StreamReader stream = new("../../../../results_disused/integrate_scaled_pdf_precision68.csv");
            for (int i = 0; i < 3; i++) {
                stream.ReadLine();
            }

            while (!stream.EndOfStream) {
                string? line = stream.ReadLine();

                if (string.IsNullOrWhiteSpace(line)) {
                    break;
                }

                string[] item = line.Split(',');

                MultiPrecision<N8> lambda = item[0], scaled_pdf = item[1];

                expecteds.Add((lambda, scaled_pdf));
            }

            return expecteds;
        }

        static (Vector<N> parameter, Vector<N> approx, bool success) PadeApproximate<N>(MultiPrecision<N8>[] xs, MultiPrecision<N8>[] ys, int numer, int denom) where N : struct, IConstant {
            static bool needs_increase_weight(MultiPrecision<N> error) {
                return error.Exponent >= -232;
            }

            Vector<N> weights = Vector<N>.Fill(xs.Length, 1);

            AdaptivePadeFitter<N> fitter = new(((Vector<N8>)xs).Convert<N>(), ((Vector<N8>)ys).Convert<N>(), numer, denom);

            (Vector<N> parameter, bool success) = fitter.ExecuteFitting(weights, needs_increase_weight);
            Vector<N> approx = fitter.FittingValue(((Vector<N8>)xs).Convert<N>(), parameter);

            return (parameter, approx, success);
        }
    }
}
