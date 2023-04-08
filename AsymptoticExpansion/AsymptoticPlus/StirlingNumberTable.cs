using System.Numerics;

namespace AsymptoticPlus {
    public static class StirlingNumberTable {
        private static readonly Dictionary<(int n, int k), BigInteger> table = new();

        public static BigInteger UnsignedValue(int n, int k) {
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
                    BigInteger v = UnsignedValue(n - 1, k - 1) + (n - 1) * UnsignedValue(n - 1, k);

                    table.Add((n, k), v);
                }
            }

            return table[(n, k)];
        }

        public static BigInteger SignedValue(int n, int k) {
            return (((n - k) & 1) == 0) ? UnsignedValue(n, k) : -UnsignedValue(n, k);
        }
    }
}
