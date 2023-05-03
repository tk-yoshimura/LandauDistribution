﻿using MultiPrecision;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Digits100 {
    internal static class ApproxUtil {

        public static MultiPrecision<N12> Pade(MultiPrecision<N12> x, ReadOnlyCollection<(MultiPrecision<N12> c, MultiPrecision<N12> d)> table) {
            (MultiPrecision<N12> sc, MultiPrecision<N12> sd) = table[^1];

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

        public static MultiPrecision<N12> Poly(MultiPrecision<N12> x, ReadOnlyCollection<MultiPrecision<N12>> table) {
            MultiPrecision<N12> s = table[^1];

            for (int i = table.Count - 2; i >= 0; i--) {
                s = s * x + table[i];
            }

            return s;
        }
    }
}