using MultiPrecision;

namespace LandauCDFMinusAsymptotic {
    class CDFGenExpected {
        static void Main() {
            List<(MultiPrecision<Pow2.N32> x0, MultiPrecision<Pow2.N32> x1, MultiPrecision<Pow2.N32> integral)> integrals = [];
            List<(MultiPrecision<Pow2.N32> x, MultiPrecision<Pow2.N32> cdf)> cdf_lower = [], cdf_upper = [];

            using (StreamReader sr = new("../../../../results_disused/cdfintegrate_precision152_2.csv")) {
                sr.ReadLine();
                while (!sr.EndOfStream) {
                    string? line = sr.ReadLine();

                    if (string.IsNullOrWhiteSpace(line)) {
                        break;
                    }

                    string[] line_split = line.Split(',');

                    MultiPrecision<Pow2.N32> x0 = line_split[0];
                    MultiPrecision<Pow2.N32> x1 = line_split[1];
                    MultiPrecision<Pow2.N32> integral = line_split[2];

                    integrals.Add((x0, x1, integral));
                }
            }

            using (StreamReader sr = new("../../../../results_disused/cdfasymp_lower_precision152.csv")) {
                sr.ReadLine();
                while (!sr.EndOfStream) {
                    string? line = sr.ReadLine();

                    if (string.IsNullOrWhiteSpace(line)) {
                        break;
                    }

                    string[] line_split = line.Split(',');

                    MultiPrecision<Pow2.N32> x = line_split[0];
                    MultiPrecision<Pow2.N32> cdf = line_split[1];

                    cdf_lower.Add((x, cdf));
                }
            }

            using (StreamReader sr = new("../../../../results_disused/cdfasymp_upper_precision152.csv")) {
                sr.ReadLine();
                while (!sr.EndOfStream) {
                    string? line = sr.ReadLine();

                    if (string.IsNullOrWhiteSpace(line)) {
                        break;
                    }

                    string[] line_split = line.Split(',');

                    MultiPrecision<Pow2.N32> x = line_split[0];
                    MultiPrecision<Pow2.N32> cdf = line_split[1];

                    cdf_upper.Add((x, cdf));
                }
            }

            MultiPrecision<Pow2.N32> sum_cdf, sum_ccdf;

            using (StreamWriter sw_lower = new("../../../../results_disused/cdf_lower_precision150.csv")) {
                sw_lower.WriteLine("lambda,cdf");

                foreach ((MultiPrecision<Pow2.N32> x, MultiPrecision<Pow2.N32> cdf) in cdf_lower) {
                    Console.WriteLine($"{x},{cdf:e8}");
                    sw_lower.WriteLine($"{x},{cdf:e155}");
                }

                (MultiPrecision<Pow2.N32> asymptotic_minus_threshold, sum_cdf) = cdf_lower[^1];

                foreach ((MultiPrecision<Pow2.N32> x0, MultiPrecision<Pow2.N32> x1, MultiPrecision<Pow2.N32> integral) in integrals) {
                    if (x1 <= asymptotic_minus_threshold) {
                        continue;
                    }

                    sum_cdf += integral;

                    Console.WriteLine($"{x1},{sum_cdf:e8}");
                    sw_lower.WriteLine($"{x1},{sum_cdf:e155}");

                    if (x1 >= 0) {
                        break;
                    }
                }
            }

            using (StreamWriter sw_upper = new("../../../../results_disused/cdf_upper_precision150.csv")) {
                sw_upper.WriteLine("lambda,cdf");

                cdf_upper.Reverse();
                integrals.Reverse();

                foreach ((MultiPrecision<Pow2.N32> x, MultiPrecision<Pow2.N32> cdf) in cdf_upper) {
                    Console.WriteLine($"{x},{cdf:e8}");
                    sw_upper.WriteLine($"{x},{cdf:e155}");
                }

                (MultiPrecision<Pow2.N32> asymptotic_plus_threshold, sum_ccdf) = cdf_upper[^1];

                foreach ((MultiPrecision<Pow2.N32> x0, MultiPrecision<Pow2.N32> x1, MultiPrecision<Pow2.N32> integral) in integrals) {
                    if (x0 >= asymptotic_plus_threshold) {
                        continue;
                    }

                    sum_ccdf += integral;

                    Console.WriteLine($"{x0},{sum_ccdf:e8}");
                    sw_upper.WriteLine($"{x0},{sum_ccdf:e155}");

                    if (x0 <= 0) {
                        break;
                    }
                }
            }

            Console.WriteLine($"check: {sum_cdf + sum_ccdf}");
            Console.WriteLine($"error: {MultiPrecision<Pow2.N32>.Abs(sum_cdf + sum_ccdf - 1):e20}");

            Console.WriteLine("END");
            Console.Read();
        }
    }
}