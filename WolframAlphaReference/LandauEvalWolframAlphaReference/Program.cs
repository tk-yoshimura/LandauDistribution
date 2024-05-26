using MultiPrecision;

namespace LandauEvalWolframAlphaReference {
    internal class Program {
        static void Main(string[] args) {
            MultiPrecision<Pow2.N32> c = 1 / MultiPrecision<Pow2.N32>.Sqrt(2 * MultiPrecision<Pow2.N32>.PI);

            using StreamReader sr = new("../../../../README.md");
            using StreamWriter sw = new("../../../../../results_disused/wolfram_negativeside_error.csv");

            sw.WriteLine("lambda,pdf_wolfram,pdf_approx,pdf_error,cdf_wolfram,cdf_approx,cdf_error");

            while (!sr.EndOfStream) {
                string? line = sr.ReadLine();

                if (line is null || line.StartsWith("|----|----|----|----|")) {
                    break;
                }
            }

            while (!sr.EndOfStream) {
                string? line = sr.ReadLine();

                if (line is null || !line.StartsWith('|')) {
                    break;
                }

                string[] line_split = line.Split('|', StringSplitOptions.RemoveEmptyEntries);

                if (line_split.Length < 4) {
                    continue;
                }

                MultiPrecision<Pow2.N32> lambda = line_split[0];

                if (lambda <= -8) {
                    continue;
                }

                if (lambda > -2) {
                    break;
                }

                MultiPrecision<Pow2.N32> pdf_wolfram = line_split[1];
                MultiPrecision<Pow2.N32> cdf_wolfram = line_split[2];

                MultiPrecision<Pow2.N32> sigma = MultiPrecision<Pow2.N32>.Exp(-lambda - 1);

                MultiPrecision<Pow2.N32> pdf_approx = c * MultiPrecision<Pow2.N32>.Sqrt(sigma) / MultiPrecision<Pow2.N32>.Exp(sigma);
                MultiPrecision<Pow2.N32> cdf_approx = c / (MultiPrecision<Pow2.N32>.Sqrt(sigma) * MultiPrecision<Pow2.N32>.Exp(sigma));

                MultiPrecision<Pow2.N32> pdf_error = pdf_wolfram / pdf_approx - 1;
                MultiPrecision<Pow2.N32> cdf_error = cdf_wolfram / cdf_approx - 1;

                sw.WriteLine($"{lambda},{pdf_wolfram:e20},{pdf_approx:e20},{pdf_error:e20},{cdf_wolfram:e20},{cdf_approx:e20},{cdf_error:e20}");
                Console.WriteLine($"{lambda}\n{pdf_wolfram:e20}\n{pdf_approx:e20}\n{pdf_error:e10}\n{cdf_wolfram:e20}\n{cdf_approx:e20}\n{cdf_error:e10}");
            }

            sw.Close();
 
            Console.WriteLine("END");
            Console.Read();
        }
    }
}
