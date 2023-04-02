using System.Numerics;

namespace SeriesExpansion {
    internal static class Factorial {
        private readonly static List<BigInteger> table = new() {
            1, 1
        };

        public static BigInteger Value(int n) {
            for (int k = table.Count; k <= n; k++) {
                table.Add(table[^1] * k);
            }

            return table[n];
        }
    }
}
