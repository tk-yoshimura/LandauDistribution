﻿using MultiPrecision;
using MultiPrecisionRootFinding;

namespace Digits82 {
    class QuantileRootFind {
        static void Main_() {
            {
                List<MultiPrecision<N12>> cdfs = new();

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
                    for (double v = 1; v > 0.5; v -= 1d / 256) {
                        cdfs.Add(MultiPrecision<N12>.Ldexp(v, exp));
                    }
                }

                for (int exp = -16; exp > -64; exp--) {
                    for (double v = 1; v > 0.5; v -= 1d / 64) {
                        cdfs.Add(MultiPrecision<N12>.Ldexp(v, exp));
                    }
                }

                for (int exp = -64; exp > -256; exp--) {
                    for (double v = 1; v > 0.5; v -= 1d / 16) {
                        cdfs.Add(MultiPrecision<N12>.Ldexp(v, exp));
                    }
                }

                for (int exp = -256; exp > -1024; exp--) {
                    for (double v = 1; v > 0.5; v -= 1d / 4) {
                        cdfs.Add(MultiPrecision<N12>.Ldexp(v, exp));
                    }
                }

                for (int exp = -1024; exp > -4096; exp--) {
                    cdfs.Add(MultiPrecision<N12>.Ldexp(1, exp));
                }

                for (int exp = -4096; exp >= -16384; exp -= 4) {
                    cdfs.Add(MultiPrecision<N12>.Ldexp(1, exp));
                }

                MultiPrecision<N12> lambda = "1.355780420990801325032092809390650910517114086024118870";

                using StreamWriter sw = new("../../../../results_disused/quantile_cdf_precision82_3.csv");
                sw.WriteLine("cdf(exp),cdf(frac),lambda,scaled_lambda,err");

                foreach (MultiPrecision<N12> cdf in cdfs) {
                    lambda = NewtonRaphsonFinder<N12>.RootFind((l) => (CDF.Value(l) - cdf, PDF.Value(l)), lambda, (-20, "inf"), accurate_bits: 275);
                    MultiPrecision<N12> err = (CDF.Value(lambda) - cdf) / CDF.Value(lambda);

                    MultiPrecision<N12> s = lambda / (-MultiPrecision<N12>.Log(-MultiPrecision<N12>.Log(cdf)) - 1);

                    Console.WriteLine($"{cdf:e5},{lambda:e16},{s:e16},{err:e10}");
                    sw.WriteLine($"{cdf.Exponent},{MultiPrecision<N12>.Ldexp(cdf, -cdf.Exponent)},{lambda},{s},{err:e10}");
                }

                sw.Flush();
            }

            //{
            //    List<MultiPrecision<N12>> ccdfs = new();
            //
            //    for (double ccdf = 0.75; ccdf > 0.5; ccdf -= 1d / 8192) {
            //        ccdfs.Add(ccdf);
            //    }
            //
            //    for (int exp0 = -1, h = 4096; exp0 >= -256; exp0 *= 2, h /= 2) {
            //        for (int exp = exp0; exp > exp0 * 2; exp--) {
            //            for (double v = 1; v > 0.5; v -= 1d / h) {
            //                ccdfs.Add(MultiPrecision<N12>.Ldexp(v, exp));
            //            }
            //        }
            //    }
            //
            //    ccdfs.Add(MultiPrecision<N12>.Ldexp(1, -256));
            //
            //    MultiPrecision<N12> lambda = "1.355780420990801325032092809390650910517114086024118870";
            //
            //    using StreamWriter sw = new("../../../../results_disused/quantile_ccdf_precision82_3.csv");
            //    sw.WriteLine("ccdf,lambda,scaled_lambda,err");
            //
            //    foreach (MultiPrecision<N12> ccdf in ccdfs) {
            //        lambda = NewtonRaphsonFinder<N12>.RootFind((l) => (CDF.Value(l, is_complementary: true) - ccdf, -PDF.Value(l)), lambda, (-20, "inf"), accurate_bits: 275);
            //        MultiPrecision<N12> err = (CDF.Value(lambda, is_complementary: true) - ccdf) / CDF.Value(lambda, is_complementary: true);
            //
            //        MultiPrecision<N12> s = lambda * ccdf;
            //
            //        Console.WriteLine($"{ccdf:e5},{lambda:e16},{s:e16},{err:e10}");
            //        sw.WriteLine($"{ccdf.Exponent},{MultiPrecision<N12>.Ldexp(ccdf, -ccdf.Exponent)},{lambda},{s},{err:e10}");
            //    }
            //
            //    sw.WriteLine("0,inf,1,0");
            //
            //    sw.Flush();
            //}

            Console.WriteLine("END");
            Console.Read();
        }
    }
}