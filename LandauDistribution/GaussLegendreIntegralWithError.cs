using MultiPrecision;
using MultiPrecisionIntegrate;

namespace LandauDistribution {
    static class GaussLegendreIntegralWithError<N> where N : struct, IConstant {
        public static MultiPrecision<N> Integrate(Func<MultiPrecision<N>, MultiPrecision<N>> f, MultiPrecision<N> a, MultiPrecision<N> b, int n, int div_iters) {
            if (div_iters < 0) {
                throw new ArgumentOutOfRangeException(nameof(div_iters));
            }

            int m = checked(1 << div_iters);
            
            MultiPrecision<N> h = MultiPrecision<N>.Ldexp(b - a, -div_iters);

            MultiPrecision<N> s = 0;

            for (int i = 0; i < m; i++) {
                s += GaussLegendreIntegral<N>.Integrate(f, a + i * h, a + (i + 1) * h, n);
            }

            return s;
        }

        public static (MultiPrecision<N> s, MultiPrecision<N> e) Integrate(Func<MultiPrecision<N>, MultiPrecision<N>> f, MultiPrecision<N> a, MultiPrecision<N> b, int n) {
            MultiPrecision<N> s0 = Integrate(f, a, b, n, div_iters: 0);
            MultiPrecision<N> s1 = Integrate(f, a, b, n + 1, div_iters: 0);

            return (s1, MultiPrecision<N>.Abs(s0 - s1));
        }
    }
}
