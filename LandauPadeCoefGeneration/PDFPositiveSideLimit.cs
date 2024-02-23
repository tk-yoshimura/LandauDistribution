using MultiPrecision;
using MultiPrecisionAlgebra;
using MultiPrecisionCurveFitting;

namespace LandauPadeCoefGeneration {
    class PDFPositiveSideLimit {
        static void Main() {
            List<(MultiPrecision<Pow2.N64> lambda, MultiPrecision<Pow2.N64> scaled_pdf)> expecteds = ReadExpacted();

            Console.WriteLine($"expected: {expecteds.Count} loaded");

            List<(MultiPrecision<Pow2.N64> min, MultiPrecision<Pow2.N64> max, MultiPrecision<Pow2.N64> minrange)> ranges = [
                (0, 1d / 2048, 1d / 1048576),
            ];

            using (StreamWriter sw = new("../../../../results_disused/pade_pdf_pluslimit_invert_precision152.csv")) {
                bool approximate(MultiPrecision<Pow2.N64> xmin, MultiPrecision<Pow2.N64> xmax) {
                    if (xmin > 0) {
                        return false;
                    }

                    Console.WriteLine($"[{xmin}, {xmax}]");

                    List<(MultiPrecision<Pow2.N64> x, MultiPrecision<Pow2.N64> y)> expecteds_range
                        = expecteds.Where(item => item.lambda <= xmax).ToList();

                    Console.WriteLine($"expecteds {expecteds_range.Count} samples");

                    MultiPrecision<Pow2.N64> y0 = expecteds_range.First().y;

                    Vector<Pow2.N64> xs = expecteds_range.Select(item => item.x).ToArray();
                    Vector<Pow2.N64> ys = expecteds_range.Select(item => item.y).ToArray();

                    xs /= xmax;
                    xs = (MultiPrecision<Pow2.N64>.Sqrt, xs);
                    xs[0] = 0;

                    for (int coefs = 248; coefs <= expecteds_range.Count / 2 && coefs <= 300; coefs++) {
                        foreach ((int m, int n) in CurveFittingUtils.EnumeratePadeDegree(coefs, 0)) {
                            PadeFitter<Pow2.N64> pade = new(xs, ys, m, n, intercept: y0);

                            Vector<Pow2.N64> param = pade.ExecuteFitting();
                            Vector<Pow2.N64> errs = pade.Error(param);

                            MultiPrecision<Pow2.N64> max_rateerr = CurveFittingUtils.MaxRelativeError(ys, pade.FittingValue(xs, param));

                            Console.WriteLine($"m={m},n={n}");
                            Console.WriteLine($"{max_rateerr:e20}");


                            if (max_rateerr > "1e-145") {
                                break;
                            }

                            if (max_rateerr < "1e-152" &&
                                !CurveFittingUtils.HasLossDigitsPolynomialCoef(param[..m], 0, xs[^1]) &&
                                !CurveFittingUtils.HasLossDigitsPolynomialCoef(param[m..], 0, xs[^1])) {

                                sw.WriteLine($"x=[{xmin},{xmax}]");
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
            StreamReader stream = new("../../../../results_disused/scaled_pdf_pluslimit_invert_precision162.csv");
            for (int i = 0; i < 3; i++) {
                stream.ReadLine();
            }

            while (!stream.EndOfStream) {
                string? line = stream.ReadLine();

                if (string.IsNullOrWhiteSpace(line)) {
                    break;
                }

                string[] item = line.Split(',');

                MultiPrecision<Pow2.N64> lambda = item[0], scaled_pdf = item[1];

                expecteds.Add((lambda, scaled_pdf));
            }

            return expecteds;
        }
    }
}
