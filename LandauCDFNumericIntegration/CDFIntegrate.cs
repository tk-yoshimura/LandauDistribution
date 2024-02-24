using LandauPadeApprox;
using MultiPrecision;
using MultiPrecisionIntegrate;

namespace LandauCDFNumericIntegration {
    class CDFIntegrate {
        static void Main() {
            MultiPrecision<N20> sqrt_2pi_inv = 1 / MultiPrecision<N20>.Sqrt(2 * MultiPrecision<N20>.PI);

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

            using (StreamWriter sw = new("../../../../results_disused/cdfintegrate_precision152.csv")) {
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

                    (MultiPrecision<N20> y, MultiPrecision<N20> error, long eval_points) = (x0 > -2)
                        ? GaussKronrodIntegral<N20>.AdaptiveIntegrate(
                            lambda => PDFPadeN16.Value(lambda.Convert<Pow2.N16>()).Convert<N20>(),
                            x0.Convert<N20>(), x1.Convert<N20>(), eps.Convert<N20>() * 1e-154, GaussKronrodOrder.G256K513, 64
                        )
                        : BisectionAdaptiveIntegrate(
                            lambda => {
                                MultiPrecision<N20> sigma = MultiPrecision<N20>.Exp(-lambda - 1);

                                MultiPrecision<N20> scale = sqrt_2pi_inv * MultiPrecision<N20>.Sqrt(sigma) / MultiPrecision<N20>.Exp(sigma);

                                return PDFPadeN16.AMinus(lambda.Convert<Pow2.N16>()).Convert<N20>() * scale;
                            },
                            x0.Convert<N20>(), x1.Convert<N20>(), eps.Convert<N20>() * 1e-156, GaussKronrodOrder.G256K513, 64
                        );

                    MultiPrecision<Pow2.N16> relative_eps = error.Convert<Pow2.N16>() / eps;
                    MultiPrecision<Pow2.N16> relative_error = error.Convert<Pow2.N16>() / y.Convert<Pow2.N16>();

                    Console.WriteLine($"{y:e16},{eps:e8},{error:e8},{relative_eps:e8}");
                    sw.WriteLine($"{x0},{x1},{y},{relative_eps:e8},{error:e8},{relative_error:e8}");

                    sw.Flush();

                    x0 = x1;
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }

        static (MultiPrecision<N> y, MultiPrecision<N> error, long eval_points) BisectionAdaptiveIntegrate<N>(
            Func<MultiPrecision<N>, MultiPrecision<N>> f,
            MultiPrecision<N> a, MultiPrecision<N> b,
            MultiPrecision<N> eps,
            GaussKronrodOrder order = GaussKronrodOrder.G31K63, int maxdepth = -1) where N : struct, IConstant {

            if (a > b) {
                (MultiPrecision<N> y, MultiPrecision<N> error, long eval_points) =
                    BisectionAdaptiveIntegrate(f, b, a, eps, order, maxdepth);

                return (-y, error, eval_points);
            }
            else {
                MultiPrecision<N> fa = f(a), fb = f(b), h = b - a;

                while (h > 0) {
                    h /= 2;

                    MultiPrecision<N> c = a + h, fc = f(c);

                    if ((fa + fc) * h / 2 < eps) {
                        a = c;
                        fa = fc;
                        continue;
                    }
                    if ((fc + fb) * h / 2 < eps) {
                        b = c;
                        fb = fc;
                        continue;
                    }

                    break;
                }

                return GaussKronrodIntegral<N>.AdaptiveIntegrate(f, a, b, eps, order, maxdepth);
            }
        }
    }
}