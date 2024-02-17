using MultiPrecision;

namespace PDF {
    class ScaledPDF {
        static void Main() {
            List<MultiPrecision<N18>> xs = [];

            for (MultiPrecision<N18> x = 0; x < 1; x += 1d / 2048) {
                xs.Add(x);
            }
            for (MultiPrecision<N18> h = 1d / 1024, x0 = 1, x1 = 2; x1 <= 8192; h *= 2, x0 *= 2, x1 *= 2) {
                for (MultiPrecision<N18> x = x0; x < x1; x += h) {
                    xs.Add(x);
                }
            }
            xs.Add(8192);

            for (MultiPrecision<N18> x = 0; x < 1; x += 1d / 2048) {
                xs.Add(-x);
            }
            for (MultiPrecision<N18> h = 1d / 1024, x0 = 1, x1 = 2; x1 <= 16; h *= 2, x0 *= 2, x1 *= 2) {
                for (MultiPrecision<N18> x = x0; x < x1; x += h) {
                    xs.Add(-x);
                }
            }
            for (MultiPrecision<N18> x = 16; x <= 20; x += 1d / 128) {
                xs.Add(-x);
            }

            xs.Sort();

            xs.Insert(0, MultiPrecision<N18>.MinusZero);
            xs.Insert(0, MultiPrecision<N18>.Zero);

            using StreamWriter sw = new("../../../../results_disused/integrate_scaled_pdf_precision150.csv");
            sw.WriteLine("# scale_pdf := (lambda >= 0) ? ( pdf * (lambda^2 + pi^2) ) : ( pdf * sqrt(2 pi) * exp(sigma) / sqrt(sigma) )");
            sw.WriteLine("# sigma := exp(-lambda-1)");

            sw.WriteLine("lambda,scaled_pdf,error");

            foreach (MultiPrecision<N18> x in xs) {
                Console.WriteLine(x);

                (MultiPrecision<N18> y, MultiPrecision<N18> err) = NumericIntegration.ScaledPDF<N18>.Value(x, eps: 1e-150);
                    
                Console.WriteLine($"{y:e64}");
                Console.WriteLine($"{err:e8}");

                sw.WriteLine($"{x},{y},{err:e8}");
                sw.Flush();
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}