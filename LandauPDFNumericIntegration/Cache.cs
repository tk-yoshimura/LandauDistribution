using MultiPrecision;

namespace LandauPDFNumericIntegration {
    static class ExpCache<N> where N : struct, IConstant {
        static readonly Dictionary<MultiPrecision<N>, MultiPrecision<N>> cache = [];

        public static MultiPrecision<N> Value(MultiPrecision<N> t) {
            if (!cache.TryGetValue(t, out MultiPrecision<N> value)) {
                value = MultiPrecision<N>.Exp(t);
                cache.Add(t, value);
            }

            return value;
        }
    }

    static class LogCache<N> where N : struct, IConstant {
        static readonly Dictionary<MultiPrecision<N>, MultiPrecision<N>> cache = [];

        public static MultiPrecision<N> Value(MultiPrecision<N> t) {
            if (!cache.TryGetValue(t, out MultiPrecision<N> value)) {
                value = MultiPrecision<N>.Log(t);
                cache.Add(t, value);
            }

            return value;
        }
    }

    static class SinPICache<N> where N : struct, IConstant {
        static readonly Dictionary<MultiPrecision<N>, MultiPrecision<N>> cache = [];

        public static MultiPrecision<N> Value(MultiPrecision<N> t) {
            if (!cache.TryGetValue(t, out MultiPrecision<N> value)) {
                value = MultiPrecision<N>.SinPI(t);
                cache.Add(t, value);
            }

            return value;
        }
    }

    static class CosPICache<N> where N : struct, IConstant {
        static readonly Dictionary<MultiPrecision<N>, MultiPrecision<N>> cache = [];

        public static MultiPrecision<N> Value(MultiPrecision<N> t) {
            if (!cache.TryGetValue(t, out MultiPrecision<N> value)) {
                value = MultiPrecision<N>.CosPI(t);
                cache.Add(t, value);
            }

            return value;
        }
    }

    static class PowCache<N> where N : struct, IConstant {
        static readonly Dictionary<MultiPrecision<N>, MultiPrecision<N>> cache = [];


        public static MultiPrecision<N> Value(MultiPrecision<N> t) {
            if (!cache.TryGetValue(t, out MultiPrecision<N> value)) {
                value = MultiPrecision<N>.Pow(t, -t);
                cache.Add(t, value);
            }

            return value;
        }
    }
}
