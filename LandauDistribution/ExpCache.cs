using MultiPrecision;
using System.Collections.Generic;

namespace LandauDistribution {
    static class ExpCache<N> where N : struct, IConstant {
        static readonly Dictionary<MultiPrecision<N>, MultiPrecision<N>> f_table = new();
        static readonly Dictionary<long, MultiPrecision<N>> n_table = new();

        public static MultiPrecision<N> Value(MultiPrecision<N> t) {
            long n = (long)MultiPrecision<N>.Ceiling(t);
            MultiPrecision<N> f = t - n;

            MultiPrecision<N> yf, yn;
            if (!f_table.ContainsKey(f)) {
                yf = MultiPrecision<N>.Exp(f);

                if (f_table.Count < 268435456) {
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
                yn = MultiPrecision<N>.Pow(MultiPrecision<N>.E, n);

                if (n_table.Count < 268435456) {
                    n_table.Add(n, yn);
                }
            }
            else {
                yn = n_table[n];
            }

            return yf * yn;
        }
    }
}
