using MultiPrecision;
using System.Collections.Generic;

namespace LandauDistribution {
    static class PowCache<N> where N : struct, IConstant {
        static readonly Dictionary<MultiPrecision<N>, MultiPrecision<N>> table = new();

        static PowCache() {
            table.Add(0, 1);
        }

        public static MultiPrecision<N> Value(MultiPrecision<N> t) {
            if (!table.ContainsKey(t)) {
                MultiPrecision<Plus1<N>> t_ex = t.Convert<Plus1<N>>();

                MultiPrecision<Plus1<N>> y = MultiPrecision<Plus1<N>>.Pow(t_ex, -t_ex);

                if (table.Count < 67108864) {
                    table.Add(t, y.Convert<N>());
                }

                return y.Convert<N>();
            }

            return table[t];
        }
    }
}
