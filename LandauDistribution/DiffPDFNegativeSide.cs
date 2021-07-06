using MultiPrecision;
using System;

namespace LandauDistribution {
    static class DiffPDFNegativeSide<N> where N : struct, IConstant {
        static readonly MultiPrecision<N> c = MultiPrecision<N>.Log(2 * MultiPrecision<N>.RcpPI);
        static readonly MultiPrecision<N> r = 4 * MultiPrecision<N>.RcpPI * MultiPrecision<N>.RcpPI * MultiPrecision<N>.RcpPI;

        public static (MultiPrecision<N> value, MultiPrecision<N> error, long accurate_bits) Value(MultiPrecision<N> x, int intergrate_iterations = 20) {
            if (!(x <= 0)) {
                throw new ArgumentOutOfRangeException(nameof(x), "Must be non-positive.");
            }

            const int needs_bits = 64;

            MultiPrecision<N> b = x + c;

            MultiPrecision<N> f(MultiPrecision<N> t) {
                MultiPrecision<N> phase = t.IsZero ? 0 : (2 * t * MultiPrecision<N>.RcpPI * (b + LogCache<N>.Value(t)));

                return t * ExpCache<N>.Value(-t) * SinCosCache<N>.SinValue(phase);
            }

            MultiPrecision<N> sum = 0, error = 0, eps = null;

            for (long t = 0; t < long.MaxValue; t += 1) {
                (MultiPrecision<N> s, MultiPrecision<N> e) = MultiPrecisionUtil.RombergIntegrate(f, t, t + 1, max_iterations: intergrate_iterations, epsilon: eps);

                sum += s;
                error += e;

                if (eps is null) {
                    eps = MultiPrecision<N>.Ldexp(sum, -needs_bits - 2);
                }

                if (sum.Exponent - s.Exponent < needs_bits + 8) {
                    continue;
                }

                break;
            }

            long accurate_bits = sum.Exponent - error.Exponent;

            return (-sum * r, error * r, accurate_bits);
        }
    }
}
