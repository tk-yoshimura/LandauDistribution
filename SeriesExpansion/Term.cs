using System.Diagnostics;
using System.Numerics;

namespace SeriesExpansion {
    [DebuggerDisplay("{ToString(),nq}")]
    public class Term {
        public Fraction C { private set; get; }
        public Symbol Symbol { private set; get; }

        public Term(Fraction c) {
            this.C = c;
            this.Symbol = Symbol.None;
        }

        public Term(Fraction c, Symbol symbol) {
            this.C = c;
            this.Symbol = symbol;
        }

        public static Term operator *(Term s1, Term s2) {
            return new Term(s1.C * s2.C, s1.Symbol * s2.Symbol);
        }

        public static Term operator *(Term s, Fraction c) {
            return new Term(s.C * c, s.Symbol);
        }

        public static Term operator *(Fraction c, Term s) {
            return s * c;
        }


        public override string ToString() {
            string symbol_str = Symbol.ToString();

            return symbol_str == "None"
                ? $"{C}"
                : (BigInteger.Abs(C.Numer) != 1 || C.Denom != 1)
                ? $"{C}*{symbol_str}"
                : (C.Numer == 1)
                ? symbol_str
                : $"-{symbol_str}";
        }
    }
}
