using MultiPrecision;
using System;
using System.Collections.Generic;
using System.IO;

namespace LandauDistribution {
    class Program {
        static readonly string results_dir = "../../../../results/";

        static void Main(string[] args) {
            //List<MultiPrecision<Pow2.N4>> xs = new();

            //for (decimal x = -0.125m; x >= -0.3125m; x -= 1 / 32m) {
            //    xs.Add(x);
            //}

            //for (decimal x = -3; x >= -8; x -= 1 / 8m) {
            //    xs.Add(x);
            //}

            //for (int i = 14; i <= 50; i++) {
            //    xs.Add(MultiPrecision<Pow2.N4>.Ldexp(1, i));
            //}

            //using (StreamWriter sw = new StreamWriter(results_dir + "diff_pdf_n4_neg24_pos20_r2.csv")) {
            //    sw.WriteLine("x,pdf'(x),error,accurate_bits");

            //    foreach(MultiPrecision<Pow2.N4> x in xs) {
            //        MultiPrecision<Pow2.N4> y, error;
            //        long accurate_bits;

            //        if (x < 0) {
            //            (y, error, accurate_bits) = DiffPDFNegativeSide<Pow2.N4>.Value(x, intergrate_iterations: 24);
            //        }
            //        else { 
            //            (y, error, accurate_bits) = DiffPDFPositiveSide<Pow2.N4>.Value(x, intergrate_iterations: 20);
            //        }

            //        Console.WriteLine(y);
            //        Console.WriteLine(error);
            //        Console.WriteLine(accurate_bits);

            //        sw.WriteLine($"{x},{y},{error:E8},{accurate_bits}");
            //        sw.Flush();
            //    }
            //}

            for (decimal x = 0.1m; x <= 8m; x += 0.1m) {
                MultiPrecision<Pow2.N4> y1 = MultiPrecision<Pow2.N4>.Log(x);
                MultiPrecision<Pow2.N4> y2 = LogCache<Pow2.N4>.Value(x);
                MultiPrecision<Pow2.N4> err = y1 - y2;

                Console.WriteLine(y1.ToHexcode());
                Console.WriteLine(y2.ToHexcode());

                Console.WriteLine(err);

                Console.WriteLine("");
            }

            Console.WriteLine("END");
            Console.ReadLine();
        }
    }
}
