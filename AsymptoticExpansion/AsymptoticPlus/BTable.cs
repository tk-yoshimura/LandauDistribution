using SymbolicArithmetic;

namespace AsymptoticPlus {
    public static class BTable {
        private static readonly List<SymbolicPoly> table = [
            new SymbolicPoly(new Term(1))
        ];

        public static SymbolicPoly Value(int n) {
            ArgumentOutOfRangeException.ThrowIfNegative(n);

            for (int k = table.Count; k <= n; k++) {
                SymbolicPoly poly = new();

                for (int i = 1; i <= k; i++) {
                    SymbolicPoly poly_add = Value(k - i) * ZetaTable.Value(i) * new Term(new Fraction(1, k));

                    if ((i & 1) == 0) {
                        poly += poly_add;
                    }
                    else {
                        poly -= poly_add;
                    }
                }

                table.Add(poly);
            }

            return table[n];
        }
    }
}
