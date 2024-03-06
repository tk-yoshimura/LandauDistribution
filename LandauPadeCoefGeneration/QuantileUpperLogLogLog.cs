using MultiPrecision;
using MultiPrecisionAlgebra;
using MultiPrecisionCurveFitting;

namespace LandauPadeCoefGeneration {
    class QuantileUpperLogLogLog {
        static void Main() {
            List<(MultiPrecision<Pow2.N64> pmin, MultiPrecision<Pow2.N64> pmax, MultiPrecision<Pow2.N64> limit_range)> ranges = [];

            for (MultiPrecision<Pow2.N64> pmin = 16; pmin < 512; pmin *= 2) {
                ranges.Add((pmin, pmin * 2, pmin / 128));
            }

            List<(MultiPrecision<Pow2.N64> p, MultiPrecision<Pow2.N64> y)> expecteds = [];

            using (StreamReader sr = new("../../../../results_disused/quantile_upper_precision152_loglog.csv")) {
                sr.ReadLine();

                while (!sr.EndOfStream) {
                    string? line = sr.ReadLine();

                    if (string.IsNullOrWhiteSpace(line)) {
                        break;
                    }

                    string[] line_split = line.Split(",");

                    MultiPrecision<Pow2.N64> p = line_split[0];
                    if (p < ranges[0].pmin) {
                        continue;
                    }
                    if (p > ranges[^1].pmax) {
                        break;
                    }

                    MultiPrecision<Pow2.N64> x = line_split[1];
                    MultiPrecision<Pow2.N64> u = MultiPrecision<Pow2.N64>.Log2(x);

                    expecteds.Add((p, u));
                }
            }

            using (StreamWriter sw = new("../../../../results_disused/pade_quantile_upper_precision155_logloglog.csv")) {
                bool approximate(MultiPrecision<Pow2.N64> pmin, MultiPrecision<Pow2.N64> pmax) {
                    if (pmin < 256) {
                        if (pmax - pmin > 32) {
                            return false;
                        }
                    }
                    else { 
                        if (pmax - pmin > 64) {
                            return false;
                        }
                    }

                    Console.WriteLine($"[{pmin}, {pmax}]");

                    List<(MultiPrecision<Pow2.N64> p, MultiPrecision<Pow2.N64> y)> expecteds_range
                        = expecteds.Where(item => item.p >= pmin && item.p <= pmax).ToList();

                    Console.WriteLine($"expecteds {expecteds_range.Count} samples");

                    Vector<Pow2.N64> xs = expecteds_range.Select(item => item.p - pmin).ToArray();
                    Vector<Pow2.N64> ys = expecteds_range.Select(item => item.y).ToArray();

                    MultiPrecision<Pow2.N64> error_rate = 1 / MultiPrecision<Pow2.N64>.Pow2(ys[0]); 

                    (long exp_scale, xs) = CurveFittingUtils.StandardizeExponent(xs);

                    Console.WriteLine($"scale={exp_scale}");
                    Console.WriteLine($"error_rate={error_rate:e10}");

                    for (int coefs = 5; coefs <= expecteds_range.Count / 2 && coefs <= 100; coefs++) {
                        foreach ((int m, int n) in CurveFittingUtils.EnumeratePadeDegree(coefs, 2)) {
                            PadeFitter<Pow2.N64> pade = new(xs, ys, m, n);

                            Vector<Pow2.N64> param = pade.ExecuteFitting();

                            MultiPrecision<Pow2.N64> max_rateerr = CurveFittingUtils.MaxRelativeError(ys, pade.FittingValue(xs, param));

                            Console.WriteLine($"m={m},n={n}");
                            Console.WriteLine($"{max_rateerr:e20}");

                            Console.WriteLine($"mcount: {param.Count(item => item.val.Sign != Sign.Plus)}");

                            if (coefs > 8 && max_rateerr > "1e-12" * error_rate) {
                                return false;
                            }

                            if (coefs > 32 && max_rateerr > "1e-45" * error_rate) {
                                return false;
                            }

                            if (coefs > 32 && max_rateerr > "1e-105") {
                                coefs += 4;
                                break;
                            }

                            if (max_rateerr > "1e-150" * error_rate) {
                                break;
                            }

                            if (max_rateerr >= "1e-155" * error_rate) {
                                continue;
                            }

                            if (CurveFittingUtils.HasLossDigitsPolynomialCoef(param[..m], 0, xs[^1] - xs[0]) ||
                                CurveFittingUtils.HasLossDigitsPolynomialCoef(param[m..], 0, xs[^1] - xs[0])) {
                                continue;
                            }

                            sw.WriteLine($"p=[{pmin},{pmax}]");
                            sw.WriteLine($"m={m},n={n}");
                            sw.WriteLine($"scale={exp_scale}");
                            sw.WriteLine($"error_rate={error_rate:e10}");
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

                            sw.WriteLine("absolute err");
                            sw.WriteLine($"{max_rateerr:e20}");
                            sw.Flush();

                            return true;
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
