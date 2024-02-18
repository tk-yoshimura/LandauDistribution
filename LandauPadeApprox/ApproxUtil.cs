using MultiPrecision;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace LandauPadeApprox {
    internal static class ApproxUtil<N> where N : struct, IConstant {

        public static MultiPrecision<N> Pade(MultiPrecision<N> x, ReadOnlyCollection<(MultiPrecision<N> c, MultiPrecision<N> d)> table) {
            (MultiPrecision<N> sc, MultiPrecision<N> sd) = table[^1];

#if DEBUG
            Trace.Assert(x >= 0, $"must be positive! {x}");
#endif

            for (int i = table.Count - 2; i >= 0; i--) {
                sc = sc * x + table[i].c;
                sd = sd * x + table[i].d;
            }

#if DEBUG
            Trace.Assert(sd >= 0.5, $"pade denom digits loss! {x}");
#endif

            return sc / sd;
        }

        public static MultiPrecision<N> Poly(MultiPrecision<N> x, ReadOnlyCollection<MultiPrecision<N>> table) {
            MultiPrecision<N> s = table[^1];

            for (int i = table.Count - 2; i >= 0; i--) {
                s = s * x + table[i];
            }

            return s;
        }
    }
}