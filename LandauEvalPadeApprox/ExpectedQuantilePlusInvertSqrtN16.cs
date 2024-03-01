using LandauEvalCDFAsymptotic;
using LandauEvalPDFAsymptotic;
using LandauPadeApprox;
using MultiPrecision;
using MultiPrecisionRootFinding;

namespace LandauEvalPadeApprox {
    internal class ExpectedQuantilePlusInvertSqrtN16 {
        static void Main_() {
            const int n = 512;

            using (StreamWriter sw = new("../../../../results_disused/quantile_upper_precision152_pluslimit_invertsqrt.csv")) {
                sw.WriteLine("u,p=u^2,v:=cquantile(p) * p");

                for (int exponent = -2; exponent >= -100; exponent--) {
                    MultiPrecision<Pow2.N64> u0 = MultiPrecision<Pow2.N64>.Ldexp(1, exponent);
                    MultiPrecision<Pow2.N64> u1 = MultiPrecision<Pow2.N64>.Ldexp(1, exponent - 1);
                    MultiPrecision<Pow2.N64> h = (u0 - u1) / n;
                    
                    for (MultiPrecision<Pow2.N64> u = u0; u > u1; u -= h) {
                        MultiPrecision<Pow2.N64> p = u * u;
                        MultiPrecision<Pow2.N64> x = 1 / p;

                        if (p.Exponent > -12) {
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
                                    CDFPositiveSide<Pow2.N64>.Value(x, terms: 64).Convert<Pow2.N64>() - p,
                                    -PDFPositiveSide<Pow2.N64>.Value(x, terms: 64)
                                ),
                                x0: x, overshoot_decay: true, iters: 256
                            );
                        }

                        MultiPrecision<N80> v = (x * p).Convert<N80>();

                        Console.WriteLine($"{u}\n{p}\n{v:e20}");

                        sw.WriteLine($"{u},{p},{v}");
                    }
                }

                sw.WriteLine("0,0,1");
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
