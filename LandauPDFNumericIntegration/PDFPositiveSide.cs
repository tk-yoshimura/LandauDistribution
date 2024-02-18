using MultiPrecision;
using MultiPrecisionIntegrate;

namespace LandauPDFNumericIntegration {
    public static class PDFPositiveSide<N> where N : struct, IConstant {
        public static (MultiPrecision<N> value, MultiPrecision<N> error) ScaledValue(MultiPrecision<N> x, MultiPrecision<N> eps) {
            if (!(x >= 0)) {
                throw new ArgumentOutOfRangeException(nameof(x), "Must be non-negative.");
            }

            MultiPrecision<N> scale = x * x + MultiPrecision<N>.PI * MultiPrecision<N>.PI;

            eps /= scale;

            MultiPrecision<N> exp_mx = MultiPrecision<N>.Exp(-x);

            Func<MultiPrecision<N>, MultiPrecision<N>> f = (exp_mx.Exponent > -MultiPrecision<N>.Bits)
            ? (t) => {
                MultiPrecision<N> exp_xt = ExpCache<N>.Value(-x * t);

                if (MultiPrecision<N>.IsZero(exp_xt)) {
                    return MultiPrecision<N>.Zero;
                }

                return SinPICache<N>.Value(t) * exp_xt * (PowCache<N>.Value(t) - exp_mx * PowCache<N>.Value(t + 1));
            }
            : (t) => {
                MultiPrecision<N> exp_xt = ExpCache<N>.Value(-x * t);

                if (MultiPrecision<N>.IsZero(exp_xt)) {
                    return MultiPrecision<N>.Zero;
                }

                return SinPICache<N>.Value(t) * exp_xt * PowCache<N>.Value(t);
            };

            MultiPrecision<N> sum, error;

            double t_peak = FloorPow2PeakT((double)x);

            if (t_peak < 1d / 4) {
                MultiPrecision<N> h = t_peak * 2;

                (sum, error) = GaussKronrodIntegral<N>.AdaptiveIntegrate(
                    f, 0, h, eps, GaussKronrodOrder.G32K65, depth: 160
                );

                MultiPrecision<N> t = h;
                for (; t + h <= 1 && !MultiPrecision<N>.IsZero(ExpCache<N>.Value(-x * t)); t += h) {
                    (MultiPrecision<N> s, MultiPrecision<N> e)
                        = GaussKronrodIntegral<N>.AdaptiveIntegrate(
                            f, t, t + h, eps, GaussKronrodOrder.G32K65, depth: 160
                    );

                    sum += s;
                    error += e;

                    if (s < eps) {
                        t += h;
                        break;
                    }
                }
                if (t < 1 && !MultiPrecision<N>.IsZero(ExpCache<N>.Value(-x * t))) {
                    (MultiPrecision<N> s, MultiPrecision<N> e)
                        = GaussKronrodIntegral<N>.AdaptiveIntegrate(
                            f, t, 1, eps, GaussKronrodOrder.G32K65, depth: 160
                    );

                    sum += s;
                    error += e;
                }
            }
            else {
                (sum, error) = GaussKronrodIntegral<N>.AdaptiveIntegrate(
                    f, 0, 1, eps, GaussKronrodOrder.G32K65, depth: 160
                );
            }

            for (long t = 2; t < long.MaxValue - 1 && !MultiPrecision<N>.IsZero(ExpCache<N>.Value(-x * t)); t += 2) {
                (MultiPrecision<N> s, MultiPrecision<N> e)
                    = GaussKronrodIntegral<N>.AdaptiveIntegrate(
                        f, t, t + 1, eps, GaussKronrodOrder.G32K65, depth: 160
                );

                sum += s;
                error += e;

                if (s < eps) {
                    break;
                }
            }

            scale *= MultiPrecision<N>.RcpPI;

            sum *= scale;
            error *= scale;

            return (sum, error);
        }

        public static double PeakT(double x) {
            if (x < 1) {
                return double.NaN;
            }

            double f(double t) {
                return Math.PI / Math.Tan(Math.PI * t) - Math.Log(t) - 1 - x;
            };

            static double df(double t) {
                double s = Math.PI / Math.Sin(Math.PI * t);

                return -s * s - 1 / t;
            };

            double t = Math.Min(1 / x, 0.5);
            double dt = 1;
            int iter = 0;

            while (iter < 256 && t != t - dt / 2) {
                dt = f(t) / df(t);
                t -= dt;
                iter++;
            }

            return t;
        }

        public static double FloorPow2PeakT(double x) {
            double t = PeakT(x);

            if (double.IsNaN(t)) {
                return double.NaN;
            }

            for (int exp = 0; exp > -1000; exp--) {
                double v = Math.ScaleB(1, exp);

                if (t >= v) {
                    return v;
                }
            }

            return Math.ScaleB(1, -1000);
        }
    }
}
