using System.Diagnostics;
using System.Numerics;

namespace LandauSymbolicArithmetic {
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

        public static implicit operator Term(BigInteger n) {
            return new Term(new Fraction(n));
        }

        public static implicit operator Term(Fraction c) {
            return new Term(c);
        }

        public static Term operator -(Term s) {
            return new Term(-s.C, s.Symbol);
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

        public static Term Parse(string str) {
            string[] item = str.TrimStart('+', '-').Split('*');

            Fraction f = 1;
            int gamma = 0, pi = 0;
            List<(int n, int pow)> zeta = [];

            foreach (string s in item) {
                if (s.StartsWith("gamma")) {
                    gamma = s.Contains('^') ? int.Parse(s.Split('^')[1]) : 1;
                }
                else if (s.StartsWith("pi")) {
                    pi = s.Contains('^') ? int.Parse(s.Split('^')[1]) : 1;
                }
                else if (s.StartsWith("zeta")) {
                    int n = int.Parse(s.Split('(', ')')[1]);
                    int pow = s.Contains('^') ? int.Parse(s.Split('^')[1]) : 1;

                    zeta.Add((n, pow));
                }
                else {
                    string[] nd = s.Split('/');
                    BigInteger n = BigInteger.Parse(nd[0]), d = nd.Length > 1 ? BigInteger.Parse(nd[1]) : 1;

                    f = new Fraction(n, d);
                }
            }

            if (str.StartsWith('-')) {
                f = -f;
            }

            return new Term(f, new Symbol(pi, gamma, [.. zeta]));
        }
    }
}
