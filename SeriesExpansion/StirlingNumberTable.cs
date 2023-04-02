using AsymptoticMinus;
using System.Numerics;

namespace SeriesExpansion {
    public static class StirlingNumberTable {
        private readonly static Dictionary<(int n, int k), BigInteger> table = new();

        public static BigInteger Value(int n, int k) {
            if (n < 0 || n < k) {
                throw new ArgumentOutOfRangeException($"{nameof(n)}, {nameof(k)}");
            }

            if (k == 0) {
                return 0;
            }

            if (!table.ContainsKey((n, k))) {
                if (k == 1) {
                    table.Add((n, k), Factorial.Value(n - 1));
                }
                else if (n == k) {
                    table.Add((n, k), 1);
                }
                else {
                    BigInteger v = Value(n - 1, k - 1) + (n - 1) * Value(n - 1, k);

                    table.Add((n, k), v);
                }
            }

            return table[(n, k)];
        }
    }
}
