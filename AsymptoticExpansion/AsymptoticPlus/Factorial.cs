using System.Numerics;

namespace AsymptoticPlus {
    internal static class Factorial {
        private static readonly List<BigInteger> table = [
            1,
            1
        ];

        public static BigInteger Value(int n) {
            for (int k = table.Count; k <= n; k++) {
                table.Add(table[^1] * k);
            }

            return table[n];
        }
    }
}
