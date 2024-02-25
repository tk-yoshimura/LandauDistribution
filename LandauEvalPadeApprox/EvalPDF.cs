using LandauPadeApprox;
using MultiPrecision;

namespace LandauEvalPadeApprox {
    class EvalPDF {
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
                using StreamWriter sw = new("../../../../results_disused/pade_pdf_precision154.csv");
                sw.WriteLine("lambda,pdf");

                foreach (MultiPrecision<Pow2.N16> x in xs) {
                    MultiPrecision<Pow2.N16> y = PDFPadeN16.Value(x);

                    Console.WriteLine($"{x},{y:e16}");
                    sw.WriteLine($"{x},{y}");
                }

                sw.Flush();
            }

            {
                using StreamReader sr = new("../../../../results_disused/scaled_pdf_precision165.csv");

                for (int i = 0; i < 3; i++) {
                    sr.ReadLine();
                }

                using StreamWriter sw = new("../../../../results_disused/evalpade_scaledpdf_precision150.csv");
                sw.WriteLine("# scale_pdf := (lambda >= 0) ? ( pdf * (lambda^2 + pi^2) ) : ( pdf * sqrt(2 pi) * exp(sigma) / sqrt(sigma) )");
                sw.WriteLine("# sigma := exp(-lambda-1)");

                sw.WriteLine("lambda,scaled_pdf,error");

                while (!sr.EndOfStream) {
                    string? line = sr.ReadLine();

                    if (string.IsNullOrWhiteSpace(line)) {
                        break;
                    }

                    string[] line_split = line.Split(',');

                    MultiPrecision<Pow2.N16> x = line_split[0];
                    MultiPrecision<Pow2.N32> expected = line_split[1];
                    MultiPrecision<Pow2.N32> y = ((x.Sign == Sign.Plus) ? PDFPadeN16.APlus(x) : PDFPadeN16.AMinus(x)).Convert<Pow2.N32>();
                    MultiPrecision<Pow2.N32> err = MultiPrecision<Pow2.N32>.Abs(y - expected) / expected;

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