using MultiPrecision;

namespace LandauBasicFunctionCache {
    public static class ExpCache<N> where N : struct, IConstant {
        static readonly Pow2CacheTableL0<N> l0_cache = new();
        static readonly Pow2CacheTableL1<N> l1_cache = new();
        static readonly Pow2CacheTableL2<N> l2_cache = new();
        static readonly Pow2CacheTableL3<N> l3_cache = new();

        public static MultiPrecision<N> Exp(MultiPrecision<N> x) {
            return Pow2(x * MultiPrecision<N>.LbE);
        }

        public static MultiPrecision<N> Pow2(MultiPrecision<N> x) {
            if (MultiPrecision<N>.IsNaN(x)) {
                return MultiPrecision<N>.NaN;
            }
            if (MultiPrecision<N>.IsInfinity(x)) {
                return x < 0 ? 0 : MultiPrecision<N>.PositiveInfinity;
            }

            long exponent = (long)MultiPrecision<N>.Floor(x);
            MultiPrecision<N> f = x - exponent;

            (MultiPrecision<N> pow2_l0, _, _, MultiPrecision<N> f_l0) = l0_cache.Value(f);
            (MultiPrecision<N> pow2_l1, _, _, MultiPrecision<N> f_l1) = l1_cache.Value(f_l0);
            (MultiPrecision<N> pow2_l2, _, _, MultiPrecision<N> f_l2) = l2_cache.Value(f_l1);
            (MultiPrecision<N> pow2_l3, _, _, MultiPrecision<N> f_l3) = l3_cache.Value(f_l2);

            MultiPrecision<N> pow2_l4 = MultiPrecision<N>.Pow2(f_l3);

            MultiPrecision<N> pow2 = MultiPrecision<N>.Ldexp(pow2_l0 * pow2_l1 * pow2_l2 * pow2_l3 * pow2_l4, exponent);

            return pow2;
        }
    }

    internal class Pow2CacheTableL0<N> : CacheTable<N, MultiPrecision<N>> where N : struct, IConstant {
        public Pow2CacheTableL0() : base(xmax_exponent: 0, n: 1024) { }

        public override MultiPrecision<N> Compute(MultiPrecision<N> x) {
            return MultiPrecision<N>.Pow2(x);
        }
    }

    internal class Pow2CacheTableL1<N> : CacheTable<N, MultiPrecision<N>> where N : struct, IConstant {
        public Pow2CacheTableL1() : base(xmax_exponent: -10, n: 1024) { }

        public override MultiPrecision<N> Compute(MultiPrecision<N> x) {
            return MultiPrecision<N>.Pow2(x);
        }
    }

    internal class Pow2CacheTableL2<N> : CacheTable<N, MultiPrecision<N>> where N : struct, IConstant {
        public Pow2CacheTableL2() : base(xmax_exponent: -20, n: 1024) { }

        public override MultiPrecision<N> Compute(MultiPrecision<N> x) {
            return MultiPrecision<N>.Pow2(x);
        }
    }

    internal class Pow2CacheTableL3<N> : CacheTable<N, MultiPrecision<N>> where N : struct, IConstant {
        public Pow2CacheTableL3() : base(xmax_exponent: -30, n: 1024) { }

        public override MultiPrecision<N> Compute(MultiPrecision<N> x) {
            return MultiPrecision<N>.Pow2(x);
        }
    }
}
