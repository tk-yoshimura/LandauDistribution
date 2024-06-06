using LandauEvalCDFAsymptotic;
using LandauEvalPDFAsymptotic;
using LandauPadeApprox;
using MultiPrecision;
using MultiPrecisionRootFinding;

namespace LandauEvalPadeApprox {
    internal class ExpectedQuantilePlusN16 {
        static void Main_() {
            MultiPrecision<N20> median = "1.3557804209908013250320928093907";

            using (StreamWriter sw = new("../../../../results_disused/quantile_upper_precision152_scaled.csv")) {
                sw.WriteLine("u:=-log2(p),v:=cquantile(p)*p");

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
                                x0: x, overshoot_decay: true, iters: 256, accurate_bits: 508
                            );
                        }
                        else {
                            x = NewtonRaphsonFinder<N20>.RootFind(
                                x => (
                                    CDFPositiveSide<N20>.Value(x, terms: 64) - p,
                                    -PDFPositiveSide<N20>.Value(x, terms: 64)
                                ),
                                x0: 1 / p, overshoot_decay: true, iters: 256, accurate_bits: 508
                            );
                        }

                        MultiPrecision<N20> v = x * p;

                        Console.WriteLine($"{u}\n{v:e20}");

                        sw.WriteLine($"{u},{v:e160}");
                    }
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}