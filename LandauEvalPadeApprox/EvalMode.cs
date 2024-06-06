using LandauPadeApprox;
using MultiPrecision;
using MultiPrecisionDifferentiate;
using MultiPrecisionRootFinding;

namespace LandauEvalPadeApprox {
    internal class SearchMode {
        static void Main_() {
            using StreamWriter sw = new("../../../../results/mode_precision75.csv");

            sw.WriteLine("stats,x,pdf");

            MultiPrecision<Pow2.N16> x = SecantFinder<Pow2.N16>.RootFind(
                x => CenteredIntwayDifferential<Pow2.N16>.Differentiate(PDFPadeN16.Value, x, 1, "1e-10"),
                -0.222, iters: 1024, overshoot_decay: true, divergence_decay: true
            );

            MultiPrecision<Pow2.N16> eps = "1e-75";

            MultiPrecision<Pow2.N16> y = PDFPadeN16.Value(x);
            MultiPrecision<Pow2.N16> y_meps = PDFPadeN16.Value(x - eps);
            MultiPrecision<Pow2.N16> y_peps = PDFPadeN16.Value(x + eps);

            sw.WriteLine($"mode,{x},{y}");
            sw.WriteLine($"mode-eps,{x - eps},{y_meps}");
            sw.WriteLine($"mode+eps,{x + eps},{y_peps}");

            sw.WriteLine($"check : {y > y_meps && y > y_peps}");
            Console.WriteLine($"check : {y > y_meps && y > y_peps}");

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
