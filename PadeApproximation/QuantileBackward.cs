using MultiPrecision;
using MultiPrecisionAlgebra;
using PadeApproximation;
using static MultiPrecision.Pow2;

namespace PadeApproximate {
    class QuantileBackward {
        static void Main_() {

            List<(MultiPrecision<N16> ccdf, MultiPrecision<N16> lambda)> expecteds = ReadExpacted();

            List<(MultiPrecision<N16> c0, MultiPrecision<N16> c1, bool log2scale)> ranges = new() {
                (MultiPrecision<N16>.Ldexp(1, -1), MultiPrecision<N16>.Ldexp(1, -2), true),
                (MultiPrecision<N16>.Ldexp(1, -2), MultiPrecision<N16>.Ldexp(1, -4), true),
                (MultiPrecision<N16>.Ldexp(1, -4), MultiPrecision<N16>.Ldexp(1, -8), true),
                (MultiPrecision<N16>.Ldexp(1, -8), MultiPrecision<N16>.Ldexp(1, -16), true),
                (MultiPrecision<N16>.Ldexp(1, -16), MultiPrecision<N16>.Ldexp(1, -32), true),
                (MultiPrecision<N16>.Ldexp(1, -32), MultiPrecision<N16>.Ldexp(1, -64), true),
                (MultiPrecision<N16>.Ldexp(1, -64), MultiPrecision<N16>.Ldexp(1, -128), true),
                (MultiPrecision<N16>.Ldexp(1, -128), MultiPrecision<N16>.Ldexp(1, -256), true),
                (MultiPrecision<N16>.Ldexp(1, -256), MultiPrecision<N16>.Ldexp(1, -320), true),
            };

            foreach ((MultiPrecision<N16> u0, MultiPrecision<N16> u1, bool log2scale) in ranges) {
                List<(MultiPrecision<N16> cdf, MultiPrecision<N16> lambda)> expecteds_range = 
                    log2scale
                    ? expecteds.Where(item => item.ccdf >= u1 && item.ccdf <= u0).ToList()
                    : expecteds.Where(item => item.ccdf >= u0 && item.ccdf <= u1).ToList();

                string s0 = log2scale ? $"2xexp({u0.Exponent})" : $"{u0}";
                string s1 = log2scale ? $"2xexp({u1.Exponent})" : $"{u1}";

                MultiPrecision<N16>[] xs = expecteds_range.Select(item => log2scale ? (MultiPrecision<N16>.Log2(u0 / item.cdf)) : (item.cdf - u0)).ToArray();
                MultiPrecision<N16>[] ys = expecteds_range.Select(item => item.lambda).ToArray();

                Vector<N32> parameter, approx;
                bool success = false;

                for (int k = 8; k <= 64 && k * 2 + 1 < xs.Length; k++) {
                    Console.WriteLine($"k {k}");
                    (parameter, approx, success) = PadeApproximate<N32>(xs, ys, k, k);

                    if (success) {
                        using StreamWriter sw = new($"../../../../results_disused/padecoef_ccdf_precision82_{s0}_{s1}_{(log2scale ? "log2": "linear")}.csv");
                        PlotResult(sw, expecteds_range, k, parameter, approx);
                        break;
                    }
                }
            }
        }

        private static void PlotResult<N>(StreamWriter sw, List<(MultiPrecision<N16> u, MultiPrecision<N16> v)> expecteds, int numer, Vector<N> parameter, Vector<N> approx) where N : struct, IConstant {
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

        private static List<(MultiPrecision<N16> lambda, MultiPrecision<N16> scaled_ccdf)> ReadExpacted() {

            List<(MultiPrecision<N16> ccdf, MultiPrecision<N16> lambda)> expecteds = new();
            StreamReader stream = new("../../../../results_disused/quantile_ccdf_precision82_3.csv");
            stream.ReadLine();

            while (!stream.EndOfStream) {
                string? line = stream.ReadLine();

                if (string.IsNullOrWhiteSpace(line)) {
                    break;
                }

                string[] item = line.Split(',');

                MultiPrecision<N16> ccdf = MultiPrecision<N16>.Ldexp(item[1], int.Parse(item[0].Split('^')[0]));
                MultiPrecision<N16> lambda = item[3];

                expecteds.Add((ccdf, lambda));
            }

            return expecteds;
        }

        static (Vector<N> parameter, Vector<N> approx, bool success) PadeApproximate<N>(MultiPrecision<N16>[] xs, MultiPrecision<N16>[] ys, int numer, int denom) where N : struct, IConstant {
            static bool needs_increase_weight(MultiPrecision<N> error) {
                return error.Exponent >= -272;
            }

            Vector<N> weights = Vector<N>.Fill(xs.Length, 1);

            AdaptivePadeFitter<N> fitter = new(((Vector<N16>)xs).Convert<N>(), ((Vector<N16>)ys).Convert<N>(), numer, denom);

            (Vector<N> parameter, bool success) = fitter.ExecuteFitting(weights, needs_increase_weight);
            Vector<N> approx = fitter.FittingValue(((Vector<N16>)xs).Convert<N>(), parameter);

            return (parameter, approx, success);
        }
    }
}
