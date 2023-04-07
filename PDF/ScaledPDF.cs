using MultiPrecision;

namespace PDF {
    class ScaledPDF {
        static void Main() {
            List<MultiPrecision<Pow2.N8>> xs = new();

            for (MultiPrecision<Pow2.N8> x = 0; x < 1; x += 1d / 512) {
                xs.Add(x);
            }
            for (MultiPrecision<Pow2.N8> h = 1d / 256, x0 = 1, x1 = 2; x1 <= 8192; h *= 2, x0 *= 2, x1 *= 2) {
                for (MultiPrecision<Pow2.N8> x = x0; x < x1; x += h) {
                    xs.Add(x);
                }
            }
            xs.Add(8192);

            for (MultiPrecision<Pow2.N8> x = 0; x < 1; x += 1d / 512) {
                xs.Add(-x);
            }
            for (MultiPrecision<Pow2.N8> h = 1d / 256, x0 = 1, x1 = 2; x1 <= 16; h *= 2, x0 *= 2, x1 *= 2) {
                for (MultiPrecision<Pow2.N8> x = x0; x < x1; x += h) {
                    xs.Add(-x);
                }
            }
            for (MultiPrecision<Pow2.N8> x = 16; x <= 20; x += 1d / 32) {
                xs.Add(-x);
            }

            xs.Sort();

            using StreamWriter sw = new("../../../../results_disused/integrate_scaled_pdf_precision68.csv", append: true);
            sw.WriteLine("# scale_pdf := (lambda >= 0) ? ( pdf * (lambda^2 + pi^2) ) : ( pdf * sqrt(2 pi) * exp(sigma) / sqrt(sigma) )");
            sw.WriteLine("# sigma := exp(-lambda-1)");
            
            sw.WriteLine("lambda,scaled_pdf,error");

            foreach (MultiPrecision<Pow2.N8> x in xs) { 
                Console.WriteLine(x);

                if (x.Sign == Sign.Plus) {
                    (MultiPrecision<Pow2.N8> y, MultiPrecision<Pow2.N8> err) = NumericIntegration.PDFPositiveSide<Pow2.N8>.Value(x, 1e-68);
                    MultiPrecision<Pow2.N8> s = x * x + MultiPrecision<Pow2.N8>.PI * MultiPrecision<Pow2.N8>.PI;

                    y *= s;
                    err *= s;

                    Console.WriteLine($"{y:e64}");
                    Console.WriteLine($"{err:e8}");

                    sw.WriteLine($"{x},{y},{err:e8}");
                    sw.Flush();
                }
                else {
                    (MultiPrecision<Pow2.N8> y, MultiPrecision<Pow2.N8> err) = NumericIntegration.PDFNegativeSide<Pow2.N8>.ScaledValue(x, 1e-68);

                    Console.WriteLine($"{y:e64}");
                    Console.WriteLine($"{err:e8}");

                    sw.WriteLine($"{x},{y},{err:e8}");
                    sw.Flush();
                }
            } 

            Console.WriteLine("END");
            Console.Read();
        }
    }
}