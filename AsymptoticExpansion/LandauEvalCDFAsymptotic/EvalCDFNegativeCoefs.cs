using MultiPrecision;

namespace LandauEvalCDFAsymptotic {
    class EvalCDFNegativeCoefs {
        static void Main() {
            using (StreamWriter sw = new("../../../../../results_disused/cdf_minuscoef_precision155.txt")) {
                for (int i = 0; i < 64; i++) {
                    sw.WriteLine($"{CDFNegativeSide<Pow2.N32>.Coefs[i]:e155}");
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}