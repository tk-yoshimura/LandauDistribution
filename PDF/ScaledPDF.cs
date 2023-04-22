using MultiPrecision;

namespace PDF {
    class ScaledPDF {
        static void Main() {
            List<MultiPrecision<N12>> xs = new();

            for (MultiPrecision<N12> x = 0; x < 1; x += 1d / 2048) {
                xs.Add(x);
            }
            for (MultiPrecision<N12> h = 1d / 1024, x0 = 1, x1 = 2; x1 <= 8192; h *= 2, x0 *= 2, x1 *= 2) {
                for (MultiPrecision<N12> x = x0; x < x1; x += h) {
                    xs.Add(x);
                }
            }
            xs.Add(8192);

            for (MultiPrecision<N12> x = 0; x < 1; x += 1d / 2048) {
                xs.Add(-x);
            }
            for (MultiPrecision<N12> h = 1d / 1024, x0 = 1, x1 = 2; x1 <= 16; h *= 2, x0 *= 2, x1 *= 2) {
                for (MultiPrecision<N12> x = x0; x < x1; x += h) {
                    xs.Add(-x);
                }
            }
            for (MultiPrecision<N12> x = 16; x <= 20; x += 1d / 128) {
                xs.Add(-x);
            }
            
            xs.Sort();

            xs.Insert(0, MultiPrecision<N12>.MinusZero);
            xs.Insert(0, MultiPrecision<N12>.Zero);

            using StreamWriter sw = new("../../../../results_disused/integrate_scaled_pdf_precision100.csv");
            sw.WriteLine("# scale_pdf := (lambda >= 0) ? ( pdf * (lambda^2 + pi^2) ) : ( pdf * sqrt(2 pi) * exp(sigma) / sqrt(sigma) )");
            sw.WriteLine("# sigma := exp(-lambda-1)");

            sw.WriteLine("lambda,scaled_pdf,error");

            foreach (MultiPrecision<N12> x in xs) {
                Console.WriteLine(x);

                if (x.Sign == Sign.Plus) {
                    (MultiPrecision<N12> y, MultiPrecision<N12> err) = NumericIntegration.PDFPositiveSide<N12>.Value(x, 1e-100);
                    MultiPrecision<N12> s = x * x + MultiPrecision<N12>.PI * MultiPrecision<N12>.PI;

                    y *= s;
                    err *= s;

                    Console.WriteLine($"{y:e64}");
                    Console.WriteLine($"{err:e8}");

                    sw.WriteLine($"{x},{y},{err:e8}");
                    sw.Flush();
                }
                else {
                    (MultiPrecision<N12> y, MultiPrecision<N12> err) = NumericIntegration.PDFNegativeSide<N12>.ScaledValue(x, 1e-100);

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