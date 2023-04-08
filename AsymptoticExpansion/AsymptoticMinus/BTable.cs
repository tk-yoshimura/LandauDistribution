using SymbolicArithmetic;

namespace AsymptoticMinus {
    internal static class BTable {
        private static readonly List<Fraction> table = new() {
            0, 0, 0
        };

        public static (Fraction f, bool imag) Value(int n) {
            for (int k = table.Count; k <= n; k++) {
                table.Add(new Fraction((k - 1) % 4 / 2 == 0 ? -1 : +1, checked(k * (k - 1))));
            }

            return (table[n], (n & 1) == 1);
        }
    }
}
