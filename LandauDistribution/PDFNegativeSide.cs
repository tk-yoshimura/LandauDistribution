using MultiPrecision;

namespace LandauDistribution {
    public static class PDFNegativeSide<N> where N : struct, IConstant {
        static readonly MultiPrecision<N> c = MultiPrecision<N>.Log(2 * MultiPrecision<N>.RcpPI);
        static readonly MultiPrecision<N> r = 2 * MultiPrecision<N>.RcpPI * MultiPrecision<N>.RcpPI;

        public static (MultiPrecision<N> value, MultiPrecision<N> error, long accurate_bits) Value(MultiPrecision<N> x, int points = 64, int needs_bits = 212) {
            if (!(x <= 0)) {
                throw new ArgumentOutOfRangeException(nameof(x), "Must be non-positive.");
            }
            if (MultiPrecision<N>.Bits < needs_bits + 16) {
                throw new ArithmeticException("Lack of precision.");
            }

            MultiPrecision<N> b = x + c;

            MultiPrecision<N> f(MultiPrecision<N> t) {
                MultiPrecision<N> phase = t.IsZero ? 0 : (2 * t * MultiPrecision<N>.RcpPI * (b + MultiPrecision<N>.Log(t)));

                return MultiPrecision<N>.Exp(-t) * MultiPrecision<N>.Cos(phase);
            }

            MultiPrecision<N> sum = 0, error = 0, eps = MultiPrecision<N>.Ldexp(1, -MultiPrecision<N>.Bits);
            MultiPrecision<N> t_prev = 0;

            foreach ((MultiPrecision<N> t, _) in LandauNegativeSideRoot.LandauNegativeSideRootMP<N>.EnumHalfPIValues(x, 65536).Skip(1)) {
                (MultiPrecision<N> s, MultiPrecision<N> e) = GaussLegendreIntegralWithError<N>.Integrate(f, t_prev, t, points);

                sum += s;
                error += e;
                t_prev = t;

                if (sum.Exponent - s.Exponent < needs_bits + 16) {
                    continue;
                }

                break;
            }

            long accurate_bits = sum.Exponent - error.Exponent;

            return (sum * r, error * r, accurate_bits);
        }
    }
}
