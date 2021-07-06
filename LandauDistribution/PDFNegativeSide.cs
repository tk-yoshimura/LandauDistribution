using MultiPrecision;
using System;

namespace LandauDistribution {
    static class PDFNegativeSide<N> where N : struct, IConstant {
        static readonly MultiPrecision<N> c = MultiPrecision<N>.Log(2 * MultiPrecision<N>.RcpPI);
        static readonly MultiPrecision<N> r = 2 * MultiPrecision<N>.RcpPI * MultiPrecision<N>.RcpPI;

        public static (MultiPrecision<N> value, MultiPrecision<N> error, long accurate_bits) Value(MultiPrecision<N> x, int intergrate_iterations = 20) {
            if (!(x <= 0)) {
                throw new ArgumentOutOfRangeException(nameof(x), "Must be non-positive.");
            }

            const int needs_bits = 64;

            MultiPrecision<N> b = x + c;

            MultiPrecision<N> f(MultiPrecision<N> t) {
                MultiPrecision<N> phase = t.IsZero ? 0 : (2 * t * MultiPrecision<N>.RcpPI * (b + LogCache<N>.Value(t)));

                return ExpCache<N>.Value(-t) * SinCosCache<N>.CosValue(phase);
            }

            MultiPrecision<N> sum = 0, error = 0;

            for (long t = 0; t < long.MaxValue; t += 1) {
                (MultiPrecision<N> s, MultiPrecision<N> e) = MultiPrecisionUtil.RombergIntegrate<N>(f, t, t + 1, max_iterations: intergrate_iterations);

                sum += s;
                error += e;

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
