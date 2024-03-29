﻿using MultiPrecision;
using System.Collections.ObjectModel;

namespace LandauEvalCDFAsymptotic {
    public static class CDFNegativeSide<N> where N : struct, IConstant {
        public static readonly ReadOnlyCollection<MultiPrecision<N>> Coefs;
        static readonly MultiPrecision<N> c = 1 / MultiPrecision<N>.Sqrt(2 * MultiPrecision<N>.PI);

        static CDFNegativeSide() {
            List<MultiPrecision<N>> coefs = [];

            string filepath = Path.GetFullPath("../../../..").Split('/', '\\')[^1] == "LandauDistribution"
                ? "../../../../results_disused/asymp_minus_cdf_poly_frac.txt"
                : "../../../../../results_disused/asymp_minus_cdf_poly_frac.txt";

            using StreamReader stream = new(filepath);

            while (!stream.EndOfStream) {
                string? line = stream.ReadLine();

                if (string.IsNullOrWhiteSpace(line)) {
                    break;
                }

                string[] item = line.Split('/');

                MultiPrecision<N> coef = item.Length <= 1 ? item[0] : MultiPrecision<Plus1<N>>.Div(item[0], item[1]).Convert<N>();
                coefs.Add(coef.Convert<N>());
            }

            Coefs = Array.AsReadOnly(coefs.ToArray());
        }

        public static MultiPrecision<N> Sigma(MultiPrecision<N> lambda) {
            MultiPrecision<N> sigma = MultiPrecision<N>.Exp(-lambda - 1);

            return sigma.Convert<N>();
        }

        public static MultiPrecision<N> Value(MultiPrecision<N> lambda, int terms) {
            MultiPrecision<N> sigma = Sigma(lambda), invsigma = 1 / sigma;

            MultiPrecision<N> u = 1, s = 0;
            foreach (MultiPrecision<N> coef in Coefs.Take(terms)) {
                s += coef * u;
                u *= invsigma;
            }

            s *= Scale(sigma);

            return s;
        }

        public static MultiPrecision<N> Scale(MultiPrecision<N> sigma) {
            MultiPrecision<N> scale = 1 / (MultiPrecision<N>.Sqrt(sigma) * MultiPrecision<N>.Exp(sigma));

            return c * scale;
        }
    }
}
