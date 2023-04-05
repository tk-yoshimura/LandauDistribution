using MultiPrecision;

namespace PDF {
    class Program {
        static void Main() {
            {
                (MultiPrecision<Pow2.N8> y, MultiPrecision<Pow2.N8> err) = NumericIntegration.PDFPositiveSide<Pow2.N8>.Value(0, 1e-64);

                Console.WriteLine(y);
                Console.WriteLine(err);
            }
            {
                (MultiPrecision<Pow2.N8> y, MultiPrecision<Pow2.N8> err) = NumericIntegration.PDFNegativeSide<Pow2.N8>.Value(0, 1e-64);

                Console.WriteLine(y);
                Console.WriteLine(err);
            }

            {
                (MultiPrecision<Pow2.N8> y, MultiPrecision<Pow2.N8> err) = NumericIntegration.PDFPositiveSide<Pow2.N8>.Value(32, 1e-64);

                Console.WriteLine(y);
                Console.WriteLine(err);
            }
            {
                (MultiPrecision<Pow2.N8> y, MultiPrecision<Pow2.N8> err) = NumericIntegration.PDFPositiveSide<Pow2.N8>.Value(1024, 1e-64);

                Console.WriteLine(y);
                Console.WriteLine(err);
            }
            {
                (MultiPrecision<Pow2.N8> y, MultiPrecision<Pow2.N8> err) = NumericIntegration.PDFPositiveSide<Pow2.N8>.Value(1048576, 1e-64);

                Console.WriteLine(y);
                Console.WriteLine(err);
            }
            {
                (MultiPrecision<Pow2.N8> y, MultiPrecision<Pow2.N8> err) = NumericIntegration.PDFNegativeSide<Pow2.N8>.Value(-0.25, 1e-64);

                Console.WriteLine(y);
                Console.WriteLine(err);
            }
            {
                (MultiPrecision<Pow2.N8> y, MultiPrecision<Pow2.N8> err) = NumericIntegration.PDFNegativeSide<Pow2.N8>.Value(-0.5, 1e-64);

                Console.WriteLine(y);
                Console.WriteLine(err);
            }
            {
                (MultiPrecision<Pow2.N8> y, MultiPrecision<Pow2.N8> err) = NumericIntegration.PDFNegativeSide<Pow2.N8>.Value(-1, 1e-64);

                Console.WriteLine(y);
                Console.WriteLine(err);
            }
            {
                (MultiPrecision<Pow2.N8> y, MultiPrecision<Pow2.N8> err) = NumericIntegration.PDFNegativeSide<Pow2.N8>.Value(-2, 1e-64);

                Console.WriteLine(y);
                Console.WriteLine(err);
            }
            {
                (MultiPrecision<Pow2.N8> y, MultiPrecision<Pow2.N8> err) = NumericIntegration.PDFNegativeSide<Pow2.N8>.Value(-4, 1e-64);

                Console.WriteLine(y);
                Console.WriteLine(err);
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}