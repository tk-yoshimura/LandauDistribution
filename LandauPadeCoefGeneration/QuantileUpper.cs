using MultiPrecision;
using MultiPrecisionAlgebra;
using MultiPrecisionCurveFitting;

namespace LandauPadeCoefGeneration {
    class QuantileUpper {
        static void Main_() {
            List<(MultiPrecision<Pow2.N64> pmin, MultiPrecision<Pow2.N64> pmax, MultiPrecision<Pow2.N64> limit_range)> ranges = [];

            for (MultiPrecision<Pow2.N64> pmin = 1; pmin < 256; pmin *= 2) {
                ranges.Add((pmin, pmin * 2, pmin / 256));
            }

            List<(MultiPrecision<Pow2.N64> p, MultiPrecision<Pow2.N64> y)> expecteds = [];

            using (StreamReader sr = new("../../../../results_disused/quantile_upper_precision152_scaled.csv")) {
                sr.ReadLine();

                while (!sr.EndOfStream) {
                    string? line = sr.ReadLine();

                    if (string.IsNullOrWhiteSpace(line)) {
                        break;
                    }

                    string[] line_split = line.Split(",");

                    MultiPrecision<Pow2.N64> p = line_split[0];
                    MultiPrecision<Pow2.N64> x = line_split[1];

                    if (p > ranges[^1].pmax) {
                        break;
                    }

                    expecteds.Add((p, x));
                }
            }

            using (StreamWriter sw = new("../../../../results_disused/pade_quantile_upper_precision151.csv")) {
                bool approximate(MultiPrecision<Pow2.N64> pmin, MultiPrecision<Pow2.N64> pmax) {
                    Console.WriteLine($"[{pmin}, {pmax}]");

                    List<(MultiPrecision<Pow2.N64> p, MultiPrecision<Pow2.N64> y)> expecteds_range
                        = expecteds.Where(item => item.p >= pmin && item.p <= pmax).ToList();

                    Console.WriteLine($"expecteds {expecteds_range.Count} samples");

                    Vector<Pow2.N64> xs = expecteds_range.Select(item => item.p - pmin).ToArray();
                    Vector<Pow2.N64> ys = expecteds_range.Select(item => item.y).ToArray();

                    for (int coefs = 5; coefs <= expecteds_range.Count / 2 && coefs <= 128; coefs++) {
                        foreach ((int m, int n) in CurveFittingUtils.EnumeratePadeDegree(coefs, 2)) {
                            PadeFitter<Pow2.N64> pade = new(xs, ys, m, n);

                            Vector<Pow2.N64> param = pade.ExecuteFitting();
                            Vector<Pow2.N64> errs = pade.Error(param);

                            MultiPrecision<Pow2.N64> max_rateerr = CurveFittingUtils.MaxRelativeError(ys, pade.FittingValue(xs, param));

                            Console.WriteLine($"m={m},n={n}");
                            Console.WriteLine($"{max_rateerr:e20}");

                            if (coefs > 8 && max_rateerr > "1e-15") {
                                return false;
                            }

                            if (coefs > 16 && max_rateerr > "1e-30") {
                                return false;
                            }

                            if (coefs > 32 && max_rateerr > "1e-60") {
                                return false;
                            }

                            if (max_rateerr > "1e-50") {
                                coefs += 16;
                                break;
                            }

                            if (max_rateerr > "1e-100") {
                                coefs += 8;
                                break;
                            }

                            if (max_rateerr > "1e-135") {
                                coefs += 4;
                                break;
                            }

                            if (max_rateerr > "1e-140") {
                                coefs += 2;
                                break;
                            }

                            if (max_rateerr > "1e-148") {
                                break;
                            }

                            if (max_rateerr < "1e-160") {
                                return false;
                            }

                            if (max_rateerr < "1e-151" &&
                                !CurveFittingUtils.HasLossDigitsPolynomialCoef(param[..m], 0, pmax - pmin) &&
                                !CurveFittingUtils.HasLossDigitsPolynomialCoef(param[m..], 0, pmax - pmin)) {

                                sw.WriteLine($"p=[{pmin},{pmax}]");
                                sw.WriteLine($"m={m},n={n}");
                                sw.WriteLine($"expecteds {expecteds_range.Count} samples");
                                sw.WriteLine($"sample rate {(double)expecteds_range.Count / (param.Dim - 1)}");
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
