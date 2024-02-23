using MultiPrecision;

namespace LandauEvalCDFAsymptotic {
    class EvalCDFIntegrate {
        static void Main_() {
            List<(MultiPrecision<N18> lambda0, MultiPrecision<N18> lambda1, MultiPrecision<N18> integral)> expecteds = ReadExpecteds();

            SummaryNegativeSide(expecteds);
            SummaryPositiveSide(expecteds);

            Console.WriteLine("END");
            Console.Read();
        }

        private static List<(MultiPrecision<N18> lambda0, MultiPrecision<N18> lambda1, MultiPrecision<N18> integral)> ReadExpecteds() {
            List<(MultiPrecision<N18> lambda0, MultiPrecision<N18> lambda1, MultiPrecision<N18> integral)> expecteds = [];
            
            using StreamReader sr = new("../../../../../results_disused/cdfintegrate_precision152.csv");
            sr.ReadLine();

            while (!sr.EndOfStream) {
                string? line = sr.ReadLine();

                if (string.IsNullOrWhiteSpace(line)) {
                    break;
                }

                string[] item = line.Split(',');

                MultiPrecision<N18> lambda0 = item[0], lambda1 = item[1], cdf = item[2];

                expecteds.Add((lambda0, lambda1, cdf));
            }

            return expecteds;
        }

        private static void SummaryNegativeSide(List<(MultiPrecision<N18> lambda0, MultiPrecision<N18> lambda1, MultiPrecision<N18> integral)> expecteds) {
            StreamWriter sw_neg = new("../../../../../results_disused/evalcdfintegrate_lower_precision152.csv");
            List<(MultiPrecision<N18> lambda0, MultiPrecision<N18> lambda1, MultiPrecision<N18> integral)> expecteds_neg = expecteds.Where((item) => item.lambda0 <= -2.0).ToList();

            sw_neg.WriteLine("lambda0,lambda1,error");

            foreach ((MultiPrecision<N18> lambda0, MultiPrecision<N18> lambda1, MultiPrecision<N18> expected) in expecteds_neg) {
                MultiPrecision<N18> actual = CDFNegativeSide<N18>.Value(lambda1, terms : 64) - CDFNegativeSide<N18>.Value(lambda0, terms : 64);
                MultiPrecision<N18> relative_error = MultiPrecision<N18>.Abs(expected - actual) / expected;
                sw_neg.WriteLine($"{lambda0},{lambda1},{relative_error:e15}");
            }

            sw_neg.Flush();
        }

        private static void SummaryPositiveSide(List<(MultiPrecision<N18> lambda0, MultiPrecision<N18> lambda1, MultiPrecision<N18> integral)> expecteds) {
            StreamWriter sw_pos = new("../../../../../results_disused/evalcdfintegrate_upper_precision152.csv");
            List<(MultiPrecision<N18> lambda0, MultiPrecision<N18> lambda1, MultiPrecision<N18> integral)> expecteds_pos = expecteds.Where((item) => item.lambda0 >= 256.0).Reverse().ToList();

            sw_pos.WriteLine("lambda0,lambda1,error");

            foreach ((MultiPrecision<N18> lambda0, MultiPrecision<N18> lambda1, MultiPrecision<N18> expected) in expecteds_pos) {
                MultiPrecision<N18> actual = CDFPositiveSide<N18>.Value(lambda0, terms : 64) - CDFPositiveSide<N18>.Value(lambda1, terms : 64);
                MultiPrecision<N18> relative_error = MultiPrecision<N18>.Abs(expected - actual) / expected;
                sw_pos.WriteLine($"{lambda0},{lambda1},{relative_error:e15}");
            }

            sw_pos.Flush();
        }
    }
}