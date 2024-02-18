using MultiPrecision;

namespace LandauBasicFunctionCache {
    public static class SinCosCache<N> where N : struct, IConstant {
        static readonly SinCosPICacheTableL0<N> l0_cache = new();
        static readonly SinCosPICacheTableL1<N> l1_cache = new();
        static readonly SinCosPICacheTableL2<N> l2_cache = new();
        static readonly SinCosPICacheTableL3<N> l3_cache = new();

        public static MultiPrecision<N> Sin(MultiPrecision<N> x) {
            return SinPI(x * MultiPrecision<N>.RcpPI);
        }

        public static MultiPrecision<N> Cos(MultiPrecision<N> x) {
            return CosPI(x * MultiPrecision<N>.RcpPI);
        }

        public static MultiPrecision<N> SinPI(MultiPrecision<N> x) {
            if (MultiPrecision<N>.IsInfinity(x) || MultiPrecision<N>.IsNaN(x)) {
                return MultiPrecision<N>.NaN;
            }

            if (MultiPrecision<N>.IsNegative(x)) {
                return -SinPI(-x);
            }

            if (x >= 2) {
                x -= 2 * MultiPrecision<N>.Floor(MultiPrecision<N>.Ldexp(x, -1));
            }

            if (x > 1) {
                return -SinPI(2 - x);
            }

            if (x > MultiPrecision<N>.Point5) {
                return SinPI(1 - x);
            }

            ((MultiPrecision<N> sin_l0, MultiPrecision<N> cos_l0), _, _, MultiPrecision<N> x_l0) = l0_cache.Value(x);
            ((MultiPrecision<N> sin_l1, MultiPrecision<N> cos_l1), _, _, MultiPrecision<N> x_l1) = l1_cache.Value(x_l0);
            ((MultiPrecision<N> sin_l2, MultiPrecision<N> cos_l2), _, _, MultiPrecision<N> x_l2) = l2_cache.Value(x_l1);
            ((MultiPrecision<N> sin_l3, MultiPrecision<N> cos_l3), _, _, MultiPrecision<N> x_l3) = l3_cache.Value(x_l2);

            MultiPrecision<N> sin_l4 = MultiPrecision<N>.SinPI(x_l3);
            MultiPrecision<N> cos_l4 = MultiPrecision<N>.Sqrt(1 - sin_l4 * sin_l4);

            MultiPrecision<N> sin_xc, cos_xc;
            (sin_xc, cos_xc) = (sin_l0 * cos_l1 + cos_l0 * sin_l1, cos_l0 * cos_l1 - sin_l0 * sin_l1);
            (sin_xc, cos_xc) = (sin_xc * cos_l2 + cos_xc * sin_l2, cos_xc * cos_l2 - sin_xc * sin_l2);
            (sin_xc, cos_xc) = (sin_xc * cos_l3 + cos_xc * sin_l3, cos_xc * cos_l3 - sin_xc * sin_l3);
            sin_xc = sin_xc * cos_l4 + cos_xc * sin_l4;

            return sin_xc;
        }

        public static MultiPrecision<N> CosPI(MultiPrecision<N> x) {
            if (MultiPrecision<N>.IsInfinity(x) || MultiPrecision<N>.IsNaN(x)) {
                return MultiPrecision<N>.NaN;
            }

            if (MultiPrecision<N>.IsNegative(x)) {
                return CosPI(-x);
            }

            if (x >= 2) {
                x -= 2 * MultiPrecision<N>.Floor(MultiPrecision<N>.Ldexp(x, -1));
            }

            if (x > 1) {
                return CosPI(2 - x);
            }

            if (x > MultiPrecision<N>.Point5) {
                return -CosPI(1 - x);
            }

            ((MultiPrecision<N> sin_l0, MultiPrecision<N> cos_l0), _, _, MultiPrecision<N> x_l0) = l0_cache.Value(x);
            ((MultiPrecision<N> sin_l1, MultiPrecision<N> cos_l1), _, _, MultiPrecision<N> x_l1) = l1_cache.Value(x_l0);
            ((MultiPrecision<N> sin_l2, MultiPrecision<N> cos_l2), _, _, MultiPrecision<N> x_l2) = l2_cache.Value(x_l1);
            ((MultiPrecision<N> sin_l3, MultiPrecision<N> cos_l3), _, _, MultiPrecision<N> x_l3) = l3_cache.Value(x_l2);

            MultiPrecision<N> sin_l4 = MultiPrecision<N>.SinPI(x_l3);
            MultiPrecision<N> cos_l4 = MultiPrecision<N>.Sqrt(1 - sin_l4 * sin_l4);

            MultiPrecision<N> sin_xc, cos_xc;
            (sin_xc, cos_xc) = (sin_l0 * cos_l1 + cos_l0 * sin_l1, cos_l0 * cos_l1 - sin_l0 * sin_l1);
            (sin_xc, cos_xc) = (sin_xc * cos_l2 + cos_xc * sin_l2, cos_xc * cos_l2 - sin_xc * sin_l2);
            (sin_xc, cos_xc) = (sin_xc * cos_l3 + cos_xc * sin_l3, cos_xc * cos_l3 - sin_xc * sin_l3);
            cos_xc = cos_xc * cos_l4 - sin_xc * sin_l4;

            return cos_xc;
        }
    }

    internal class SinCosPICacheTableL0<N> : CacheTable<N, (MultiPrecision<N> sin, MultiPrecision<N> cos)> where N : struct, IConstant {
        public SinCosPICacheTableL0() : base(xmax_exponent: -1, n: 1024) { }

        public override (MultiPrecision<N> sin, MultiPrecision<N> cos) Compute(MultiPrecision<N> x) {
            return (MultiPrecision<N>.SinPI(x), MultiPrecision<N>.CosPI(x));
        }
    }

    internal class SinCosPICacheTableL1<N> : CacheTable<N, (MultiPrecision<N> sin, MultiPrecision<N> cos)> where N : struct, IConstant {

        public SinCosPICacheTableL1() : base(xmax_exponent: -11, n: 1024) { }

        public override (MultiPrecision<N> sin, MultiPrecision<N> cos) Compute(MultiPrecision<N> x) {
            MultiPrecision<N> sin = MultiPrecision<N>.SinPI(x);
            MultiPrecision<N> cos = MultiPrecision<N>.Sqrt(1 - sin * sin);

            return (sin, cos);
        }
    }

    internal class SinCosPICacheTableL2<N> : CacheTable<N, (MultiPrecision<N> sin, MultiPrecision<N> cos)> where N : struct, IConstant {
        public SinCosPICacheTableL2() : base(xmax_exponent: -21, n: 1024) { }

        public override (MultiPrecision<N> sin, MultiPrecision<N> cos) Compute(MultiPrecision<N> x) {
            MultiPrecision<N> sin = MultiPrecision<N>.SinPI(x);
            MultiPrecision<N> cos = MultiPrecision<N>.Sqrt(1 - sin * sin);

            return (sin, cos);
        }
    }

    internal class SinCosPICacheTableL3<N> : CacheTable<N, (MultiPrecision<N> sin, MultiPrecision<N> cos)> where N : struct, IConstant {
        public SinCosPICacheTableL3() : base(xmax_exponent: -31, n: 1024) { }

        public override (MultiPrecision<N> sin, MultiPrecision<N> cos) Compute(MultiPrecision<N> x) {
            MultiPrecision<N> sin = MultiPrecision<N>.SinPI(x);
            MultiPrecision<N> cos = MultiPrecision<N>.Sqrt(1 - sin * sin);

            return (sin, cos);
        }
    }
}
