using LandauEvalCDFAsymptotic;
using LandauEvalPDFAsymptotic;
using LandauPadeApprox;
using MultiPrecision;
using MultiPrecisionRootFinding;

namespace LandauEvalPadeApprox {
    internal class ExpectedQuantilePlusInvertN16 {
        static void Main_() {
            MultiPrecision<Pow2.N32> median = "1.3557804209908013250320928093907";

            using (StreamWriter sw = new("../../../../results_disused/quantile_upper_precision152_pluslimit_invert.csv")) {
                sw.WriteLine("p,v:=cquantile(p) * p");

                MultiPrecision<Pow2.N32> x = median;

                for (MultiPrecision<Pow2.N32> p = 0.5, h = 1d / 1048576; p > 0; p -= h) {
                    if (p.Exponent <= -8) {
                        x = 1 / p;
                    }

                    if (p.Exponent > -12) {
                        x = NewtonRaphsonFinder<Pow2.N32>.RootFind(
                            x => (
                                CDFPadeN16.Value(x.Convert<Pow2.N16>(), complementary: true).Convert<Pow2.N32>() - p,
                                -PDFPadeN16.Value(x.Convert<Pow2.N16>()).Convert<Pow2.N32>()
                            ),
                            x0: x, overshoot_decay: true, iters: 256, accurate_bits: MultiPrecision<Pow2.N16>.Bits
                        );
                    }
                    else {
                        x = NewtonRaphsonFinder<Pow2.N32>.RootFind(
                            x => (
                                CDFPositiveSide<Pow2.N32>.Value(x, terms: 64).Convert<Pow2.N32>() - p,
                                -PDFPositiveSide<Pow2.N32>.Value(x, terms: 64)
                            ),
                            x0: x, overshoot_decay: true, iters: 256
                        );
                    }

                    MultiPrecision<Pow2.N32> v = x * p;

                    Console.WriteLine($"{p}\n{v:e20}");

                    sw.WriteLine($"{p},{v}");
                }

                sw.WriteLine("0,1");
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
