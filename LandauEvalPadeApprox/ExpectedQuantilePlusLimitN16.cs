using LandauEvalCDFAsymptotic;
using LandauEvalPDFAsymptotic;
using MultiPrecision;
using MultiPrecisionRootFinding;

namespace LandauEvalPadeApprox {
    internal class ExpectedQuantilePlusLimitN16 {
        static void Main_() {
            using (StreamWriter sw = new("../../../../results_disused/quantile_upper_precision152_limit_scaled.csv")) {
                sw.WriteLine("u:=-log2(p),v:=cquantile(p)*p");

                for (MultiPrecision<Pow2.N32> u0 = 256; u0 < 512; u0 *= 2) {
                    for (MultiPrecision<Pow2.N32> u = u0; u < u0 * 2 || u == 512; u += u0 / (u0 < 4 ? 32768 : 16384)) {
                        MultiPrecision<Pow2.N32> p = MultiPrecision<Pow2.N32>.Pow2(-u);

                        MultiPrecision<Pow2.N32> x = NewtonRaphsonFinder<Pow2.N32>.RootFind(
                            x => (
                                CDFPositiveSide<Pow2.N32>.Value(x, terms: 64) - p,
                                -PDFPositiveSide<Pow2.N32>.Value(x, terms: 64)
                            ),
                            x0: 1 / p, overshoot_decay: true, iters: 256, accurate_bits: 1016
                        );

                        MultiPrecision<Pow2.N32> v = x * p;

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