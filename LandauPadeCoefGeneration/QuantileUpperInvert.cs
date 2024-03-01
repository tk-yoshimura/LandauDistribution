using MultiPrecision;
using MultiPrecisionAlgebra;
using MultiPrecisionCurveFitting;

namespace LandauPadeCoefGeneration {
    class QuantileUpperInvert {
        static void Main() {
            const int max_samples = 4096;

            List<(MultiPrecision<Pow2.N64> pmin, MultiPrecision<Pow2.N64> pmax, MultiPrecision<Pow2.N64> limit_range)> ranges = [
                (0, 1 / 1024d, 1 / 65536d)
            ];

            List<(MultiPrecision<Pow2.N64> p, MultiPrecision<Pow2.N64> y)> expecteds = [];

            using (StreamReader sr = new("../../../../results_disused/quantile_upper_precision152_pluslimit_invert_2.csv")) {
                sr.ReadLine();

                while (!sr.EndOfStream) {
                    string? line = sr.ReadLine();

                    if (string.IsNullOrWhiteSpace(line)) {
                        break;
                    }

                    string[] line_split = line.Split(",");

                    MultiPrecision<Pow2.N64> p = line_split[0];
                    MultiPrecision<Pow2.N64> x = line_split[1];

                    expecteds.Add((p, x));
                }
            }

            expecteds.Reverse();

            using (StreamWriter sw = new("../../../../results_disused/pade_quantile_upper_invert_precision150_scaled_2.csv")) {
                bool approximate(MultiPrecision<Pow2.N64> pmin, MultiPrecision<Pow2.N64> pmax) {
                    Console.WriteLine($"[{pmin}, {pmax}]");

                    List<(MultiPrecision<Pow2.N64> p, MultiPrecision<Pow2.N64> y)> expecteds_range
                        = expecteds.Where(item => item.p >= pmin && item.p <= pmax).ToList();

                    if (expecteds_range.Count > max_samples) {
                        int c = expecteds_range.Count / max_samples;

                        expecteds_range = expecteds_range.Where((_, idx) => idx % c == 0).ToList();
                    }

                    Console.WriteLine($"expecteds {expecteds_range.Count} samples");

                    Vector<Pow2.N64> xs = expecteds_range.Select(item => item.p).ToArray();
                    Vector<Pow2.N64> ys = expecteds_range.Select(item => item.y).ToArray();

                    xs -= pmin;

                    (long exp_scale, xs) = CurveFittingUtils.StandardizeExponent(xs);

                    Console.WriteLine($"scale={exp_scale}");

                    for (int coefs = 5; coefs <= expecteds_range.Count / 2 && coefs <= 128; coefs++) {
                        foreach ((int m, int n) in CurveFittingUtils.EnumeratePadeDegree(coefs, 2)) {
                            PadeFitter<Pow2.N64> pade = new(xs, ys, m, n);

                            Vector<Pow2.N64> param = pade.ExecuteFitting();

                            MultiPrecision<Pow2.N64> max_rateerr = CurveFittingUtils.MaxRelativeError(ys, pade.FittingValue(xs, param));

                            Console.WriteLine($"m={m},n={n}");
                            Console.WriteLine($"{max_rateerr:e20}");

                            Console.WriteLine($"mcount: {param.Count(item => item.val.Sign != Sign.Plus)}");

                            if (coefs > 8 && max_rateerr > "1e-12") {
                                return false;
                            }

                            if (coefs > 32 && max_rateerr > "1e-45") {
                                return false;
                            }

                            if (max_rateerr > "1e-145") {
                                break;
                            }

                            if (max_rateerr < "1e-150" &&
                                !CurveFittingUtils.HasLossDigitsPolynomialCoef(param[..m], 0, xs[^1] - xs[0]) &&
                                !CurveFittingUtils.HasLossDigitsPolynomialCoef(param[m..], 0, xs[^1] - xs[0])) {

                                sw.WriteLine($"p=[{pmin},{pmax}]");
                                sw.WriteLine($"m={m},n={n}");
                                sw.WriteLine($"scale={exp_scale}");
                                sw.WriteLine($"expecteds {expecteds_range.Count} samples");

                                sw.WriteLine("numer");
                                foreach (var (_, val) in param[..m]) {
                                    sw.WriteLine($"{val:e155}");
                                }
                                sw.WriteLine("denom");
                                foreach (var (_, val) in param[m..]) {
                                    sw.WriteLine($"{val:e155}");
                                }

                                sw.WriteLine("coef");
                                foreach ((var numer, var denom) in CurveFittingUtils.EnumeratePadeCoef(param, m, n)) {
                                    sw.WriteLine($"(\"{numer:e155}\", \"{denom:e155}\"),");
                                }

                                sw.WriteLine("relative err");
                                sw.WriteLine($"{max_rateerr:e20}");
                                sw.Flush();

                                return true;
                            }
                        }
                    }

                    return false;
                }

                Segmenter<Pow2.N64> segmenter = new(ranges, approximate);
                segmenter.Execute();

                foreach ((var xmin, var xmax, bool is_successs) in segmenter.ApproximatedRanges) {
                    sw.WriteLine($"[{xmin},{xmax}],{(is_successs ? "OK" : "NG")}");
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
