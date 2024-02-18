using LandauPadeApprox;
using MultiPrecision;
using MultiPrecisionRootFinding;

namespace LandauEvalPadeApprox {
    class QuantileRootFind {
        static void Main_() {
            {
                List<MultiPrecision<N12>> cdfs = [];

                for (int exp = -1; exp > -2; exp--) {
                    for (double v = 1; v > 0.5; v -= 1d / 4096) {
                        cdfs.Add(MultiPrecision<N12>.Ldexp(v, exp));
                    }
                }

                for (int exp = -2; exp > -4; exp--) {
                    for (double v = 1; v > 0.5; v -= 1d / 2048) {
                        cdfs.Add(MultiPrecision<N12>.Ldexp(v, exp));
                    }
                }

                for (int exp = -4; exp > -16; exp--) {
                    for (double v = 1; v > 0.5; v -= 1d / 1024) {
                        cdfs.Add(MultiPrecision<N12>.Ldexp(v, exp));
                    }
                }

                for (int exp = -16; exp > -64; exp--) {
                    for (double v = 1; v > 0.5; v -= 1d / 128) {
                        cdfs.Add(MultiPrecision<N12>.Ldexp(v, exp));
                    }
                }

                for (int exp = -64; exp > -256; exp--) {
                    for (double v = 1; v > 0.5; v -= 1d / 32) {
                        cdfs.Add(MultiPrecision<N12>.Ldexp(v, exp));
                    }
                }

                for (int exp = -256; exp > -1024; exp--) {
                    for (double v = 1; v > 0.5; v -= 1d / 8) {
                        cdfs.Add(MultiPrecision<N12>.Ldexp(v, exp));
                    }
                }

                for (int exp = -1024; exp > -4096; exp--) {
                    for (double v = 1; v > 0.5; v -= 1d / 2) {
                        cdfs.Add(MultiPrecision<N12>.Ldexp(v, exp));
                    }
                }

                for (int exp = -4096; exp > -16384; exp -= 2) {
                    cdfs.Add(MultiPrecision<N12>.Ldexp(1, exp));
                }

                for (int exp = -16384; exp > -65536; exp -= 8) {
                    cdfs.Add(MultiPrecision<N12>.Ldexp(1, exp));
                }

                for (int exp = -65536; exp > -262144; exp -= 32) {
                    cdfs.Add(MultiPrecision<N12>.Ldexp(1, exp));
                }

                for (int exp = -262144; exp > -1048576; exp -= 128) {
                    cdfs.Add(MultiPrecision<N12>.Ldexp(1, exp));
                }

                for (int exp = -1048576; exp > -4194304; exp -= 512) {
                    cdfs.Add(MultiPrecision<N12>.Ldexp(1, exp));
                }

                for (int exp = -4194304; exp >= -16777216; exp -= 2048) {
                    cdfs.Add(MultiPrecision<N12>.Ldexp(1, exp));
                }

                MultiPrecision<N12> lambda = "1.355780420990801325032092809390650910517114086024118870";

                using StreamWriter sw = new("../../../../results_disused/quantile_cdf_precision102_2.csv");
                sw.WriteLine("cdf(exp),cdf(frac),lambda,scaled_lambda,err");

                foreach (MultiPrecision<N12> cdf in cdfs) {
                    lambda = NewtonRaphsonFinder<N12>.RootFind((l) => (CDFPadeN12.Value(l) - cdf, PDFPadeN12.Value(l)), lambda, (-20, "inf"), accurate_bits: 340);
                    MultiPrecision<N12> err = (CDFPadeN12.Value(lambda) - cdf) / CDFPadeN12.Value(lambda);

                    MultiPrecision<N12> s = lambda / (-MultiPrecision<N12>.Log(-MultiPrecision<N12>.Log(cdf)) - 1);

                    Console.WriteLine($"{cdf:e5},{lambda:e16},{s:e16},{err:e10}");
                    sw.WriteLine($"{cdf.Exponent},{MultiPrecision<N12>.Ldexp(cdf, -cdf.Exponent)},{lambda},{s},{err:e10}");
                }

                sw.Flush();
            }

            {
                List<MultiPrecision<N12>> ccdfs = [];

                for (int exp0 = -1, h = 2048; exp0 >= -512; exp0 *= 2, h /= 2) {
                    for (int exp = exp0; exp > exp0 * 2 && exp >= -400; exp--) {
                        for (double v = 1; v > 0.5; v -= 1d / h) {
                            ccdfs.Add(MultiPrecision<N12>.Ldexp(v, exp));
                        }
                    }
                }

                MultiPrecision<N12> lambda = "1.355780420990801325032092809390650910517114086024118870";

                using StreamWriter sw = new("../../../../results_disused/quantile_ccdf_precision103_2.csv");
                sw.WriteLine("ccdf,lambda,scaled_lambda,err");

                foreach (MultiPrecision<N12> ccdf in ccdfs) {
                    lambda = NewtonRaphsonFinder<N12>.RootFind((l) => (CDFPadeN12.Value(l, complementary: true) - ccdf, -PDFPadeN12.Value(l)), lambda, (-20, "inf"), accurate_bits: 344);
                    MultiPrecision<N12> err = (CDFPadeN12.Value(lambda, complementary: true) - ccdf) / CDFPadeN12.Value(lambda, complementary: true);

                    MultiPrecision<N12> s = lambda * ccdf;

                    Console.WriteLine($"{ccdf:e5},{lambda:e16},{s:e16},{err:e10}");
                    sw.WriteLine($"{ccdf.Exponent},{MultiPrecision<N12>.Ldexp(ccdf, -ccdf.Exponent)},{lambda},{s},{err:e10}");
                }

                sw.WriteLine("0,inf,1,0");

                sw.Flush();
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}