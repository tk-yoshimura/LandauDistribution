using LandauPadeApprox;
using MultiPrecision;
using MultiPrecisionIntegrate;

namespace LandauCDFMinusAsymptotic {
    class CDFIntegrate {
        static void Main_() {
            List<MultiPrecision<Pow2.N16>> xs = [];

            {
                for (MultiPrecision<Pow2.N16> x = 0; x < 1; x += 1d / 4096) {
                    xs.Add(x);
                }
                for (MultiPrecision<Pow2.N16> h = 1d / 2048, x0 = 1, x1 = 2; x1 <= 4096; h *= 2, x0 *= 2, x1 *= 2) {
                    for (MultiPrecision<Pow2.N16> x = x0; x < x1; x += h) {
                        xs.Add(x);
                    }
                }
                xs.Add(4096);

                for (MultiPrecision<Pow2.N16> x = 0; x < 1; x += 1d / 4096) {
                    xs.Add(-x);
                }
                for (MultiPrecision<Pow2.N16> h = 1d / 2048, x0 = 1, x1 = 2; x1 <= 16; h *= 2, x0 *= 2, x1 *= 2) {
                    for (MultiPrecision<Pow2.N16> x = x0; x < x1; x += h) {
                        xs.Add(-x);
                    }
                }
                xs.Add(-16);
            }

            xs.Sort();

            {
                using StreamWriter sw = new("../../../../results_disused/cdfintegrate_precision152.csv");
                sw.WriteLine("lambda0,lambda1,integrate,error/eps,error,relative_error");

                MultiPrecision<Pow2.N16> x0 = xs[0];

                foreach (MultiPrecision<Pow2.N16> x1 in xs.Skip(1)) {
                    Console.WriteLine($"{x0},{x1}");

                    MultiPrecision<Pow2.N16> eps;

                    if ((x0 >= -2 && x0 <= 2) & (x1 >= -2 && x1 <= 2)) {
                        eps = (PDFPadeN16.Value(x0) + PDFPadeN16.Value(x1)) * (x1 - x0);
                    }
                    else if (x1.Sign == Sign.Plus) {
                        eps = 1 / x0 - 1 / x1;
                    }
                    else {
                        MultiPrecision<Pow2.N16> sigma0 = MultiPrecision<Pow2.N16>.Exp(-x0 - 1);
                        MultiPrecision<Pow2.N16> sigma1 = MultiPrecision<Pow2.N16>.Exp(-x1 - 1);

                        MultiPrecision<Pow2.N16> y0 = MultiPrecision<Pow2.N16>.Sqrt(sigma0) / MultiPrecision<Pow2.N16>.Exp(sigma0);
                        MultiPrecision<Pow2.N16> y1 = MultiPrecision<Pow2.N16>.Sqrt(sigma1) / MultiPrecision<Pow2.N16>.Exp(sigma1);

                        eps = y1 - y0;
                    }
                    (MultiPrecision<Pow2.N16> y, MultiPrecision<Pow2.N16> error, long eval_points) = 
                        GaussKronrodIntegral<Pow2.N16>.AdaptiveIntegrate(
                            PDFPadeN16.Value, x0, x1, eps * 1e-152, GaussKronrodOrder.G128K257, 64
                        );

                    MultiPrecision<Pow2.N16> relative_eps = error / eps;
                    MultiPrecision<Pow2.N16> relative_error = error / y;

                    Console.WriteLine($"{y:e16},{eps:e8},{error:e8},{relative_eps:e8}");
                    sw.WriteLine($"{x0},{x1},{y},{relative_eps:e8},{error:e8},{relative_error:e8}");
                    sw.Flush();

                    x0 = x1;
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}