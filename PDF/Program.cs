using MultiPrecision;

namespace PDF {
    class Program {
        static void Main() {
            (MultiPrecision<Pow2.N8> y, MultiPrecision<Pow2.N8> err) = NumericIntegration.PDFNegativeSide<Pow2.N8>.ScaledValue(-0.25, 1e-40);

            Console.WriteLine(y);
            Console.WriteLine(err);

            Console.WriteLine("END");
            Console.Read();
        }
    }
}