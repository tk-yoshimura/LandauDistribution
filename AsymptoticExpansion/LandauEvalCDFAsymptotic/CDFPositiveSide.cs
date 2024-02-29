using MultiPrecision;
using System.Collections.ObjectModel;

namespace LandauEvalCDFAsymptotic {
    public static class CDFPositiveSide<N> where N : struct, IConstant {
        public static readonly ReadOnlyCollection<MultiPrecision<N>> Coefs;

        static CDFPositiveSide() {
            List<MultiPrecision<N>> coefs = [];

            string filepath = Path.GetFullPath("../../../..").Split('/', '\\')[^1] == "LandauDistribution"
                ? "../../../../results_disused/asymp_plus_cdf_poly_bits2048.bin"
                : "../../../../../results_disused/asymp_plus_cdf_poly_bits2048.bin";

            using BinaryReader stream = new(File.OpenRead(filepath));

            for (int i = 0; i < 64; i++) {
                MultiPrecision<Pow2.N64> coef = stream.ReadMultiPrecision<Pow2.N64>();
                coefs.Add(coef.Convert<N>());
            }

            Coefs = Array.AsReadOnly(coefs.ToArray());
        }

        public static MultiPrecision<N> Omega(MultiPrecision<N> lambda) {
            MultiPrecision<Plus1<N>> lambda_p1 = lambda.Convert<Plus1<N>>();
            MultiPrecision<Plus1<N>> omega = lambda_p1 * (1 - MultiPrecision<Plus1<N>>.Log(lambda_p1) / (lambda_p1 + 1));

            while (true) {
                MultiPrecision<Plus1<N>> f = omega + MultiPrecision<Plus1<N>>.Log(omega) - lambda_p1;
                MultiPrecision<Plus1<N>> d = (2 * omega * (omega + 1) * f) / (2 * MultiPrecision<Plus1<N>>.Square(omega + 1) + f);
                omega -= d;

                if (MultiPrecision<Plus1<N>>.IsZero(d) || omega.Exponent - d.Exponent > MultiPrecision<N>.Bits) {
                    break;
                }
            }

            return omega.Convert<N>();
        }

        public static MultiPrecision<N> Value(MultiPrecision<N> lambda, int terms) {
            MultiPrecision<N> invomega = 1 / Omega(lambda);

            MultiPrecision<N> u = 1, s = 0;
            foreach (MultiPrecision<N> coef in Coefs.Take(terms)) {
                u *= invomega;
                s += coef * u;
            }

            return s;
        }
    }
}
