using SymbolicArithmetic;
using System.Numerics;

namespace AsymptoticPlus {
    public static class CTable {
        private static readonly Dictionary<(int n, int k), Term> table = new();

        public static Term Value(int j, int k) {
            if (j < 1 || k >= j) {
                throw new ArgumentOutOfRangeException($"{nameof(j)}, {nameof(k)}");
            }

            if ((checked(j + k) & 1) == 0) {
                return BigInteger.Zero;
            }

            if (!table.ContainsKey((j, k))) {
                Term term = new(new Fraction(1, Factorial.Value(j - k) * Factorial.Value(k)), new Symbol(pi: j - k - 1, gamma: 0));

                int n = checked(j + k - 1) / 2;

                table.Add((j, k), ((n & 1) == 0) ? term : -term);
            }

            return table[(j, k)];
        }
    }
}
