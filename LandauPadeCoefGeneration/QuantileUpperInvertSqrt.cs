using MultiPrecision;
using MultiPrecisionAlgebra;
using MultiPrecisionCurveFitting;

namespace LandauPadeCoefGeneration {
    class QuantileUpperInvertSqrt {
        static void Main_() {
            const int samples = 1024;

            List<(MultiPrecision<Pow2.N128> umin, MultiPrecision<Pow2.N128> umax, MultiPrecision<Pow2.N128> limit_range)> ranges = [
                (0, 1d / 8, MultiPrecision<Pow2.N128>.Ldexp(1, -100))
            ];

            Dictionary<MultiPrecision<Pow2.N128>, MultiPrecision<Pow2.N128>> expecteds = [];

            using (StreamReader sr = new("../../../../results_disused/quantile_upper_precision152_pluslimit_invertsqrt.csv")) {
                sr.ReadLine();

                while (!sr.EndOfStream) {
                    string? line = sr.ReadLine();

                    if (string.IsNullOrWhiteSpace(line)) {
                        break;
                    }

                    string[] line_split = line.Split(",");

                    MultiPrecision<Pow2.N128> u = line_split[0];
                    MultiPrecision<Pow2.N128> x = line_split[2];

                    expecteds.Add(u, x);
                }
            }

            using (StreamWriter sw = new("../../../../results_disused/pade_quantile_upper_invert_precision150_invertsqrt.csv")) {
                bool approximate(MultiPrecision<Pow2.N128> umin, MultiPrecision<Pow2.N128> umax) {
                    Console.WriteLine($"[{umin}, {umax}]");

                    List<(MultiPrecision<Pow2.N128> u, MultiPrecision<Pow2.N128> y)> expecteds_range = [];

                    for (MultiPrecision<Pow2.N128> u = umin, h = (umax - umin) / samples; u <= umax; u += h) {
                        if (expecteds.TryGetValue(u, out MultiPrecision<Pow2.N128>? v)) {
                            expecteds_range.Add((u, v));
                        }
                    }

                    Console.WriteLine($"expecteds {expecteds_range.Count} samples");

                    Vector<Pow2.N128> xs = expecteds_range.Select(item => item.u).ToArray();
                    Vector<Pow2.N128> ys = expecteds_range.Select(item => item.y).ToArray();

                    xs -= umin;

                    (long exp_scale, xs) = CurveFittingUtils.StandardizeExponent(xs);

                    Console.WriteLine($"scale={exp_scale}");

                    for (int coefs = 5; coefs <= expecteds_range.Count / 2 && coefs <= 280; coefs++) {
                        foreach ((int m, int n) in CurveFittingUtils.EnumeratePadeDegree(coefs, 2)) {
                            PadeFitter<Pow2.N128> pade = (umin > 0) ? new(xs, ys, m, n) : new(xs, ys, m, n, intercept: 1);

                            Vector<Pow2.N128> param = pade.ExecuteFitting();

                            MultiPrecision<Pow2.N128> max_rateerr = CurveFittingUtils.MaxRelativeError(ys, pade.FittingValue(xs, param));

                            Console.WriteLine($"m={m},n={n}");
                            Console.WriteLine($"{max_rateerr:e20}");

                            Console.WriteLine($"mcount: {param.Count(item => item.val.Sign != Sign.Plus)}");

                            if (param.Any(v => MultiPrecision<Pow2.N128>.IsNaN(v.val))) {
                                return false;
                            }

                            if (coefs > 16 && max_rateerr > $"1e-{coefs / 2}") {
                                return false;
                            }

                            if (coefs > 16 && max_rateerr > "1e-135") {
                                coefs += 10;
                                break;
                            }

                            if (coefs > 16 && max_rateerr > "1e-140") {
                                coefs += 5;
                                break;
                            }

                            if (max_rateerr > "1e-145") {
                                break;
                            }

                            if (max_rateerr < "1e-150" &&
                                !CurveFittingUtils.HasLossDigitsPolynomialCoef(param[..m], 0, xs[^1] - xs[0]) &&
                                !CurveFittingUtils.HasLossDigitsPolynomialCoef(param[m..], 0, xs[^1] - xs[0])) {

                                sw.WriteLine($"p=[{umin},{umax}]");
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

                Segmenter<Pow2.N128> segmenter = new(ranges, approximate);
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
