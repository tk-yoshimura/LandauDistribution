using MultiPrecision;

namespace LandauEvalPDFAsymptotic {
    class EvalPDFNegativeCoefs {
        static void Main_() {
            using (StreamWriter sw = new("../../../../../results_disused/pdf_minuscoef_precision155.txt")) {
                for (int i = 0; i < 64; i++) {
                    sw.WriteLine($"{PDFNegativeSide<Pow2.N32>.Coefs[i]:e155}");
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}