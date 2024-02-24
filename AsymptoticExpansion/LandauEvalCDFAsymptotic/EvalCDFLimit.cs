using MultiPrecision;

namespace LandauEvalCDFAsymptotic {
    class EvalCDFLimit {
        static void Main_() {
            List<MultiPrecision<N18>> xs = [];

            for (MultiPrecision<N18> x = 0; x < 1; x += 1d / 4096) {
                xs.Add(x);
            }
            for (MultiPrecision<N18> h = 1d / 2048, x0 = 1, x1 = 2; x1 <= 4096; h *= 2, x0 *= 2, x1 *= 2) {
                for (MultiPrecision<N18> x = x0; x < x1; x += h) {
                    xs.Add(x);
                }
            }
            xs.Add(4096);

            for (MultiPrecision<N18> x = 0; x < 1; x += 1d / 4096) {
                xs.Add(-x);
            }
            for (MultiPrecision<N18> h = 1d / 2048, x0 = 1, x1 = 2; x1 <= 16; h *= 2, x0 *= 2, x1 *= 2) {
                for (MultiPrecision<N18> x = x0; x < x1; x += h) {
                    xs.Add(-x);
                }
            }
            for (MultiPrecision<N18> x = 16; x <= 20; x += 1d / 256) {
                xs.Add(-x);
            }

            xs.Sort();

            using (StreamWriter sw = new("../../../../../results_disused/cdfasymp_lower_precision152.csv")) {
                sw.WriteLine("x,cdf");

                foreach (MultiPrecision<N18> x in xs.Where(x => x <= -9.9375)) {
                    MultiPrecision<N18> cdf = CDFNegativeSide<N18>.Value(x, terms: 64);

                    sw.WriteLine($"{x},{cdf}");
                }
            }

            using (StreamWriter sw = new("../../../../../results_disused/cdfasymp_upper_precision152.csv")) {
                sw.WriteLine("x,cdf");

                foreach (MultiPrecision<N18> x in xs.Where(x => x >= 2048)) {
                    MultiPrecision<N18> cdf = CDFPositiveSide<N18>.Value(x, terms: 64);

                    sw.WriteLine($"{x},{cdf}");
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
