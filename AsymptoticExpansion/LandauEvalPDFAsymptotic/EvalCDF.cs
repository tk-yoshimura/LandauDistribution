using MultiPrecision;

namespace LandauEvalErrorAsymptoticPDF {
    class EvalCDF {
        static void Main_() {
            List<(MultiPrecision<N12> lambda, MultiPrecision<N12> cdf)> expecteds_lower, expecteds_upper;
            (expecteds_lower, expecteds_upper) = ReadExpecteds();

            SummaryNegativeSide(expecteds_lower);
            SummaryPositiveSide(expecteds_upper);

            Console.WriteLine("END");
            Console.Read();
        }

        private static (List<(MultiPrecision<N12> lambda, MultiPrecision<N12> cdf)> expecteds_lower, List<(MultiPrecision<N12> lambda, MultiPrecision<N12> cdf)> expecteds_upper) ReadExpecteds() {
            List<(MultiPrecision<N12> lambda, MultiPrecision<N12> cdf)> expecteds_lower = [];
            List<(MultiPrecision<N12> lambda, MultiPrecision<N12> cdf)> expecteds_upper = [];

            using StreamReader stream_lower = new("../../../../../results_disused/cdf_lower_precision70.csv");
            stream_lower.ReadLine();

            while (!stream_lower.EndOfStream) {
                string? line = stream_lower.ReadLine();

                if (string.IsNullOrWhiteSpace(line)) {
                    break;
                }

                string[] item = line.Split(',');

                MultiPrecision<N12> lambda = item[0], cdf = item[1];

                expecteds_lower.Add((lambda, cdf));
            }

            using StreamReader stream_upper = new("../../../../../results_disused/cdf_upper_precision70.csv");
            stream_upper.ReadLine();

            while (!stream_upper.EndOfStream) {
                string? line = stream_upper.ReadLine();

                if (string.IsNullOrWhiteSpace(line)) {
                    break;
                }

                string[] item = line.Split(',');

                MultiPrecision<N12> lambda = item[0], cdf = item[1];

                expecteds_upper.Add((lambda, cdf));
            }

            return (expecteds_lower, expecteds_upper);
        }

        private static void SummaryNegativeSide(List<(MultiPrecision<N12> lambda, MultiPrecision<N12> cdf)> expecteds_lower) {
            StreamWriter sw_neg = new("../../../../../results_disused/cdf_negative_asymptotic_error.csv");
            List<(MultiPrecision<N12> lambda, MultiPrecision<N12> cdf)> expecteds_neg = expecteds_lower.Where((item) => item.lambda <= -2.0).ToList();

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

        private static void SummaryPositiveSide(List<(MultiPrecision<N12> lambda, MultiPrecision<N12> cdf)> expecteds_upper) {
            StreamWriter sw_pos = new("../../../../../results_disused/cdf_positive_asymptotic_error.csv");
            List<(MultiPrecision<N12> lambda, MultiPrecision<N12> cdf)> expecteds_pos =
                [.. expecteds_upper.Where((item) => item.lambda >= 4).OrderBy(item => item.lambda)];

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