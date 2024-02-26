using LandauEvalCDFAsymptotic;
using LandauEvalPDFAsymptotic;
using LandauPadeApprox;
using MultiPrecision;
using MultiPrecisionRootFinding;

namespace LandauEvalPadeApprox {
    internal class ExpectedQuantileLogLogN16 {
        static void Main() {
            MultiPrecision<N20> median = "1.3557804209908013250320928093907";

            using (StreamWriter sw = new("../../../../results_disused/quantile_upper_precision152_loglog_2.csv")) {
                sw.WriteLine("u:=-log2(p),v:=log2(cquantile(p))-u");

                MultiPrecision<N20> x = median;

                for (MultiPrecision<N20> u0 = 1; u0 < 512; u0 *= 2) {
                    for (MultiPrecision<N20> u = u0; u < u0 * 2 || u == 512; u += u0 / (u0 < 4 ? 32768 : 16384)) {
                        MultiPrecision<N20> p = MultiPrecision<N20>.Pow2(-u);

                        if (u < 12) {
                            x = NewtonRaphsonFinder<N20>.RootFind(
                                x => (
                                    CDFPadeN16.Value(x.Convert<Pow2.N16>(), complementary: true).Convert<N20>() - p,
                                    -PDFPadeN16.Value(x.Convert<Pow2.N16>()).Convert<N20>()
                                ),
                                x0: x, overshoot_decay: true, iters: 256, accurate_bits: MultiPrecision<Pow2.N16>.Bits
                            );
                        }
                        else { 
                            x = NewtonRaphsonFinder<N20>.RootFind(
                                x => (
                                    CDFPositiveSide<N20>.Value(x, terms: 64).Convert<N20>() - p,
                                    -PDFPositiveSide<N20>.Value(x, terms: 64)
                                ),
                                x0: x, overshoot_decay: true, iters: 256
                            );
                        }

                        MultiPrecision<N20> v = MultiPrecision<N20>.Log2(x) - u;

                        Console.WriteLine($"{u}\n{v:e20}");

                        sw.WriteLine($"{u},{v}");
                    }
                }
            }

            //using (StreamWriter sw = new("../../../../results_disused/quantile_lower_precision152.csv")) {
            //    sw.WriteLine("u:=-log2(p),x:=quantile(p)");

            //    MultiPrecision<N20> x = median;

            //    for (MultiPrecision<N20> u0 = 1; u0 < 1048576; u0 *= 2) {
            //        for (MultiPrecision<N20> u = u0; u < u0 * 2 || u == 1048576; u += u0 / (u0 < 4 ? 32768 : 16384)) {
            //            MultiPrecision<N20> p = MultiPrecision<N20>.Pow2(-u);

            //            x = NewtonRaphsonFinder<N20>.RootFind(
            //                x => (
            //                    CDFPadeN16.Value(x.Convert<Pow2.N16>(), complementary: false).Convert<N20>() - p, 
            //                    PDFPadeN16.Value(x.Convert<Pow2.N16>()).Convert<N20>()
            //                ),
            //                x0: x, overshoot_decay: true, iters: 256, accurate_bits: MultiPrecision<Pow2.N16>.Bits
            //            );

            //            Console.WriteLine($"{u}\n{x:e20}");

            //            sw.WriteLine($"{u},{x}");
            //        }
            //    }
            //}

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
