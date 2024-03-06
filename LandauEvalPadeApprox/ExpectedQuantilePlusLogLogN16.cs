using LandauEvalCDFAsymptotic;
using LandauEvalPDFAsymptotic;
using LandauPadeApprox;
using MultiPrecision;
using MultiPrecisionRootFinding;

namespace LandauEvalPadeApprox {
    internal class ExpectedQuantilePlusLogLogN16 {
        static void Main_() {
            MultiPrecision<Pow2.N64> median = "1.3557804209908013250320928093907";

            using (StreamWriter sw = new("../../../../results_disused/quantile_upper_precision152_loglog.csv")) {
                sw.WriteLine("u:=-log2(p),v:=log2(cquantile(p))-u");

                MultiPrecision<Pow2.N64> x = median;

                for (MultiPrecision<Pow2.N64> u0 = 1; u0 < 512; u0 *= 2) {
                    for (MultiPrecision<Pow2.N64> u = u0; u < u0 * 2 || u == 512; u += u0 / (u0 < 4 ? 32768 : 16384)) {
                        MultiPrecision<Pow2.N64> p = MultiPrecision<Pow2.N64>.Pow2(-u);

                        if (u < 12) {
                            x = NewtonRaphsonFinder<Pow2.N64>.RootFind(
                                x => (
                                    CDFPadeN16.Value(x.Convert<Pow2.N16>(), complementary: true).Convert<Pow2.N64>() - p,
                                    -PDFPadeN16.Value(x.Convert<Pow2.N16>()).Convert<Pow2.N64>()
                                ),
                                x0: x, overshoot_decay: true, iters: 256, accurate_bits: MultiPrecision<Pow2.N16>.Bits
                            );
                        }
                        else {
                            x = NewtonRaphsonFinder<Pow2.N64>.RootFind(
                                x => (
                                    CDFPositiveSide<Pow2.N64>.Value(x, terms: 64) - p,
                                    -PDFPositiveSide<Pow2.N64>.Value(x, terms: 64)
                                ),
                                x0: 1 / p, overshoot_decay: true, iters: 256
                            );
                        }

                        MultiPrecision<Pow2.N64> v = MultiPrecision<Pow2.N64>.Log2(x) - u;

                        Console.WriteLine($"{u}\n{v:e20}");

                        sw.WriteLine($"{u},{v}");
                    }
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}