using LandauPadeApprox;
using MultiPrecision;

namespace LandauEvalPadeApprox {
    class EvalCDF {
        static void Main_() {
            List<double> xs = [];

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
                using StreamWriter sw = new("../../../../results_disused/evalpade_cdf_precision152.csv");
                sw.WriteLine("lambda,cdf,ccdf");

                foreach (MultiPrecision<Pow2.N16> x in xs) {
                    MultiPrecision<Pow2.N16> cdf = CDFPadeN16.Value(x), ccdf = CDFPadeN16.Value(x, complementary: true);

                    Console.WriteLine($"{x},{cdf:e16},{ccdf:e16}");
                    sw.WriteLine($"{x},{cdf},{ccdf}");
                }

                sw.Flush();
            }

            {
                using StreamReader sr = new("../../../../results_disused/cdf_lower_precision152.csv");

                sr.ReadLine();

                using StreamWriter sw = new("../../../../results_disused/evalpade_cdf_precision152_evalerr.csv");

                sw.WriteLine("lambda,cdf,error");

                while (!sr.EndOfStream) {
                    string? line = sr.ReadLine();

                    if (string.IsNullOrWhiteSpace(line)) {
                        break;
                    }

                    string[] line_split = line.Split(',');

                    MultiPrecision<N24> x = line_split[0], expected = line_split[1];
                    MultiPrecision<N24> y = CDFPadeN16.Value(x.Convert<Pow2.N16>()).Convert<N24>();
                    MultiPrecision<N24> err = MultiPrecision<N24>.Abs(y - expected) / expected;

                    Console.WriteLine($"{x},{y:e16},{err:e8}");
                    sw.WriteLine($"{x},{y},{err:e8}");
                    sw.Flush();
                }
            }

            {
                using StreamReader sr = new("../../../../results_disused/cdf_upper_precision152.csv");

                sr.ReadLine();

                using StreamWriter sw = new("../../../../results_disused/evalpade_ccdf_precision152_evalerr.csv");

                sw.WriteLine("lambda,ccdf,error");

                while (!sr.EndOfStream) {
                    string? line = sr.ReadLine();

                    if (string.IsNullOrWhiteSpace(line)) {
                        break;
                    }

                    string[] line_split = line.Split(',');

                    MultiPrecision<N24> x = line_split[0], expected = line_split[1];
                    MultiPrecision<N24> y = CDFPadeN16.Value(x.Convert<Pow2.N16>(), complementary: true).Convert<N24>();
                    MultiPrecision<N24> err = MultiPrecision<N24>.Abs(y - expected) / expected;

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