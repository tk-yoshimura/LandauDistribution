using MultiPrecision;
using SeriesExpansion;

namespace ToNumeric {
    public static class SymbolNumeric<N> where N : struct, IConstant {
        private readonly static Dictionary<Symbol, MultiPrecision<N>> table = new();
        private readonly static Dictionary<int, MultiPrecision<N>> zeta = new();

        static SymbolNumeric() {
            using StreamReader sr = new("../../../../AsymptoticExpansion/zetan_bits8192.csv");

            int n = 3;

            while (!sr.EndOfStream) {
                string? line = sr.ReadLine();

                if (string.IsNullOrWhiteSpace(line)) {
                    break;
                }

                MultiPrecision<N> v = line.Replace("\r\n", string.Empty).Replace("\n", string.Empty);

                zeta.Add(n, 1 + v);
                n += 2;
            }
        }

        public static MultiPrecision<N> Value(Symbol symbol) {
            if (!table.ContainsKey(symbol)) {
                MultiPrecision<N> v = MultiPrecision<N>.Pow(MultiPrecision<N>.PI, symbol.PI) * MultiPrecision<N>.Pow(MultiPrecision<N>.EulerGamma, symbol.Gamma);

                foreach ((int n, int pow) in symbol.ZetaList) {
                    v *= MultiPrecision<N>.Pow(zeta[n], pow);
                }

                table.Add(symbol, v);
            }

            return table[symbol];
        }

        public static MultiPrecision<N> Value(Term term) {
            MultiPrecision<N> c = MultiPrecision<Plus1<N>>.Div(term.C.Numer.ToString(), term.C.Denom.ToString()).Convert<N>();
            MultiPrecision<N> s = Value(term.Symbol);

            return c * s;
        }

        public static MultiPrecision<N> Value<M>(SymbolicPoly poly) where M : struct, IConstant {
            MultiPrecision<M> s = poly.Terms.Select(term => SymbolNumeric<M>.Value(term)).Sum();

            return s.Convert<N>();
        }
    }
}
