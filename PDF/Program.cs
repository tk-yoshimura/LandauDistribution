using MultiPrecision;

namespace PDF {
    class Program {
        static void Main() {
            List<MultiPrecision<Pow2.N8>> xs = new() { 
                0
            };

            for (int exp = -24; exp <= 24; exp++) {
                xs.Add(Math.ScaleB(1, exp));
            }

            using StreamWriter sw = new("../../../../results/integrate_pdf_precision64_test.csv");
            sw.WriteLine("method,lambda,pdf,error");

            foreach (MultiPrecision<Pow2.N8> x in xs) { 
                Console.WriteLine(x);

                (MultiPrecision<Pow2.N8> y, MultiPrecision<Pow2.N8> err) = NumericIntegration.PDFPositiveSide<Pow2.N8>.Value(x, 1e-64);
            
                Console.WriteLine(y);
                Console.WriteLine($"{err:e8}");

                sw.WriteLine($"positive,{x},{y},{err:e8}");
                sw.Flush();
            } 

            foreach (MultiPrecision<Pow2.N8> x in xs) {
                if (x > -16) {
                    break;
                }

                Console.WriteLine(-x);
                
                (MultiPrecision<Pow2.N8> y, MultiPrecision<Pow2.N8> err) = NumericIntegration.PDFNegativeSide<Pow2.N8>.Value(-x, 1e-64);
            
                Console.WriteLine(y);
                Console.WriteLine($"{err:e8}");

                sw.WriteLine($"negative,-{x},{y},{err:e8}");
                sw.Flush();
            } 

            Console.WriteLine("END");
            Console.Read();
        }
    }
}