using MultiPrecision;

namespace LandauEvalCDFAsymptotic {
    class EvalPlusCDFLimit {
        static void Main() {
            using (StreamWriter sw = new("../../../../../results_disused/scaled_cdf_pluslimit_invert_precision162.csv")) {
                sw.WriteLine("# scale_cdf := (lambda >= 0) ? ( cdf * (lambda + pi) ) : ( cdf * sqrt(2 * pi * sigma) * exp(sigma) )");
                sw.WriteLine("# sigma := exp(-lambda-1)");

                sw.WriteLine("1/lambda,scaled_cdf");

                sw.WriteLine("0,1");

                for (int i = 1, n = 8192; i <= n; i++) {
                    MultiPrecision<N24> invlambda = MultiPrecision<N24>.Div(i, n * 2048);
                    MultiPrecision<N24> lambda = 1 / invlambda;
                    MultiPrecision<N24> cdf = CDFPositiveSide<N24>.Value(lambda, terms: 64);
                    MultiPrecision<N24> scaledcdf = cdf * (lambda + MultiPrecision<N24>.PI);

                    sw.WriteLine($"{invlambda},{scaledcdf}");
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}