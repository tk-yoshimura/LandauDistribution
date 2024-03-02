using MultiPrecision;
using System.Collections.ObjectModel;

namespace LandauEvalPDFAsymptotic {
    public static class PDFPositiveSide<N> where N : struct, IConstant {
        public static readonly ReadOnlyCollection<MultiPrecision<N>> Coefs;

        static PDFPositiveSide() {
            if (default(N).Value > 64) {
                throw new ArithmeticException($"Unsupported long {nameof(N)}");
            }

            List<MultiPrecision<N>> coefs = [];

            string filepath = Path.GetFullPath("../../../..").Split('/', '\\')[^1] == "LandauDistribution"
                ? "../../../../results_disused/asymp_plus_pdf_poly_bits2048.bin"
                : "../../../../../results_disused/asymp_plus_pdf_poly_bits2048.bin";

            using BinaryReader stream = new(File.OpenRead(filepath));

            for (int i = 0; i < 64; i++) {
                MultiPrecision<Pow2.N64> coef = stream.ReadMultiPrecision<Pow2.N64>();
                coefs.Add(coef.Convert<N>());
            }

            Coefs = Array.AsReadOnly(coefs.ToArray());
        }

        public static MultiPrecision<N> Omega(MultiPrecision<N> lambda) {
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

        public static MultiPrecision<N> Value(MultiPrecision<N> lambda, int terms) {
            MultiPrecision<N> invomega = 1 / Omega(lambda);

            MultiPrecision<N> u = invomega, s = 0;
            foreach (MultiPrecision<N> coef in Coefs.Take(terms)) {
                u *= invomega;
                s += coef * u;
            }

            return s;
        }
    }
}
