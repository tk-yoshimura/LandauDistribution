// Copyright (c) T.Yoshimura 2024
// https://github.com/tk-yoshimura

using MultiPrecision;

namespace LandauPadeApprox {
    public static class QuantilePadeN16 {
        public static MultiPrecision<Pow2.N16> Value(MultiPrecision<Pow2.N16> p, bool complementary = false) {
            if (complementary) {
                MultiPrecision<Pow2.N16> y = (p.Exponent < -1) ? CCDF(p) : CDF(1 - p);

                return y;
            }
            else {
                MultiPrecision<Pow2.N16> y = (p.Exponent < -1) ? CDF(p) : CCDF(1 - p);

                return y;
            }
        }

        private static MultiPrecision<Pow2.N16> CDF(MultiPrecision<Pow2.N16> p) {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(p, 0.5);

            throw new NotImplementedException();
        }

        private static MultiPrecision<Pow2.N16> CCDF(MultiPrecision<Pow2.N16> p) {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(p, 0.5);

            throw new NotImplementedException();
        }
    }
}
