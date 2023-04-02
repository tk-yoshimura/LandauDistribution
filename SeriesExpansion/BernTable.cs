﻿namespace SeriesExpansion {
    public static class BernTable {
        private readonly static List<Fraction> table = new() {
            1, new Fraction(-1, 2)
        };

        public static Fraction Value(int n) {
            if (n < 0) {
                throw new ArgumentOutOfRangeException(nameof(n));
            }

            for (int k = table.Count; k <= n; k++) {
                Fraction f = 0;
                
                for (int i = 0; i < k; i++) {
                    f += new Fraction(Factorial.Value(k + 1), Factorial.Value(k + 1 - i) * Factorial.Value(i)) * Value(i);
                }

                f /= -k - 1;

                table.Add(f);
            }

            return table[n];
        }
    }
}
