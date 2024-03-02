using MultiPrecision;
// Copyright (c) T.Yoshimura 2024
// https://github.com/tk-yoshimura

namespace LandauPadeApprox {
    internal static class Omega<N> where N : struct, IConstant {

        public static MultiPrecision<N> Value(MultiPrecision<N> lambda) {
            if (!MultiPrecision<N>.IsFinite(lambda)) {
                return MultiPrecision<N>.NaN;
            }

            MultiPrecision<Plus4<N>> lambda_p1 = lambda.Convert<Plus4<N>>();
            MultiPrecision<Plus4<N>> omega = lambda_p1 * (1 - MultiPrecision<Plus4<N>>.Log(lambda_p1) / (lambda_p1 + 1));

            while (true) {
                MultiPrecision<Plus4<N>> f = omega + MultiPrecision<Plus4<N>>.Log(omega) - lambda_p1;
                MultiPrecision<Plus4<N>> d = (2 * omega * (omega + 1) * f) / (2 * MultiPrecision<Plus4<N>>.Square(omega + 1) + f);
                omega -= d;

                if (MultiPrecision<Plus4<N>>.IsZero(d) || omega.Exponent - d.Exponent > MultiPrecision<N>.Bits) {
                    break;
                }
            }

            return omega.Convert<N>();
        }
    }
}