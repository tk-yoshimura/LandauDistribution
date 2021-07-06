using MultiPrecision;
using System.Collections.Generic;

namespace LandauDistribution {
    static class LogCache<N> where N : struct, IConstant {
        static readonly Dictionary<MultiPrecision<N>, MultiPrecision<N>> table = new();

        public static MultiPrecision<N> Value(MultiPrecision<N> t) {
            if (!table.ContainsKey(t)) {
                MultiPrecision<N> y = MultiPrecision<N>.Log(t);

                if (table.Count < 268435456) {
                    table.Add(t, y);
                }

                return y;
            }

            return table[t];
        }
    }
}
