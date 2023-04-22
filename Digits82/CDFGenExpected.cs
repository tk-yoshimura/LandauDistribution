using MultiPrecision;

namespace Digits82 {
    class CDFGenExpected {
        static void Main_() {
            using StreamReader sr = new("../../../../results_disused/asymptotic_pade_pdfintegrate_precision82.csv");

            List<(MultiPrecision<N12> x0, MultiPrecision<N12> x1, MultiPrecision<N12> cdf)> cdf_expecteds = new();

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

            const double asymptotic_plus_threshold = 128, asymptotic_minus_threshold = -8;

            List<MultiPrecision<N12>> xs = new();

            for (MultiPrecision<N12> x = 0; x < 1; x += 1d / 1024) {
                xs.Add(x);
            }
            for (MultiPrecision<N12> h = 1d / 1024, x0 = 1, x1 = 2; x1 <= 8192; h *= 2, x0 *= 2, x1 *= 2) {
                for (MultiPrecision<N12> x = x0; x < x1; x += h) {
                    xs.Add(x);
                }
            }
            xs.Add(8192);

            for (MultiPrecision<N12> x = 0; x < 1; x += 1d / 1024) {
                xs.Add(-x);
            }
            for (MultiPrecision<N12> h = 1d / 1024, x0 = 1, x1 = 2; x1 <= 16; h *= 2, x0 *= 2, x1 *= 2) {
                for (MultiPrecision<N12> x = x0; x < x1; x += h) {
                    xs.Add(-x);
                }
            }
            for (MultiPrecision<N12> x = 16; x <= 20; x += 1d / 64) {
                xs.Add(-x);
            }

            xs.Sort();
            {
                using StreamWriter sw_forward = new("../../../../results_disused/cdf_forward_precision82.csv");
                sw_forward.WriteLine("lambda,cdf");


                foreach (MultiPrecision<N12> x in xs) {
                    if (x <= asymptotic_minus_threshold) {
                        MultiPrecision<N12> cdf = CDF.Value(x);

                        Console.WriteLine($"{x},{cdf:e8}");
                        sw_forward.WriteLine($"{x},{cdf}");
                    }
                    else {
                        break;
                    }
                }

                MultiPrecision<N12> sum_cdf = CDF.Value(asymptotic_minus_threshold);

                foreach ((MultiPrecision<N12> x0, MultiPrecision<N12> x1, MultiPrecision<N12> cdf_append) in cdf_expecteds) {
                    if (x1 <= asymptotic_minus_threshold) {
                        continue;
                    }

                    sum_cdf += cdf_append;

                    Console.WriteLine($"{x1},{sum_cdf:e8}");
                    sw_forward.WriteLine($"{x1},{sum_cdf}");

                    if (x1 >= 0) {
                        break;
                    }
                }

                sw_forward.Close();
            }
            {

                xs.Reverse();
                cdf_expecteds.Reverse();

                using StreamWriter sw_backward = new("../../../../results_disused/cdf_backward_precision82.csv");
                sw_backward.WriteLine("lambda,ccdf,cdf");

                foreach (MultiPrecision<N12> x in xs) {
                    if (x >= asymptotic_plus_threshold) {
                        MultiPrecision<N12> ccdf = CDF.Value(x, is_complementary: true);

                        Console.WriteLine($"{x},{ccdf:e8}");
                        sw_backward.WriteLine($"{x},{ccdf},{1 - ccdf}");
                    }
                    else {
                        break;
                    }
                }

                MultiPrecision<N12> sum_ccdf = CDF.Value(asymptotic_plus_threshold, is_complementary: true);

                foreach ((MultiPrecision<N12> x0, MultiPrecision<N12> x1, MultiPrecision<N12> ccdf_append) in cdf_expecteds) {
                    if (x0 >= asymptotic_plus_threshold) {
                        continue;
                    }

                    sum_ccdf += ccdf_append;

                    Console.WriteLine($"{x0},{sum_ccdf:e8}");
                    sw_backward.WriteLine($"{x0},{sum_ccdf},{1 - sum_ccdf}");

                    if (x0 <= 0) {
                        break;
                    }
                }

                sw_backward.Close();
            }
        }
    }
}