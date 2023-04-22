using MultiPrecision;

namespace Digits82 {
    class CDFAsymptotic {
        static void Main_() {
            using StreamReader sr = new("../../../../results_disused/asymptotic_pade_pdfintegrate_precision82.csv");
            using StreamWriter sw = new("../../../../results_disused/asymptotic_pdfintegrate_error.csv");

            sr.ReadLine();

            sw.WriteLine("lambda0,lambda1,expected,actual,error");

            while (!sr.EndOfStream) {
                string? line = sr.ReadLine();

                if (string.IsNullOrWhiteSpace(line)) {
                    break;
                }

                string[] line_split = line.Split(',');

                MultiPrecision<N12> x0 = line_split[0];
                MultiPrecision<N12> x1 = line_split[1];

                if (x1 >= -2 && x0 <= 10) {
                    continue;
                }

                MultiPrecision<N12> cdf_expected = line_split[2];

                MultiPrecision<N12> y0 = CDF.Value(x0, is_complementary: x0 > 0);
                MultiPrecision<N12> y1 = CDF.Value(x1, is_complementary: x1 > 0);

                MultiPrecision<N12> cdf_actual = (x1.Sign == Sign.Minus) ? (y1 - y0) : (y0 - y1);

                MultiPrecision<N12> err = MultiPrecision<N12>.Abs(cdf_actual - cdf_expected) / cdf_expected;

                Console.WriteLine($"{x0},{err:e8}");
                sw.WriteLine($"{x0},{x1},{cdf_expected},{cdf_actual},{err:e8}");
                sw.Flush();
            }
        }
    }
}