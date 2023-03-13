using MultiPrecision;

namespace LandauDistribution {
    static class LogCache<N> where N : struct, IConstant {
        static readonly Dictionary<MultiPrecision<N>, MultiPrecision<N>> f_table = new();
        static readonly Dictionary<long, MultiPrecision<N>> n_table = new();

        public static MultiPrecision<N> Value(MultiPrecision<N> t) {
            long n = t.Exponent;
            MultiPrecision<N> f = MultiPrecision<N>.Ldexp(t, -n);

            MultiPrecision<N> yf, yn;
            if (!f_table.ContainsKey(f)) {
                yf = MultiPrecision<N>.Log(f);

                if (f_table.Count < 16777216) {
                    f_table.Add(f, yf);
                }
            }
            else {
                yf = f_table[f];
            }

            if (n == 0) {
                return yf;
            }

            if (!n_table.ContainsKey(n)) {
                yn = MultiPrecision<N>.Ln2 * n;

                if (n_table.Count < 16777216) {
                    n_table.Add(n, yn);
                }
            }
            else {
                yn = n_table[n];
            }

            return yf + yn;
        }
    }
}
