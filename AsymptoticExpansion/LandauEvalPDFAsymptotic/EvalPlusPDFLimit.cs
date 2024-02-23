using MultiPrecision;

namespace LandauEvalPDFAsymptotic {
    class EvalPlusPDFLimit {
        static void Main() {
            using (StreamWriter sw = new("../../../../../results_disused/scaled_pdf_pluslimit_invert_precision162.csv")) {
                sw.WriteLine("# scale_pdf := (lambda >= 0) ? ( pdf * (lambda^2 + pi^2) ) : ( pdf * sqrt(2 pi) * exp(sigma) / sqrt(sigma) )");
                sw.WriteLine("# sigma := exp(-lambda-1)");

                sw.WriteLine("lambda,scaled_pdf");

                sw.WriteLine("0,1");

                for (int i = 1, n = 8192; i <= n; i++) {
                    MultiPrecision<N24> invlambda = MultiPrecision<N24>.Div(i, n * 2048);
                    MultiPrecision<N24> lambda = 1 / invlambda;
                    MultiPrecision<N24> pdf = PDFPositiveSide<N24>.Value(lambda, terms: 64);
                    MultiPrecision<N24> scaledpdf = pdf * (lambda * lambda + MultiPrecision<N24>.PI * MultiPrecision<N24>.PI);

                    sw.WriteLine($"{invlambda},{scaledpdf}");
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}