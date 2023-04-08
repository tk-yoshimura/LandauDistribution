using SeriesExpansion;

namespace AsymptoticMinus {
    internal static class CTable {
        private static readonly List<Poly> table = new() {
            new Poly(1), new Poly(0), new Poly(0)
        };

        public static (Poly p, bool imag) Value(int n) {
            for (int k = table.Count; k <= n; k++) {
                Fraction[] f = (new Fraction[k]).Select(_ => new Fraction(0)).ToArray();
                int order = 1;

                for (int i = 1; i <= k; i++) {
                    (Fraction f, bool imag) b = BTable.Value(i);
                    (Poly p, bool imag) c = Value(k - i);

                    if (b.imag && c.imag) {
                        b.f *= -1;
                    }

                    b.f *= i;

                    Poly v = c.p * b.f;

                    for (int j = 0; j < v.Length; j++) {
                        if (v[j].Numer != 0) {
                            f[j + 1] += v[j];
                            order = Math.Max(order, j + 2);
                        }
                    }
                }

                Poly p = new Poly(f[..order]) * new Fraction(1, k);

                table.Add(p);
            }

            return (table[n], (n & 1) == 1);
        }
    }
}
