using MultiPrecision;

namespace Digits100 {
    class SummaryCDF {
        static void Main_() {
            List<double> xs = new();

            for (double x = 0; x < 1; x += 1d / 4096) {
                xs.Add(x);
            }
            for (double h = 1d / 2048, x0 = 1, x1 = 2; x1 <= 4096; h *= 2, x0 *= 2, x1 *= 2) {
                for (double x = x0; x < x1; x += h) {
                    xs.Add(x);
                }
            }
            for (double x = 4096; x <= Math.ScaleB(1, 50); x *= 2) {
                xs.Add(x);
            }

            for (double x = 1d / 4096; x < 1; x += 1d / 4096) {
                xs.Add(-x);
            }
            for (double h = 1d / 2048, x0 = 1, x1 = 2; x1 <= 16; h *= 2, x0 *= 2, x1 *= 2) {
                for (double x = x0; x < x1; x += h) {
                    xs.Add(-x);
                }
            }
            for (double x = 16; x <= 20; x += 1d / 256) {
                xs.Add(-x);
            }

            xs.Sort();

            {
                using StreamWriter sw = new("../../../../results_disused/asymptotic_pade_cdf_precision103.csv");
                sw.WriteLine("lambda,cdf,ccdf");

                foreach (MultiPrecision<N12> x in xs) {
                    MultiPrecision<N12> cdf = CDF.Value(x), ccdf = CDF.Value(x, is_complementary: true);

                    Console.WriteLine($"{x},{cdf:e16},{ccdf:e16}");
                    sw.WriteLine($"{x},{cdf:e103},{ccdf:e103}");
                }

                sw.Flush();
            }

            {
                using StreamReader sr = new("../../../../results_disused/cdf_forward_precision103.csv");

                sr.ReadLine();

                using StreamWriter sw = new("../../../../results_disused/asymptotic_pade_cdf_precision103_evalerr.csv");

                sw.WriteLine("lambda,cdf,error");

                while (!sr.EndOfStream) {
                    string? line = sr.ReadLine();

                    if (string.IsNullOrWhiteSpace(line)) {
                        break;
                    }

                    string[] line_split = line.Split(',');

                    MultiPrecision<N12> x = line_split[0], expected = line_split[1];
                    MultiPrecision<N12> y = CDF.Value(x);
                    MultiPrecision<N12> err = MultiPrecision<N12>.Abs(y - expected) / expected;

                    Console.WriteLine($"{x},{y:e16},{err:e8}");
                    sw.WriteLine($"{x},{y},{err:e8}");
                    sw.Flush();
                }
            }

            {
                using StreamReader sr = new("../../../../results_disused/cdf_backward_precision103.csv");

                sr.ReadLine();

                using StreamWriter sw = new("../../../../results_disused/asymptotic_pade_ccdf_precision103_evalerr.csv");

                sw.WriteLine("lambda,ccdf,error");

                while (!sr.EndOfStream) {
                    string? line = sr.ReadLine();

                    if (string.IsNullOrWhiteSpace(line)) {
                        break;
                    }

                    string[] line_split = line.Split(',');

                    MultiPrecision<N12> x = line_split[0], expected = line_split[1];
                    MultiPrecision<N12> y = CDF.Value(x, is_complementary: true);
                    MultiPrecision<N12> err = MultiPrecision<N12>.Abs(y - expected) / expected;

                    Console.WriteLine($"{x},{y:e16},{err:e8}");
                    sw.WriteLine($"{x},{y},{err:e8}");
                    sw.Flush();
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}