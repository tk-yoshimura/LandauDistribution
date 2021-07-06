using MultiPrecision;
using System;

namespace LandauDistribution {
    class Program {
        static void Main(string[] args) {

            {
                (MultiPrecision<Pow2.N4> y, MultiPrecision<Pow2.N4> error, long accurate_bits) = PDFNegativeSide<Pow2.N4>.Value(-1);

                Console.WriteLine(y);
                Console.WriteLine(error);
                Console.WriteLine(accurate_bits);
            }

            {
                (MultiPrecision<Pow2.N4> y, MultiPrecision<Pow2.N4> error, long accurate_bits) = PDFPositiveSide<Pow2.N4>.Value(4);

                Console.WriteLine(y);
                Console.WriteLine(error);
                Console.WriteLine(accurate_bits);
            }

            {
                (MultiPrecision<Pow2.N4> y, MultiPrecision<Pow2.N4> error, long accurate_bits) = PDFPositiveSide<Pow2.N4>.Value(100);

                Console.WriteLine(y);
                Console.WriteLine(error);
                Console.WriteLine(accurate_bits);
            }

            Console.WriteLine("END");
            Console.ReadLine();
        }
    }
}
