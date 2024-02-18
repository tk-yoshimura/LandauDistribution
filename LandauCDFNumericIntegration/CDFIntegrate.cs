using LandauPadeApprox;
using MultiPrecision;
using MultiPrecisionIntegrate;

namespace LandauCDFMinusAsymptotic {
    class CDFIntegrate {
        static void Main() {
            List<MultiPrecision<N12>> xs = [];

            {
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
            }

            xs.Sort();

            {
                using StreamWriter sw = new("../../../../results_disused/asymptotic_pade_pdfintegrate_precision103.csv");
                sw.WriteLine("lambda0,lambda1,integrate,error,error/eps");

                MultiPrecision<N12> x0 = xs[0];

                foreach (MultiPrecision<N12> x1 in xs.Skip(1)) {
                    MultiPrecision<N12> eps;

                    if ((x0 >= -2 && x0 <= 2) & (x1 >= -2 && x1 <= 2)) {
                        eps = (PDFPadeN12.Value(x0) + PDFPadeN12.Value(x1)) * (x1 - x0) * 1e-104;
                    }
                    else if (x1.Sign == Sign.Plus) {
                        eps = (1 / x0 - 1 / x1) * 1e-103;
                    }
                    else {
                        MultiPrecision<N12> sigma0 = MultiPrecision<N12>.Exp(-x0 - 1);
                        MultiPrecision<N12> sigma1 = MultiPrecision<N12>.Exp(-x1 - 1);

                        MultiPrecision<N12> y0 = MultiPrecision<N12>.Sqrt(sigma0) / MultiPrecision<N12>.Exp(sigma0);
                        MultiPrecision<N12> y1 = MultiPrecision<N12>.Sqrt(sigma1) / MultiPrecision<N12>.Exp(sigma1);

                        eps = (y1 - y0) * 1e-103;
                    }

                    (MultiPrecision<N12> y, MultiPrecision<N12> error) = GaussKronrodIntegral<N12>.AdaptiveIntegrate(PDFPadeN12.Value, x0, x1, eps, GaussKronrodOrder.G32K65, 128);

                    MultiPrecision<N12> error_eps = error / eps;

                    Console.WriteLine($"{x0},{x1},{y:e16},{eps:e8},{error:e8},{error_eps:e8}");
                    sw.WriteLine($"{x0},{x1},{y},{error:e8},{error_eps:e8}");
                    sw.Flush();

                    x0 = x1;
                }
            }
        }
    }
}