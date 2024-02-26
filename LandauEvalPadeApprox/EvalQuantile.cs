using LandauPadeApprox;
using MultiPrecision;

namespace LandauEvalPadeApprox {
    class EvalQuantile {
        static void Main_() {
            {
                List<MultiPrecision<Pow2.N16>> ps = [];

                int exp0 = -1;
                for (int h = 16384; h >= 1; exp0 *= 2, h /= 2) {
                    for (int exp = exp0; exp > exp0 * 2; exp--) {
                        for (double v = 1; v > 0.5; v -= 1d / h) {
                            ps.Add(MultiPrecision<Pow2.N16>.Ldexp(v, exp));
                        }
                    }
                }
                for (int exp = exp0; exp > -16384; exp--) {
                    ps.Add(MultiPrecision<Pow2.N16>.Ldexp(1, exp));
                }
                for (int exp = -16384; exp > -262144; exp -= 4) {
                    ps.Add(MultiPrecision<Pow2.N16>.Ldexp(1, exp));
                }
                for (int exp = -262144; exp > -4194304; exp -= 16) {
                    ps.Add(MultiPrecision<Pow2.N16>.Ldexp(1, exp));
                }
                for (int exp = -4194304; exp >= -16777216; exp -= 64) {
                    ps.Add(MultiPrecision<Pow2.N16>.Ldexp(1, exp));
                }

                ps.Reverse();

                using StreamWriter sw = new("../../../../results_disused/evalquantile_cdf_precision152.csv");

                sw.WriteLine("cdf,lambda,error");

                foreach (MultiPrecision<Pow2.N16> p in ps) {
                    MultiPrecision<Pow2.N16> lambda = QuantilePadeN16.Value(p);
                    MultiPrecision<Pow2.N16> q = CDFPadeN16.Value(lambda);
                    MultiPrecision<Pow2.N16> err = MultiPrecision<Pow2.N16>.Abs(p - q) / p;

                    Console.WriteLine($"{p:e16},{lambda:e16},{err:e8}");
                    sw.WriteLine($"{p},{lambda},{err:e8}");
                }

                sw.Flush();
            }

            {
                List<MultiPrecision<Pow2.N16>> ps = [];

                int exp0 = -1;
                for (int h = 16384; h >= 1; exp0 *= 2, h /= 2) {
                    for (int exp = exp0; exp > exp0 * 2 && exp >= -400; exp--) {
                        for (double v = 1; v > 0.5; v -= 1d / h) {
                            ps.Add(MultiPrecision<Pow2.N16>.Ldexp(v, exp));
                        }
                    }
                }

                ps.Reverse();

                using StreamWriter sw = new("../../../../results_disused/evalquantile_ccdf_precision152.csv");

                sw.WriteLine("ccdf,lambda,error");

                foreach (MultiPrecision<Pow2.N16> p in ps) {
                    MultiPrecision<Pow2.N16> lambda = QuantilePadeN16.Value(p, complementary: true);
                    MultiPrecision<Pow2.N16> q = CDFPadeN16.Value(lambda, complementary: true);
                    MultiPrecision<Pow2.N16> err = MultiPrecision<Pow2.N16>.Abs(p - q) / p;

                    Console.WriteLine($"{p:e16},{lambda:e16},{err:e8}");
                    sw.WriteLine($"{p},{lambda},{err:e8}");
                }

                sw.Flush();
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}