using AsymptoticMinus;

namespace SeriesExpansion {
    public static class RTable {
        private readonly static Dictionary<(int n, int k), SymbolicPoly> table = new();

        public static SymbolicPoly Value(int j, int k) { 
            if (j < 0 || k < 0) {
                throw new ArgumentOutOfRangeException($"{nameof(j)}, {nameof(k)}");
            }

            if (!table.ContainsKey((j, k))) {
                SymbolicPoly poly = new();

                for (int m = 0; m <= Math.Min(j, k); m++) {
                    SymbolicPoly poly_append = BTable.Value(k - m) * new Term(StirlingNumberTable.SignedValue(j + 1, m + 1));
 
                    if ((m & 1) == 0) {
                        poly += poly_append;
                    }
                    else{
                        poly -= poly_append;
                    }
                }

                poly *= new Term((j & 1) == 0 ? Factorial.Value(k) : -Factorial.Value(k));

                table.Add((j, k), poly);
            }

            return table[(j, k)];
        }
    }
}
