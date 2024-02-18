using LandauSymbolicArithmetic;
using System.Numerics;

namespace LandauPDFPlusAsymptotic {
    internal static class ZetaTable {
        private static readonly List<Term> table = [
            new Term(new Fraction(-1, 2)),
            new Term(1, new Symbol(pi: 0, gamma: 1))
        ];

        public static Term Value(int n) {
            ArgumentOutOfRangeException.ThrowIfNegative(n);

            for (int k = table.Count; k <= n; k++) {
                if ((k & 1) == 0) {
                    Fraction c = BernTable.Value(k) / Factorial.Value(k) * (BigInteger.One << checked(k - 1));

                    table.Add(new Term(Fraction.Abs(c), new Symbol(pi: k, gamma: 0)));
                }
                else {
                    table.Add(new Term(1, new Symbol(pi: 0, gamma: 0, zeta: (k, 1))));
                }
            }

            return table[n];
        }
    }
}
