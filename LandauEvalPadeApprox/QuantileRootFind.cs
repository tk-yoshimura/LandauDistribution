using LandauPadeApprox;
using MultiPrecision;
using MultiPrecisionRootFinding;

namespace LandauEvalPadeApprox {
    class QuantileRootFind {
        static void Main_() {
            {
                List<MultiPrecision<Pow2.N16>> cdfs = [];

                for (int exp = -1; exp > -2; exp--) {
                    for (double v = 1; v > 0.5; v -= 1d / 4096) {
                        cdfs.Add(MultiPrecision<Pow2.N16>.Ldexp(v, exp));
                    }
                }

                for (int exp = -2; exp > -4; exp--) {
                    for (double v = 1; v > 0.5; v -= 1d / 2048) {
                        cdfs.Add(MultiPrecision<Pow2.N16>.Ldexp(v, exp));
                    }
                }

                for (int exp = -4; exp > -16; exp--) {
                    for (double v = 1; v > 0.5; v -= 1d / 1024) {
                        cdfs.Add(MultiPrecision<Pow2.N16>.Ldexp(v, exp));
                    }
                }

                for (int exp = -16; exp > -64; exp--) {
                    for (double v = 1; v > 0.5; v -= 1d / 128) {
                        cdfs.Add(MultiPrecision<Pow2.N16>.Ldexp(v, exp));
                    }
                }

                for (int exp = -64; exp > -256; exp--) {
                    for (double v = 1; v > 0.5; v -= 1d / 32) {
                        cdfs.Add(MultiPrecision<Pow2.N16>.Ldexp(v, exp));
                    }
                }

                for (int exp = -256; exp > -1024; exp--) {
                    for (double v = 1; v > 0.5; v -= 1d / 8) {
                        cdfs.Add(MultiPrecision<Pow2.N16>.Ldexp(v, exp));
                    }
                }

                for (int exp = -1024; exp > -4096; exp--) {
                    for (double v = 1; v > 0.5; v -= 1d / 2) {
                        cdfs.Add(MultiPrecision<Pow2.N16>.Ldexp(v, exp));
                    }
                }

                for (int exp = -4096; exp > -16384; exp -= 2) {
                    cdfs.Add(MultiPrecision<Pow2.N16>.Ldexp(1, exp));
                }

                for (int exp = -16384; exp > -65536; exp -= 8) {
                    cdfs.Add(MultiPrecision<Pow2.N16>.Ldexp(1, exp));
                }

                for (int exp = -65536; exp > -262144; exp -= 32) {
                    cdfs.Add(MultiPrecision<Pow2.N16>.Ldexp(1, exp));
                }

                for (int exp = -262144; exp > -1048576; exp -= 128) {
                    cdfs.Add(MultiPrecision<Pow2.N16>.Ldexp(1, exp));
                }

                for (int exp = -1048576; exp > -4194304; exp -= 512) {
                    cdfs.Add(MultiPrecision<Pow2.N16>.Ldexp(1, exp));
                }

                for (int exp = -4194304; exp >= -16777216; exp -= 2048) {
                    cdfs.Add(MultiPrecision<Pow2.N16>.Ldexp(1, exp));
                }

                MultiPrecision<Pow2.N16> lambda = "1.355780420990801325032092809390650910517114086024118870";

                using StreamWriter sw = new("../../../../results_disused/quantile_cdf_precision152.csv");
                sw.WriteLine("cdf(exp),cdf(frac),lambda,scaled_lambda,err");

                foreach (MultiPrecision<Pow2.N16> cdf in cdfs) {
                    lambda = NewtonRaphsonFinder<Pow2.N16>.RootFind(
                        (l) => (CDFPadeN16.Value(l) - cdf, PDFPadeN16.Value(l)), lambda, (-20, "inf"),
                        iters: 256, overshoot_decay: true
                    );

                    MultiPrecision<Pow2.N16> err = (CDFPadeN16.Value(lambda) - cdf) / CDFPadeN16.Value(lambda);

                    MultiPrecision<Pow2.N16> s = lambda / (-MultiPrecision<Pow2.N16>.Log(-MultiPrecision<Pow2.N16>.Log(cdf)) - 1);

                    Console.WriteLine($"{cdf:e5},{lambda:e16},{s:e16},{err:e10}");
                    sw.WriteLine($"{cdf.Exponent},{MultiPrecision<Pow2.N16>.Ldexp(cdf, -cdf.Exponent)},{lambda},{s},{err:e10}");
                }

                sw.Flush();
            }

            {
                List<MultiPrecision<Pow2.N16>> ccdfs = [];

                for (int exp0 = -1, h = 2048; exp0 >= -512; exp0 *= 2, h /= 2) {
                    for (int exp = exp0; exp > exp0 * 2 && exp >= -400; exp--) {
                        for (double v = 1; v > 0.5; v -= 1d / h) {
                            ccdfs.Add(MultiPrecision<Pow2.N16>.Ldexp(v, exp));
                        }
                    }
                }

                MultiPrecision<Pow2.N16> lambda = "1.355780420990801325032092809390650910517114086024118870";

                using StreamWriter sw = new("../../../../results_disused/quantile_ccdf_precision152.csv");
                sw.WriteLine("ccdf,lambda,scaled_lambda,err");

                foreach (MultiPrecision<Pow2.N16> ccdf in ccdfs) {
                    lambda = NewtonRaphsonFinder<Pow2.N16>.RootFind(
                        (l) => (CDFPadeN16.Value(l, complementary: true) - ccdf, -PDFPadeN16.Value(l)), lambda, (-20, "inf"),
                        iters: 256, overshoot_decay: true
                    );

                    MultiPrecision<Pow2.N16> err = (CDFPadeN16.Value(lambda, complementary: true) - ccdf) / CDFPadeN16.Value(lambda, complementary: true);

                    MultiPrecision<Pow2.N16> s = lambda * ccdf;

                    Console.WriteLine($"{ccdf:e5},{lambda:e16},{s:e16},{err:e10}");
                    sw.WriteLine($"{ccdf.Exponent},{MultiPrecision<Pow2.N16>.Ldexp(ccdf, -ccdf.Exponent)},{lambda},{s},{err:e10}");
                }

                sw.WriteLine("0,inf,1,0");

                sw.Flush();
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}