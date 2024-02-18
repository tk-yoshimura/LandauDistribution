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
                using StreamWriter sw = new("../../../../results_disused/asymptotic_pade_pdf_precision103.csv");
                sw.WriteLine("lambda,pdf");

                foreach (MultiPrecision<N12> x in xs) {
                    MultiPrecision<N12> y = PDFPadeN12.Value(x);

                    Console.WriteLine($"{x},{y:e16}");
                    sw.WriteLine($"{x},{y:e103}");
                }

                sw.Flush();
            }

            {
                using StreamReader sr = new("../../../../results_disused/integrate_scaled_pdf_precision100.csv");

                for (int i = 0; i < 3; i++) {
                    sr.ReadLine();
                }

                using StreamWriter sw = new("../../../../results_disused/asymptotic_pade_scaledpdf_precision103.csv");
                sw.WriteLine("# scale_pdf := (lambda >= 0) ? ( pdf * (lambda^2 + pi^2) ) : ( pdf * sqrt(2 pi) * exp(sigma) / sqrt(sigma) )");
                sw.WriteLine("# sigma := exp(-lambda-1)");

                sw.WriteLine("lambda,scaled_pdf,error");

                while (!sr.EndOfStream) {
                    string? line = sr.ReadLine();

                    if (string.IsNullOrWhiteSpace(line)) {
                        break;
                    }

                    string[] line_split = line.Split(',');

                    MultiPrecision<N12> x = line_split[0], expected = line_split[1];
                    MultiPrecision<N12> y = (x.Sign == Sign.Plus) ? PDFPadeN12.APlus(x) : PDFPadeN12.AMinus(x);
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