using MultiPrecision;

namespace PDF {
    class Program {
        static void Main() {
            List<double> xs = new();

            for (double x = 0; x < 1; x += 1d / 64) {
                xs.Add(x);
            }
            for (double h = 1d / 32, x0 = 1, x1 = 2; x1 <= 4096; h *= 2, x0 *= 2, x1 *= 2) {
                for (double x = x0; x < x1; x += h) {
                    xs.Add(x);
                }
            }
            for (double x = 4096; x <= Math.ScaleB(1, 50); x *= 2) {
                xs.Add(x);
            }

            for (double x = 1d / 64; x < 1; x += 1d / 64) {
                xs.Add(-x);
            }
            for (double h = 1d / 32, x0 = 1, x1 = 2; x1 <= 16; h *= 2, x0 *= 2, x1 *= 2) {
                for (double x = x0; x < x1; x += h) {
                    xs.Add(-x);
                }
            }
            for (double x = 16; x <= 20; x += 1d / 4) {
                xs.Add(-x);
            }

            xs.Sort();

            using StreamWriter sw = new("../../../../results/integrate_pdf_precision64.csv");
            sw.WriteLine("lambda,pdf,error");

            foreach (MultiPrecision<Pow2.N8> x in xs) { 
                Console.WriteLine(x);

                (MultiPrecision<Pow2.N8> y, MultiPrecision<Pow2.N8> err) = NumericIntegration.PDF<Pow2.N8>.Value(x, 1e-64);
            
                Console.WriteLine($"{y:e64}");
                Console.WriteLine($"{err:e8}");

                sw.WriteLine($"{x},{y:e64},{err:e8}");
                sw.Flush();
            } 

            Console.WriteLine("END");
            Console.Read();
        }
    }
}