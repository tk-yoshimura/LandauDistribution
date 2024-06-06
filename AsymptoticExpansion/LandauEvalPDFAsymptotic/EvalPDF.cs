using MultiPrecision;

namespace LandauEvalPDFAsymptotic {
    class EvalPDF {
        static void Main() {
            List<(MultiPrecision<N18> lambda, MultiPrecision<N18> pdf)> expecteds = ReadExpecteds();

            SummaryNegativeSide(expecteds);
            SummaryPositiveSide(expecteds);

            Console.WriteLine("END");
            Console.Read();
        }

        private static List<(MultiPrecision<N18> lambda, MultiPrecision<N18> pdf)> ReadExpecteds() {
            List<(MultiPrecision<N18> lambda, MultiPrecision<N18> pdf)> expecteds = [];
            using StreamReader stream = new("../../../../../results_disused/scaled_pdf_precision151.csv");

            for (int i = 0; i < 3; i++) {
                stream.ReadLine();
            }

            while (!stream.EndOfStream) {
                string? line = stream.ReadLine();

                if (string.IsNullOrWhiteSpace(line)) {
                    break;
                }

                string[] item = line.Split(',');

                MultiPrecision<N18> lambda = item[0], scaled_pdf = item[1];
                MultiPrecision<N18> pdf;

                if (lambda.Sign == Sign.Plus) {
                    pdf = scaled_pdf / (lambda * lambda + MultiPrecision<N18>.PI * MultiPrecision<N18>.PI);
                }
                else {
                    MultiPrecision<N18> sigma = PDFNegativeSide<N18>.Sigma(lambda);
                    pdf = scaled_pdf * PDFNegativeSide<N18>.Scale(sigma);
                }

                expecteds.Add((lambda, pdf));
            }

            return expecteds;
        }

        private static void SummaryNegativeSide(List<(MultiPrecision<N18> lambda, MultiPrecision<N18> pdf)> expecteds) {
            using StreamWriter sw_neg = new("../../../../../results_disused/pdf_negative_asymptotic_error.csv");
            List<(MultiPrecision<N18> lambda, MultiPrecision<N18> pdf)> expecteds_neg = expecteds.Where((item) => item.lambda <= -2.0).ToList();

            sw_neg.Write("lambda");
            foreach (int terms in new int[] { 2, 4, 6, 8, 12, 16, 24, 32, 48, 64 }) {
                sw_neg.Write($",{terms} {nameof(terms)}");
            }
            sw_neg.Write("\n");

            foreach ((MultiPrecision<N18> lambda, MultiPrecision<N18> expected) in expecteds_neg) {
                sw_neg.Write($"{lambda}");

                foreach (int terms in new int[] { 2, 4, 6, 8, 12, 16, 24, 32, 48, 64 }) {
                    MultiPrecision<N18> actual = PDFNegativeSide<N18>.Value(lambda, terms);

                    MultiPrecision<N18> relative_error = MultiPrecision<N18>.Abs(expected - actual) / expected;

                    sw_neg.Write($",{relative_error:e4}");
                }

                sw_neg.Write("\n");
            }

            sw_neg.Flush();
        }

        private static void SummaryPositiveSide(List<(MultiPrecision<N18> lambda, MultiPrecision<N18> pdf)> expecteds) {
            using StreamWriter sw_pos = new("../../../../../results_disused/pdf_positive_asymptotic_error.csv");
            List<(MultiPrecision<N18> lambda, MultiPrecision<N18> pdf)> expecteds_pos = expecteds.Where((item) => item.lambda >= 4).ToList();

            sw_pos.Write("lambda");
            foreach (int terms in new int[] { 2, 4, 6, 8, 12, 16, 24, 32, 48, 64 }) {
                sw_pos.Write($",{terms} {nameof(terms)}");
            }
            sw_pos.Write("\n");

            foreach ((MultiPrecision<N18> lambda, MultiPrecision<N18> expected) in expecteds_pos) {
                sw_pos.Write($"{lambda}");

                foreach (int terms in new int[] { 2, 4, 6, 8, 12, 16, 24, 32, 48, 64 }) {
                    MultiPrecision<N18> actual = PDFPositiveSide<N18>.Value(lambda, terms);

                    MultiPrecision<N18> relative_error = MultiPrecision<N18>.Abs(expected - actual) / expected;

                    sw_pos.Write($",{relative_error:e4}");
                }

                sw_pos.Write("\n");
            }

            sw_pos.Flush();
        }
    }
}