﻿using MultiPrecision;

namespace PositivePDF {
    class Program {
        static void Main() {
            (MultiPrecision<Pow2.N8> y, MultiPrecision<Pow2.N8> err, _) = LandauDistribution.PDFNegativeSide<Pow2.N8>.Value(-0.25);

            Console.WriteLine(y);
            Console.WriteLine(err);

            Console.WriteLine("END");
            Console.Read();
        }
    }
}