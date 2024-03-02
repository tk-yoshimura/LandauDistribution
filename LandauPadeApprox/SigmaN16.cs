using MultiPrecision;
// Copyright (c) T.Yoshimura 2024
// https://github.com/tk-yoshimura

namespace LandauPadeApprox {
    internal static class Sigma<N> where N : struct, IConstant {

        public static MultiPrecision<N> Value(MultiPrecision<N> lambda) {
            MultiPrecision<N> sigma = MultiPrecision<N>.Exp(-lambda - 1);

            return sigma;
        }
    }
}