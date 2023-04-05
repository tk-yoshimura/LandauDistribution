using MultiPrecision;
using MultiPrecisionIntegrate;

namespace NumericIntegration {
    public static class PDFPositiveSide<N> where N : struct, IConstant {
        public static (MultiPrecision<N> value, MultiPrecision<N> error) Value(MultiPrecision<N> x, MultiPrecision<N> eps) {
            if (!(x >= 0)) {
                throw new ArgumentOutOfRangeException(nameof(x), "Must be non-negative.");
            }

            MultiPrecision<N> exp_mx = MultiPrecision<N>.Exp(-x);

            Func<MultiPrecision<N>, MultiPrecision<N>> f = (exp_mx.Exponent > -MultiPrecision<N>.Bits)
            ? (t) => {
                MultiPrecision<N> exp_xt = ExpCache<N>.Value(-x * t);

                if (exp_xt.IsZero) {
                    return MultiPrecision<N>.Zero;
                }

                return SinPICache<N>.Value(t) * exp_xt * (PowCache<N>.Value(t) - exp_mx * PowCache<N>.Value(t + 1));
            }
            : (t) => {
                MultiPrecision<N> exp_xt = ExpCache<N>.Value(-x * t);

                if (exp_xt.IsZero) {
                    return MultiPrecision<N>.Zero;
                }

                return SinPICache<N>.Value(t) * exp_xt * PowCache<N>.Value(t);
            };

            MultiPrecision<N> sum, error;

            (sum, error) = GaussKronrodIntegral<N>.AdaptiveIntegrate(
                f, 0, 1, eps, GaussKronrodOrder.G15K31, depth: 12
            );

            for (long t = 2; t < long.MaxValue - 1 && !ExpCache<N>.Value(-x * t).IsZero; t += 2) {
                (MultiPrecision<N> s, MultiPrecision<N> e)
                    = GaussKronrodIntegral<N>.AdaptiveIntegrate(
                        f, t, t + 1, eps, GaussKronrodOrder.G15K31, depth: 12
                );

                sum += s;
                error += e;

                if (sum < eps) {
                    continue;
                }

                break;
            }

            long accurate_bits = sum.Exponent - error.Exponent;

            return (sum / MultiPrecision<N>.PI, error / MultiPrecision<N>.PI);
        }
    }
}
