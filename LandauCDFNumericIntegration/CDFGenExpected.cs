using LandauPadeApprox;
using MultiPrecision;

namespace LandauCDFMinusAsymptotic {
    class CDFGenExpected {
        static void Main_() {
            using StreamReader sr = new("../../../../results_disused/asymptotic_pade_pdfintegrate_precision103.csv");

            List<(MultiPrecision<N12> x0, MultiPrecision<N12> x1, MultiPrecision<N12> cdf)> cdf_expecteds = [];

            sr.ReadLine();
            while (!sr.EndOfStream) {
                string? line = sr.ReadLine();

                if (string.IsNullOrWhiteSpace(line)) {
                    break;
                }

                string[] line_split = line.Split(',');

                MultiPrecision<N12> x0 = line_split[0];
                MultiPrecision<N12> x1 = line_split[1];
                MultiPrecision<N12> cdf_append = line_split[2];

                cdf_expecteds.Add((x0, x1, cdf_append));
            }

            sr.Close();

            const double asymptotic_plus_threshold = 256, asymptotic_minus_threshold = -8;

            List<MultiPrecision<N12>> xs = [];

            for (MultiPrecision<N12> x = 0; x < 1; x += 1d / 2048) {
                xs.Add(x);
            }
            for (MultiPrecision<N12> h = 1d / 1024, x0 = 1, x1 = 2; x1 <= 8192; h *= 2, x0 *= 2, x1 *= 2) {
                for (MultiPrecision<N12> x = x0; x < x1; x += h) {
                    xs.Add(x);
                }
            }
            xs.Add(8192);

            for (MultiPrecision<N12> x = 0; x < 1; x += 1d / 2048) {
                xs.Add(-x);
            }
            for (MultiPrecision<N12> h = 1d / 1024, x0 = 1, x1 = 2; x1 <= 16; h *= 2, x0 *= 2, x1 *= 2) {
                for (MultiPrecision<N12> x = x0; x < x1; x += h) {
                    xs.Add(-x);
                }
            }
            for (MultiPrecision<N12> x = 16; x <= 20; x += 1d / 128) {
                xs.Add(-x);
            }

            xs.Sort();
            {
                using StreamWriter sw_lower = new("../../../../results_disused/cdf_lower_precision103.csv");
                sw_lower.WriteLine("lambda,cdf");


                foreach (MultiPrecision<N12> x in xs) {
                    if (x <= asymptotic_minus_threshold) {
                        MultiPrecision<N12> cdf = CDFPadeN12.Value(x);

                        Console.WriteLine($"{x},{cdf:e8}");
                        sw_lower.WriteLine($"{x},{cdf}");
                    }
                    else {
                        break;
                    }
                }

                MultiPrecision<N12> sum_cdf = CDFPadeN12.Value(asymptotic_minus_threshold);

                foreach ((MultiPrecision<N12> x0, MultiPrecision<N12> x1, MultiPrecision<N12> cdf_append) in cdf_expecteds) {
                    if (x1 <= asymptotic_minus_threshold) {
                        continue;
                    }

                    sum_cdf += cdf_append;

                    Console.WriteLine($"{x1},{sum_cdf:e8}");
                    sw_lower.WriteLine($"{x1},{sum_cdf}");

                    if (x1 >= 0) {
                        break;
                    }
                }

                sw_lower.Close();
            }
            {

                xs.Reverse();
                cdf_expecteds.Reverse();

                using StreamWriter sw_upper = new("../../../../results_disused/cdf_upper_precision103.csv");
                sw_upper.WriteLine("lambda,ccdf,cdf");

                foreach (MultiPrecision<N12> x in xs) {
                    if (x >= asymptotic_plus_threshold) {
                        MultiPrecision<N12> ccdf = CDFPadeN12.Value(x, complementary: true);

                        Console.WriteLine($"{x},{ccdf:e8}");
                        sw_upper.WriteLine($"{x},{ccdf},{1 - ccdf}");
                    }
                    else {
                        break;
                    }
                }

                MultiPrecision<N12> sum_ccdf = CDFPadeN12.Value(asymptotic_plus_threshold, complementary: true);

                foreach ((MultiPrecision<N12> x0, MultiPrecision<N12> x1, MultiPrecision<N12> ccdf_append) in cdf_expecteds) {
                    if (x0 >= asymptotic_plus_threshold) {
                        continue;
                    }

                    sum_ccdf += ccdf_append;

                    Console.WriteLine($"{x0},{sum_ccdf:e8}");
                    sw_upper.WriteLine($"{x0},{sum_ccdf},{1 - sum_ccdf}");

                    if (x0 <= 0) {
                        break;
                    }
                }

                sw_upper.Close();
            }
        }
    }
}