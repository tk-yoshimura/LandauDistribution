using MultiPrecision;

namespace LandauDistribution {
    static class SinCosCache<N> where N : struct, IConstant {
        const long n = 1 << 18;
        static readonly MultiPrecision<N>[] sin_table1, cos_table1, sin_table2, cos_table2;

        static SinCosCache() {
            sin_table1 = new MultiPrecision<N>[n + 1];
            cos_table1 = new MultiPrecision<N>[n + 1];
            sin_table2 = new MultiPrecision<N>[n + 1];
            cos_table2 = new MultiPrecision<N>[n + 1];

            for (long i = 0, j = n; i <= n; i++, j--) {
                MultiPrecision<N> x = (MultiPrecision<N>)i / (n * 2);
                MultiPrecision<N> s = MultiPrecision<N>.SinPI(x);

                sin_table1[i] = cos_table1[j] = s;
            }

            for (long i = 0; i <= n; i++) {
                MultiPrecision<N> x = (MultiPrecision<N>)i / (n * n * 2);
                MultiPrecision<N> s = MultiPrecision<N>.SinPI(x);
                MultiPrecision<N> c = MultiPrecision<N>.CosPI(x);

                sin_table2[i] = s;
                cos_table2[i] = c;
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

            MultiPrecision<N> x = t * (n * n * 2);

            long i = (long)MultiPrecision<N>.Floor(x);
            long i1 = i / n, i2 = i % n;

            MultiPrecision<N> u = x - i, v = 1 - u;

            MultiPrecision<N> ns1 = sin_table1[i1], ns2 = sin_table2[i2] * v + sin_table2[i2 + 1] * u;
            MultiPrecision<N> nc1 = cos_table1[i1], nc2 = cos_table2[i2] * v + cos_table2[i2 + 1] * u;

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

            MultiPrecision<N> x = t * (n * n * 2);

            long i = (long)MultiPrecision<N>.Floor(x);
            long i1 = i / n, i2 = i % n;

            MultiPrecision<N> u = x - i, v = 1 - u;

            MultiPrecision<N> ns1 = sin_table1[i1], ns2 = sin_table2[i2] * v + sin_table2[i2 + 1] * u;
            MultiPrecision<N> nc1 = cos_table1[i1], nc2 = cos_table2[i2] * v + cos_table2[i2 + 1] * u;

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
