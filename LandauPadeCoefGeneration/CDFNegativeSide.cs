using MultiPrecision;
using MultiPrecisionAlgebra;
using MultiPrecisionCurveFitting;

namespace LandauPadeCoefGeneration {
    class CDFNegativeSide {
        static void Main_() {
            List<(MultiPrecision<Pow2.N64> lambda, MultiPrecision<Pow2.N64> scaled_cdf)> expecteds = ReadExpacted();

            Console.WriteLine($"expected: {expecteds.Count} loaded");

            List<(MultiPrecision<Pow2.N64> min, MultiPrecision<Pow2.N64> max, MultiPrecision<Pow2.N64> minrange)> ranges = [
                (0, 4, 1d / 256),
                (4, 8, 1d / 128),
                (8, 10, 1d / 64)
            ];

            using (StreamWriter sw = new("../../../../results_disused/pade_cdf_precision152_minus.csv")) {
                bool approximate(MultiPrecision<Pow2.N64> xmin, MultiPrecision<Pow2.N64> xmax) {
                    Console.WriteLine($"[{xmin}, {xmax}]");

                    List<(MultiPrecision<Pow2.N64> x, MultiPrecision<Pow2.N64> y)> expecteds_range
                        = expecteds.Where(item => item.lambda >= xmin && item.lambda <= xmax).ToList();

                    Console.WriteLine($"expecteds {expecteds_range.Count} samples");

                    Vector<Pow2.N64> xs_forward = expecteds_range.Select(item => item.x - xmin).ToArray();
                    Vector<Pow2.N64> xs_backward = expecteds_range.Select(item => xmax - item.x).ToArray();
                    Vector<Pow2.N64> ys = expecteds_range.Select(item => item.y).ToArray();

                    for (int coefs = 5; coefs <= expecteds_range.Count / 2 && coefs <= 128; coefs++) {
                        foreach ((int m, int n) in CurveFittingUtils.EnumeratePadeDegree(coefs, 2)) {
                            foreach ((bool forward, Vector<Pow2.N64> xs) in new[] { (true, xs_forward), (false, xs_backward) }) {

                                PadeFitter<Pow2.N64> pade = new(xs, ys, m, n);

                                Vector<Pow2.N64> param = pade.ExecuteFitting();
                                Vector<Pow2.N64> errs = pade.Error(param);

                                MultiPrecision<Pow2.N64> max_rateerr = CurveFittingUtils.MaxRelativeError(ys, pade.FittingValue(xs, param));

                                Console.WriteLine($"m={m},n={n},{(forward ? "forward" : "backward")}");
                                Console.WriteLine($"{max_rateerr:e20}");

                                if (coefs > 8 && max_rateerr > "1e-12") {
                                    return false;
                                }

                                if (coefs > 32 && max_rateerr > "1e-50") {
                                    return false;
                                }

                                if (!forward && max_rateerr > "1e-145") {
                                    break;
                                }

                                if (max_rateerr < "2e-152" &&
                                    !CurveFittingUtils.HasLossDigitsPolynomialCoef(param[..m], 0, xmax - xmin) &&
                                    !CurveFittingUtils.HasLossDigitsPolynomialCoef(param[m..], 0, xmax - xmin)) {

                                    sw.WriteLine($"x=[{xmin},{xmax}]");
                                    sw.WriteLine($"{(forward ? "forward" : "backward")}");
                                    sw.WriteLine($"m={m},n={n}");
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

        private static List<(MultiPrecision<Pow2.N64> lambda, MultiPrecision<Pow2.N64> scaled_cdf)> ReadExpacted() {

            List<(MultiPrecision<Pow2.N64> lambda, MultiPrecision<Pow2.N64> scaled_cdf)> expecteds = [];
            StreamReader stream = new("../../../../results_disused/cdf_lower_precision152.csv");
            for (int i = 0; i < 3; i++) {
                stream.ReadLine();
            }

            while (!stream.EndOfStream) {
                string? line = stream.ReadLine();

                if (string.IsNullOrWhiteSpace(line)) {
                    break;
                }

                if (!line.StartsWith('-')) {
                    continue;
                }

                string[] item = line.Split(',');

                MultiPrecision<Pow2.N64> lambda = item[0], cdf = item[1];

                MultiPrecision<Pow2.N64> sigma = MultiPrecision<Pow2.N64>.Exp(-lambda - 1);

                MultiPrecision<Pow2.N64> scaled_cdf = cdf * MultiPrecision<Pow2.N64>.Sqrt(2 * MultiPrecision<Pow2.N64>.PI * sigma) * MultiPrecision<Pow2.N64>.Exp(sigma);

                expecteds.Add((-lambda, scaled_cdf));
            }

            return expecteds;
        }
    }
}
