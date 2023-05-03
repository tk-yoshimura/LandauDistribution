using MultiPrecision;
using MultiPrecisionAlgebra;
using PadeApproximation;
using static MultiPrecision.Pow2;

namespace PadeApproximate {
    class QuantileForward {
        static void Main_() {

            List<(MultiPrecision<N64> cdf, MultiPrecision<N64> lambda)> expecteds = ReadExpacted();

            List<(MultiPrecision<N64> c0, MultiPrecision<N64> c1, bool log2scale)> ranges = new() {
                (MultiPrecision<N64>.Ldexp(1, -1), MultiPrecision<N64>.Ldexp(1, -2), true),
                (MultiPrecision<N64>.Ldexp(1, -2), MultiPrecision<N64>.Ldexp(1, -4), true),
                (MultiPrecision<N64>.Ldexp(1, -4), MultiPrecision<N64>.Ldexp(1, -8), true),
                (MultiPrecision<N64>.Ldexp(1, -8), MultiPrecision<N64>.Ldexp(1, -16), true),
                (MultiPrecision<N64>.Ldexp(1, -16), MultiPrecision<N64>.Ldexp(1, -32), true),
                (MultiPrecision<N64>.Ldexp(1, -32), MultiPrecision<N64>.Ldexp(1, -64), true),
                (MultiPrecision<N64>.Ldexp(1, -64), MultiPrecision<N64>.Ldexp(1, -128), true),
                (MultiPrecision<N64>.Ldexp(1, -128), MultiPrecision<N64>.Ldexp(1, -256), true),
                (MultiPrecision<N64>.Ldexp(1, -256), MultiPrecision<N64>.Ldexp(1, -512), true),
                (MultiPrecision<N64>.Ldexp(1, -512), MultiPrecision<N64>.Ldexp(1, -1024), true),
                (MultiPrecision<N64>.Ldexp(1, -1024), MultiPrecision<N64>.Ldexp(1, -2048), true),
                (MultiPrecision<N64>.Ldexp(1, -2048), MultiPrecision<N64>.Ldexp(1, -4096), true),
                (MultiPrecision<N64>.Ldexp(1, -4096), MultiPrecision<N64>.Ldexp(1, -8192), true),
                (MultiPrecision<N64>.Ldexp(1, -8192), MultiPrecision<N64>.Ldexp(1, -16384), true),
                (MultiPrecision<N64>.Ldexp(1, -16384), MultiPrecision<N64>.Ldexp(1, -32768), true),
                (MultiPrecision<N64>.Ldexp(1, -32768), MultiPrecision<N64>.Ldexp(1, -65536), true),
                (MultiPrecision<N64>.Ldexp(1, -65536), MultiPrecision<N64>.Ldexp(1, -131072), true),
                (MultiPrecision<N64>.Ldexp(1, -131072), MultiPrecision<N64>.Ldexp(1, -262144), true),
            };

            foreach ((MultiPrecision<N64> u0, MultiPrecision<N64> u1, bool log2scale) in ranges) {
                List<(MultiPrecision<N64> cdf, MultiPrecision<N64> lambda)> expecteds_range =
                    log2scale
                    ? expecteds.Where(item => item.cdf >= u1 && item.cdf <= u0).ToList()
                    : expecteds.Where(item => item.cdf >= u0 && item.cdf <= u1).ToList();

                string s0 = log2scale ? $"2xexp({u0.Exponent})" : $"{u0}";
                string s1 = log2scale ? $"2xexp({u1.Exponent})" : $"{u1}";

                MultiPrecision<N64>[] xs = expecteds_range.Select(item => log2scale ? (MultiPrecision<N64>.Log2(u0 / item.cdf)) : (item.cdf - u0)).ToArray();
                MultiPrecision<N64>[] ys = expecteds_range.Select(item => item.lambda).ToArray();

                Vector<N64> parameter, approx;
                bool success = false;

                for (int k = 30; k <= 64 && k * 2 + 1 < xs.Length; k++) {
                    Console.WriteLine($"k {k}");
                    (parameter, approx, success) = PadeApproximate<N64>(xs, ys, k, k);

                    if (parameter[k..].Any(v => v.val.Sign == Sign.Minus)) {
                        continue;
                    }

                    if (success) {
                        using StreamWriter sw = new($"../../../../results_disused/padecoef_cdf_precision102_{s0}_{s1}_{(log2scale ? "log2" : "linear")}.csv");
                        PlotResult(sw, expecteds_range, k, parameter, approx);
                        break;
                    }
                }
            }
        }

        private static void PlotResult<N>(StreamWriter sw, List<(MultiPrecision<N64> u, MultiPrecision<N64> v)> expecteds, int numer, Vector<N> parameter, Vector<N> approx) where N : struct, IConstant {
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
                sw.WriteLine($"{expecteds[i].u},{expecteds[i].v},{approx[i]},{(expecteds[i].v - approx[i].Convert<N64>()):e10}");
            }
        }

        private static List<(MultiPrecision<N64> lambda, MultiPrecision<N64> scaled_cdf)> ReadExpacted() {

            List<(MultiPrecision<N64> cdf, MultiPrecision<N64> lambda)> expecteds = new();
            StreamReader stream = new("../../../../results_disused/quantile_cdf_precision103.csv");
            stream.ReadLine();

            while (!stream.EndOfStream) {
                string? line = stream.ReadLine();

                if (string.IsNullOrWhiteSpace(line)) {
                    break;
                }

                string[] item = line.Split(',');

                MultiPrecision<N64> cdf = MultiPrecision<N64>.Ldexp(item[1], int.Parse(item[0].Split('^')[0]));
                MultiPrecision<N64> lambda = item[2];

                expecteds.Add((cdf, lambda));
            }

            return expecteds;
        }

        static (Vector<N> parameter, Vector<N> approx, bool success) PadeApproximate<N>(MultiPrecision<N64>[] xs, MultiPrecision<N64>[] ys, int numer, int denom) where N : struct, IConstant {
            static bool needs_increase_weight(MultiPrecision<N> error) {
                return error.Exponent >= -338;
            }

            Vector<N> weights = Vector<N>.Fill(xs.Length, 1);

            AdaptivePadeFitter<N> fitter = new(((Vector<N64>)xs).Convert<N>(), ((Vector<N64>)ys).Convert<N>(), numer, denom);

            (Vector<N> parameter, bool success) = fitter.ExecuteFitting(weights, needs_increase_weight);
            Vector<N> approx = fitter.FittingValue(((Vector<N64>)xs).Convert<N>(), parameter);

            return (parameter, approx, success);
        }
    }
}
