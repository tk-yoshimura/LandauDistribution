using MultiPrecision;

namespace LandauDistribution {
    static class ExpCache<N> where N : struct, IConstant {
        static readonly Dictionary<MultiPrecision<N>, MultiPrecision<N>> cache = new();

        public static MultiPrecision<N> Value(MultiPrecision<N> t) {
            if (!cache.ContainsKey(t)) {
                cache.Add(t, MultiPrecision<N>.Exp(t));
            }

            return cache[t];
        }
    }

    static class LogCache<N> where N : struct, IConstant {
        static readonly Dictionary<MultiPrecision<N>, MultiPrecision<N>> cache = new();

        public static MultiPrecision<N> Value(MultiPrecision<N> t) {
            if (!cache.ContainsKey(t)) {
                cache.Add(t, MultiPrecision<N>.Log(t));
            }

            return cache[t];
        }
    }

    static class SinPICache<N> where N : struct, IConstant {
        static readonly Dictionary<MultiPrecision<N>, MultiPrecision<N>> cache = new();

        public static MultiPrecision<N> Value(MultiPrecision<N> t) {
            if (!cache.ContainsKey(t)) {
                cache.Add(t, MultiPrecision<N>.SinPI(t));
            }

            return cache[t];
        }
    }

    static class CosPICache<N> where N : struct, IConstant {
        static readonly Dictionary<MultiPrecision<N>, MultiPrecision<N>> cache = new();

        public static MultiPrecision<N> Value(MultiPrecision<N> t) {
            if (!cache.ContainsKey(t)) {
                cache.Add(t, MultiPrecision<N>.CosPI(t));
            }

            return cache[t];
        }
    }

    static class PowCache<N> where N : struct, IConstant {
        static readonly Dictionary<MultiPrecision<N>, MultiPrecision<N>> cache = new();


        public static MultiPrecision<N> Value(MultiPrecision<N> t) {
            if (!cache.ContainsKey(t)) {
                cache.Add(t, MultiPrecision<N>.Pow(t, -t));
            }

            return cache[t];
        }
    }
}
