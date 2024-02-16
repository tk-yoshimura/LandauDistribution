using MultiPrecision;

namespace AsymptoticEvalError {
    class PDF {
        static void Main() {
            List<(MultiPrecision<N12> lambda, MultiPrecision<N12> pdf)> expecteds = ReadExpecteds();

            SummaryNegativeSide(expecteds);
            SummaryPositiveSide(expecteds);

            Console.WriteLine("END");
            Console.Read();
        }

        private static List<(MultiPrecision<N12> lambda, MultiPrecision<N12> pdf)> ReadExpecteds() {
            List<(MultiPrecision<N12> lambda, MultiPrecision<N12> pdf)> expecteds = [];
            using StreamReader stream = new("../../../../../results_disused/integrate_scaled_pdf_precision100.csv");

            for (int i = 0; i < 3; i++) {
                stream.ReadLine();
            }

            while (!stream.EndOfStream) {
                string? line = stream.ReadLine();

                if (string.IsNullOrWhiteSpace(line)) {
                    break;
                }

                string[] item = line.Split(',');

                MultiPrecision<N12> lambda = item[0], scaled_pdf = item[1];
                MultiPrecision<N12> pdf;

                if (lambda.Sign == Sign.Plus) {
                    pdf = scaled_pdf / (lambda * lambda + MultiPrecision<N12>.PI * MultiPrecision<N12>.PI);
                }
                else {
                    MultiPrecision<N12> sigma = PDFNegativeSide<N12>.Sigma(lambda);
                    pdf = scaled_pdf * PDFNegativeSide<N12>.Scale(sigma);
                }

                expecteds.Add((lambda, pdf));
            }

            return expecteds;
        }

        private static void SummaryNegativeSide(List<(MultiPrecision<N12> lambda, MultiPrecision<N12> pdf)> expecteds) {
            using StreamWriter sw_neg = new("../../../../../results_disused/pdf_negative_asymptotic_error.csv");
            List<(MultiPrecision<N12> lambda, MultiPrecision<N12> pdf)> expecteds_neg = expecteds.Where((item) => item.lambda <= -2.0).ToList();

            sw_neg.Write("lambda");
            foreach (int terms in new int[] { 2, 4, 6, 8, 12, 16, 24, 32, 48, 64 }) {
                sw_neg.Write($",{terms} {nameof(terms)}");
            }
            sw_neg.Write("\n");

            foreach ((MultiPrecision<N12> lambda, MultiPrecision<N12> expected) in expecteds_neg) {
                sw_neg.Write($"{lambda}");

                foreach (int terms in new int[] { 2, 4, 6, 8, 12, 16, 24, 32, 48, 64 }) {
                    MultiPrecision<N12> actual = PDFNegativeSide<N12>.Value(lambda, terms);

                    MultiPrecision<N12> relative_error = MultiPrecision<N12>.Abs(expected - actual) / expected;

                    sw_neg.Write($",{relative_error:e4}");
                }

                sw_neg.Write("\n");
            }

            sw_neg.Flush();
        }

        private static void SummaryPositiveSide(List<(MultiPrecision<N12> lambda, MultiPrecision<N12> pdf)> expecteds) {
            using StreamWriter sw_pos = new("../../../../../results_disused/pdf_positive_asymptotic_error.csv");
            List<(MultiPrecision<N12> lambda, MultiPrecision<N12> pdf)> expecteds_pos = expecteds.Where((item) => item.lambda >= 4).ToList();

            sw_pos.Write("lambda");
            foreach (int terms in new int[] { 2, 4, 6, 8, 12, 16, 24, 32, 48, 64 }) {
                sw_pos.Write($",{terms} {nameof(terms)}");
            }
            sw_pos.Write("\n");

            foreach ((MultiPrecision<N12> lambda, MultiPrecision<N12> expected) in expecteds_pos) {
                sw_pos.Write($"{lambda}");

                foreach (int terms in new int[] { 2, 4, 6, 8, 12, 16, 24, 32, 48, 64 }) {
                    MultiPrecision<N12> actual = PDFPositiveSide<N12>.Value(lambda, terms);

                    MultiPrecision<N12> relative_error = MultiPrecision<N12>.Abs(expected - actual) / expected;

                    sw_pos.Write($",{relative_error:e4}");
                }

                sw_pos.Write("\n");
            }

            sw_pos.Flush();
        }
    }
}