using MultiPrecision;

namespace Digits100 {
    class SummaryQuantile {
        static void Main_() {
            List<MultiPrecision<N12>> ps = new();

            int exp0 = -1;
            for (int h = 16384; h >= 1; exp0 *= 2, h /= 2) {
                for (int exp = exp0; exp > exp0 * 2; exp--) {
                    for (double v = 1; v > 0.5; v -= 1d / h) {
                        ps.Add(MultiPrecision<N12>.Ldexp(v, exp));
                    }
                }
            }
            for (int exp = exp0; exp > -16384; exp--) {
                ps.Add(MultiPrecision<N12>.Ldexp(1, exp));
            }
            for (int exp = -16384; exp >= -262144; exp -= 4) {
                ps.Add(MultiPrecision<N12>.Ldexp(1, exp));
            }

            ps.Reverse();

            {
                using StreamWriter sw = new("../../../../results_disused/quantile_cdf_precision102_evalerr.csv");

                sw.WriteLine("cdf,lambda,error");

                foreach (MultiPrecision<N12> p in ps) {
                    MultiPrecision<N12> lambda = Quantile.Value(p);
                    MultiPrecision<N12> q = CDF.Value(lambda);
                    MultiPrecision<N12> err = MultiPrecision<N12>.Abs(p - q) / p;

                    Console.WriteLine($"{p:e16},{lambda:e16},{err:e8}");
                    sw.WriteLine($"{p},{lambda},{err:e8}");
                    sw.Flush();
                }
            }

            {
                using StreamWriter sw = new("../../../../results_disused/quantile_ccdf_precision102_evalerr.csv");

                sw.WriteLine("ccdf,lambda,error");

                foreach (MultiPrecision<N12> p in ps) {
                    MultiPrecision<N12> lambda = Quantile.Value(p, is_complementary: true);
                    MultiPrecision<N12> q = CDF.Value(lambda, is_complementary: true);
                    MultiPrecision<N12> err = MultiPrecision<N12>.Abs(p - q) / p;

                    Console.WriteLine($"{p:e16},{lambda:e16},{err:e8}");
                    sw.WriteLine($"{p},{lambda},{err:e8}");
                    sw.Flush();
                }
            }

            {
                using StreamWriter sw = new("../../../../results_disused/quantile_cdf_precision102_evalerr_2.csv");

                sw.WriteLine("cdf,lambda_expected,lambda_predict,error");

                StreamReader sr = new("../../../../results_disused/quantile_cdf_precision103.csv");
                sr.ReadLine();

                while (!sr.EndOfStream) {
                    string? line = sr.ReadLine();

                    if (string.IsNullOrWhiteSpace(line)) {
                        break;
                    }

                    string[] item = line.Split(',');

                    MultiPrecision<N12> cdf = MultiPrecision<N12>.Ldexp(item[1], int.Parse(item[0].Split('^')[0]));
                    MultiPrecision<N12> lambda_expected = item[2];
                    MultiPrecision<N12> lambda_predict = Quantile.Value(cdf);

                    MultiPrecision<N12> err = MultiPrecision<N12>.Abs(lambda_expected - lambda_predict) / lambda_expected;

                    Console.WriteLine($"{cdf:e16},{lambda_predict:e16},{err:e8}");
                    sw.WriteLine($"{cdf},{lambda_expected},{lambda_predict},{err:e8}");
                    sw.Flush();
                }
            }

            {
                using StreamWriter sw = new("../../../../results_disused/quantile_ccdf_precision102_evalerr_2.csv");

                sw.WriteLine("ccdf,lambda_expected,lambda_predict,error");

                StreamReader sr = new("../../../../results_disused/quantile_ccdf_precision103_2.csv");
                sr.ReadLine();

                while (!sr.EndOfStream) {
                    string? line = sr.ReadLine();

                    if (string.IsNullOrWhiteSpace(line)) {
                        break;
                    }

                    string[] item = line.Split(',');

                    MultiPrecision<N12> ccdf = MultiPrecision<N12>.Ldexp(item[1], int.Parse(item[0].Split('^')[0]));

                    if (ccdf.IsZero) {
                        continue;
                    }

                    MultiPrecision<N12> lambda_expected = item[2];
                    MultiPrecision<N12> lambda_predict = Quantile.Value(ccdf, is_complementary: true);

                    MultiPrecision<N12> err = MultiPrecision<N12>.Abs(lambda_expected - lambda_predict) / lambda_expected;

                    Console.WriteLine($"{ccdf:e16},{lambda_predict:e16},{err:e8}");
                    sw.WriteLine($"{ccdf},{lambda_expected},{lambda_predict},{err:e8}");
                    sw.Flush();
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}