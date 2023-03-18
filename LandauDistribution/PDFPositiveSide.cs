using MultiPrecision;

namespace LandauDistribution {
    public static class PDFPositiveSide<N> where N : struct, IConstant {
        public static (MultiPrecision<N> value, MultiPrecision<N> error, long accurate_bits) Value(MultiPrecision<N> x, int points = 64, int needs_bits = 212) {
            if (!(x >= 0)) {
                throw new ArgumentOutOfRangeException(nameof(x), "Must be non-negative.");
            }
            if (MultiPrecision<N>.Bits < needs_bits + 16) {
                throw new ArithmeticException("Lack of precision.");
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

            MultiPrecision<N> sum, error, eps;

            (sum, error) = GaussLegendreIntegralWithError<N>.Integrate(f, 0, 1, points);

            eps = MultiPrecision<N>.Ldexp(sum, -needs_bits - 2);

            for (long t = 2; t < long.MaxValue - 1 && !ExpCache<N>.Value(-x * t).IsZero; t += 2) {
                (MultiPrecision<N> s, MultiPrecision<N> e) = GaussLegendreIntegralWithError<N>.Integrate(f, t, t + 1, points);

                sum += s;
                error += e;

                if (sum.Exponent - s.Exponent < needs_bits + 8) {
                    continue;
                }

                break;
            }

            long accurate_bits = sum.Exponent - error.Exponent;

            return (sum / MultiPrecision<N>.PI, error / MultiPrecision<N>.PI, accurate_bits);
        }
    }
}
