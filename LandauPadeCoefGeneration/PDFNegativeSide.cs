using MultiPrecision;
using MultiPrecisionAlgebra;
using MultiPrecisionCurveFitting;

namespace LandauPadeCoefGeneration {
    class PDFNegativeSide {
        static void Main_() {
            List<(MultiPrecision<Pow2.N64> lambda, MultiPrecision<Pow2.N64> scaled_pdf)> expecteds = ReadExpacted();

            Console.WriteLine($"expected: {expecteds.Count} loaded");

            List<(MultiPrecision<Pow2.N64> min, MultiPrecision<Pow2.N64> max, MultiPrecision<Pow2.N64> minrange)> ranges = [
                (0, 4, 1d / 256),
                (4, 8, 1d / 128),
                (8, 10, 1d / 64),
                (10, 12, 1d / 64)
            ];

            using (StreamWriter sw = new("../../../../results_disused/pade_pdf_precision153_minus.csv")) {
                bool approximate(MultiPrecision<Pow2.N64> xmin, MultiPrecision<Pow2.N64> xmax) {
                    Console.WriteLine($"[{xmin}, {xmax}]");

                    List<(MultiPrecision<Pow2.N64> x, MultiPrecision<Pow2.N64> y)> expecteds_range
                        = expecteds.Where(item => item.lambda >= xmin && item.lambda <= xmax).ToList();

                    Console.WriteLine($"expecteds {expecteds_range.Count} samples");

                    Vector<Pow2.N64> xs_forward = expecteds_range.Select(item => item.x - xmin).ToArray();
                    Vector<Pow2.N64> xs_backward = expecteds_range.Select(item => xmax - item.x).ToArray();
                    Vector<Pow2.N64> ys = expecteds_range.Select(item => item.y).ToArray();

                    for (int coefs = 5; coefs <= expecteds_range.Count / 8 && coefs <= 128; coefs++) {

                        foreach ((int m, int n) in CurveFittingUtils.EnumeratePadeDegree(coefs, 2)) {
                            bool skip = false;

                            foreach ((bool forward, Vector<Pow2.N64> xs) in new[] { (true, xs_forward), (false, xs_backward) }) {

                                PadeFitter<Pow2.N64> pade = new(xs, ys, m, n);

                                Vector<Pow2.N64> param = pade.ExecuteFitting();
                                Vector<Pow2.N64> errs = pade.Error(param);

                                MultiPrecision<Pow2.N64> max_rateerr = CurveFittingUtils.MaxRelativeError(ys, pade.FittingValue(xs, param));

                                Console.WriteLine($"m={m},n={n},{(forward ? "forward" : "backward")}");
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

                                if (!forward && max_rateerr > "1e-50") {
                                    coefs += 16;
                                    skip = true;
                                    break;
                                }

                                if (!forward && max_rateerr > "1e-100") {
                                    coefs += 8;
                                    skip = true;
                                    break;
                                }

                                if (!forward && max_rateerr > "1e-135") {
                                    coefs += 4;
                                    skip = true;
                                    break;
                                }

                                if (!forward && max_rateerr > "1e-140") {
                                    coefs += 2;
                                    skip = true;
                                    break;
                                }

                                if (!forward && max_rateerr > "1e-148") {
                                    skip = true;
                                    break;
                                }

                                if (max_rateerr < "1e-160") {
                                    return false;
                                }

                                if (max_rateerr < "1e-153" &&
                                    !CurveFittingUtils.HasLossDigitsPolynomialCoef(param[..m], 0, xmax - xmin) &&
                                    !CurveFittingUtils.HasLossDigitsPolynomialCoef(param[m..], 0, xmax - xmin)) {

                                    sw.WriteLine($"x=[{xmin},{xmax}]");
                                    sw.WriteLine($"{(forward ? "forward" : "backward")}");
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

                            if (skip) {
                                break;
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

        private static List<(MultiPrecision<Pow2.N64> lambda, MultiPrecision<Pow2.N64> scaled_pdf)> ReadExpacted() {

            List<(MultiPrecision<Pow2.N64> lambda, MultiPrecision<Pow2.N64> scaled_pdf)> expecteds = [];
            StreamReader stream = new("../../../../results_disused/scaled_pdf_precision151.csv");
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

                MultiPrecision<Pow2.N64> lambda = item[0], scaled_pdf = item[1];

                expecteds.Add((-lambda, scaled_pdf));
            }

            return expecteds;
        }
    }
}
