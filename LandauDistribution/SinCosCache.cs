using MultiPrecision;

namespace LandauDistribution {
    static class SinCosCache<N> where N : struct, IConstant {
        const int d = 18, n = 1 << d;
        static readonly MultiPrecision<N>[] sin_table, cos_table;

        static SinCosCache() {
            sin_table = new MultiPrecision<N>[n + 1];
            cos_table = new MultiPrecision<N>[n + 1];

            for (long i = 0, j = n; i <= n; i++, j--) {
                MultiPrecision<N> x = MultiPrecision<N>.Ldexp(i, -d - 1);
                MultiPrecision<N> s = MultiPrecision<N>.SinPI(x);

                sin_table[i] = cos_table[j] = s;
            }
        }

        public static MultiPrecision<N> SinPIValue(MultiPrecision<N> t) {
            if (t.Sign == Sign.Minus) {
                return -SinPIValue(-t);
            }

            if (t >= 2) {
                return SinPIValue(t % 2);
            }

            if (t > 1) {
                return -SinPIValue(2 - t);
            }

            if (t > MultiPrecision<N>.Point5) {
                return SinPIValue(1 - t);
            }

            MultiPrecision<N> x = MultiPrecision<N>.Ldexp(t, d + 1);

            long i = (long)MultiPrecision<N>.Floor(x);

            MultiPrecision<N> u = MultiPrecision<N>.Ldexp(x - i, -d - 1);

            if (u.IsZero) {
                return sin_table[i];
            }

            MultiPrecision<N> ns1 = sin_table[i], ns2 = MultiPrecision<N>.SinPI(u);
            MultiPrecision<N> nc1 = cos_table[i], nc2 = MultiPrecision<N>.CosPI(u);

            MultiPrecision<N> y = ns1 * nc2 + nc1 * ns2;

            return y;
        }

        public static MultiPrecision<N> CosPIValue(MultiPrecision<N> t) {
            if (t.Sign == Sign.Minus) {
                return CosPIValue(-t);
            }

            if (t >= 2) {
                return CosPIValue(t % 2);
            }

            if (t > 1) {
                return CosPIValue(2 - t);
            }

            if (t > MultiPrecision<N>.Point5) {
                return -CosPIValue(1 - t);
            }

            MultiPrecision<N> x = MultiPrecision<N>.Ldexp(t, d + 1);

            long i = (long)MultiPrecision<N>.Floor(x);

            MultiPrecision<N> u = MultiPrecision<N>.Ldexp(x - i, -d - 1);

            if (u.IsZero) {
                return cos_table[i];
            }

            MultiPrecision<N> ns1 = sin_table[i], ns2 = MultiPrecision<N>.SinPI(u);
            MultiPrecision<N> nc1 = cos_table[i], nc2 = MultiPrecision<N>.CosPI(u);

            MultiPrecision<N> y = nc1 * nc2 - ns1 * ns2;

            return y;
        }

        public static MultiPrecision<N> SinValue(MultiPrecision<N> t) {
            return SinPIValue(MultiPrecision<N>.RcpPI * t);
        }

        public static MultiPrecision<N> CosValue(MultiPrecision<N> t) {
            return CosPIValue(MultiPrecision<N>.RcpPI * t);
        }
    }
}
