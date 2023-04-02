namespace SeriesExpansion {
    public static class BTable {
        private readonly static List<SymbolicPoly> table = new() {
            new SymbolicPoly(new Term(1))
        };

        public static SymbolicPoly Value(int n) { 
            if (n < 0) {
                throw new ArgumentOutOfRangeException(nameof(n));
            }

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
