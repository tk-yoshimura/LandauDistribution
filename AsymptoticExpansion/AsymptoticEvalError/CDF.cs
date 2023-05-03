using MultiPrecision;

namespace AsymptoticEvalError {
    class CDF {
        static void Main_() {
            List<(MultiPrecision<N12> lambda, MultiPrecision<N12> cdf)> expecteds_forward, expecteds_backward;
            (expecteds_forward, expecteds_backward) = ReadExpecteds();

            SummaryNegativeSide(expecteds_forward);
            SummaryPositiveSide(expecteds_backward);

            Console.WriteLine("END");
            Console.Read();
        }

        private static (List<(MultiPrecision<N12> lambda, MultiPrecision<N12> cdf)> expecteds_forward, List<(MultiPrecision<N12> lambda, MultiPrecision<N12> cdf)> expecteds_backward) ReadExpecteds() {
            List<(MultiPrecision<N12> lambda, MultiPrecision<N12> cdf)> expecteds_forward = new();
            List<(MultiPrecision<N12> lambda, MultiPrecision<N12> cdf)> expecteds_backward = new();

            using StreamReader stream_forward = new("../../../../../results_disused/cdf_forward_precision70.csv");
            stream_forward.ReadLine();

            while (!stream_forward.EndOfStream) {
                string? line = stream_forward.ReadLine();

                if (string.IsNullOrWhiteSpace(line)) {
                    break;
                }

                string[] item = line.Split(',');

                MultiPrecision<N12> lambda = item[0], cdf = item[1];

                expecteds_forward.Add((lambda, cdf));
            }

            using StreamReader stream_backward = new("../../../../../results_disused/cdf_backward_precision70.csv");
            stream_backward.ReadLine();

            while (!stream_backward.EndOfStream) {
                string? line = stream_backward.ReadLine();

                if (string.IsNullOrWhiteSpace(line)) {
                    break;
                }

                string[] item = line.Split(',');

                MultiPrecision<N12> lambda = item[0], cdf = item[1];

                expecteds_backward.Add((lambda, cdf));
            }

            return (expecteds_forward, expecteds_backward);
        }

        private static void SummaryNegativeSide(List<(MultiPrecision<N12> lambda, MultiPrecision<N12> cdf)> expecteds_forward) {
            StreamWriter sw_neg = new("../../../../../results_disused/cdf_negative_asymptotic_error.csv");
            List<(MultiPrecision<N12> lambda, MultiPrecision<N12> cdf)> expecteds_neg = expecteds_forward.Where((item) => item.lambda <= -2.0).ToList();

            sw_neg.Write("lambda");
            foreach (int terms in new int[] { 2, 4, 6, 8, 12, 16, 24, 32, 48, 64 }) {
                sw_neg.Write($",{terms} {nameof(terms)}");
            }
            sw_neg.Write("\n");

            foreach ((MultiPrecision<N12> lambda, MultiPrecision<N12> expected) in expecteds_neg) {
                sw_neg.Write($"{lambda}");

                foreach (int terms in new int[] { 2, 4, 6, 8, 12, 16, 24, 32, 48, 64 }) {
                    MultiPrecision<N12> actual = CDFNegativeSide<N12>.Value(lambda, terms);

                    MultiPrecision<N12> relative_error = MultiPrecision<N12>.Abs(expected - actual) / expected;

                    sw_neg.Write($",{relative_error:e4}");
                }

                sw_neg.Write("\n");
            }

            sw_neg.Flush();
        }

        private static void SummaryPositiveSide(List<(MultiPrecision<N12> lambda, MultiPrecision<N12> cdf)> expecteds_backward) {
            StreamWriter sw_pos = new("../../../../../results_disused/cdf_positive_asymptotic_error.csv");
            List<(MultiPrecision<N12> lambda, MultiPrecision<N12> cdf)> expecteds_pos = expecteds_backward
                .Where((item) => item.lambda >= 4).OrderBy(item => item.lambda).ToList();

            sw_pos.Write("lambda");
            foreach (int terms in new int[] { 2, 4, 6, 8, 12, 16, 24, 32, 48, 64 }) {
                sw_pos.Write($",{terms} {nameof(terms)}");
            }
            sw_pos.Write("\n");

            foreach ((MultiPrecision<N12> lambda, MultiPrecision<N12> expected) in expecteds_pos) {
                sw_pos.Write($"{lambda}");

                foreach (int terms in new int[] { 2, 4, 6, 8, 12, 16, 24, 32, 48, 64 }) {
                    MultiPrecision<N12> actual = CDFPositiveSide<N12>.Value(lambda, terms);

                    MultiPrecision<N12> relative_error = MultiPrecision<N12>.Abs(expected - actual) / expected;

                    sw_pos.Write($",{relative_error:e4}");
                }

                sw_pos.Write("\n");
            }

            sw_pos.Flush();
        }
    }
}