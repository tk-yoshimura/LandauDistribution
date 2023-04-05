using SeriesExpansion;
using System.Numerics;

namespace ToNumeric {
    public static class SymbolicPolyParser {
        public static SymbolicPoly ParsePoly(string str) {
            if (str.Contains('\n')) {
                throw new FormatException();
            }

            Term[] terms = str.Replace("+", " +").Replace("-", " -").Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => ParseTerm(s)).ToArray();

            return new SymbolicPoly(terms);
        }

        public static Term ParseTerm(string str) {
            string[] item = str.Split('*');

            Fraction f = 1;
            int gamma = 0, pi = 0;
            List<(int n, int pow)> zeta = new();

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

            return new Term(f, new Symbol(pi, gamma, zeta.ToArray()));
        }
    }
}
