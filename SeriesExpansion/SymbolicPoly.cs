using System.Diagnostics;

namespace SeriesExpansion {
    [DebuggerDisplay("{ToString(),nq}")]
    public class SymbolicPoly {
        private readonly Dictionary<Symbol, Fraction> terms;

        public SymbolicPoly(params Term[] terms) {
            this.terms = new Dictionary<Symbol, Fraction>(terms.Select(term => new KeyValuePair<Symbol, Fraction>(term.Symbol, term.C)));
        }

        private SymbolicPoly(Dictionary<Symbol, Fraction> terms) {
            this.terms = new Dictionary<Symbol, Fraction>(terms);
        }

        public Term[] Terms => terms.Select(item => new Term(item.Value, item.Key)).OrderBy(term => term.Symbol).ToArray();

        public static SymbolicPoly operator +(SymbolicPoly p1, SymbolicPoly p2) {
            Dictionary<Symbol, Fraction> ret_terms = new(p1.terms);

            foreach (Symbol symbol in p2.terms.Keys) {
                Fraction f = p2.terms[symbol];

                if (ret_terms.ContainsKey(symbol)) {
                    ret_terms[symbol] += f;
                }
                else {
                    ret_terms.Add(symbol, f);
                }
            }

            Symbol[] symbols = ret_terms.Keys.ToArray();
            foreach (Symbol symbol in symbols) {
                Fraction f = ret_terms[symbol];

                if (f.Numer == 0) {
                    ret_terms.Remove(symbol);
                }
            }

            return new SymbolicPoly(ret_terms);
        }

        public static SymbolicPoly operator -(SymbolicPoly p1, SymbolicPoly p2) {
            Dictionary<Symbol, Fraction> ret_terms = new(p1.terms);

            foreach (Symbol symbol in p2.terms.Keys) {
                Fraction f = p2.terms[symbol];

                if (ret_terms.ContainsKey(symbol)) {
                    ret_terms[symbol] -= f;
                }
                else {
                    ret_terms.Add(symbol, f * -1);
                }
            }

            Symbol[] symbols = ret_terms.Keys.ToArray();
            foreach (Symbol symbol in symbols) {
                Fraction f = ret_terms[symbol];

                if (f.Numer == 0) {
                    ret_terms.Remove(symbol);
                }
            }

            return new SymbolicPoly(ret_terms);
        }

        public static SymbolicPoly operator *(SymbolicPoly p, Term c) {
            Dictionary<Symbol, Fraction> ret_terms = new();

            foreach (Symbol symbol in p.terms.Keys) {
                Symbol ret_symbol = symbol * c.Symbol;
                Fraction ret_fraction = p.terms[symbol] * c.C;
                
                ret_terms.Add(ret_symbol, ret_fraction);
            }

            return new SymbolicPoly(ret_terms);
        }

        public static SymbolicPoly operator *(SymbolicPoly p1, SymbolicPoly p2) {
            SymbolicPoly ret_poly = new();

            foreach (Symbol symbol in p2.terms.Keys) {
                ret_poly += p1 * new Term(p2.terms[symbol], symbol);
            }

            return ret_poly;
        }

        public override string ToString() {
            Term[] terms = Terms;

            string str = string.Join("+", terms.Select(term => term.ToString())).Replace("+-", "-");

            if (string.IsNullOrWhiteSpace(str)) {
                return "0";
            }

            return str;
        }
    }
}
