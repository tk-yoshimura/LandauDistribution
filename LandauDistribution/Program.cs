using MultiPrecision;
using System;
using System.Collections.Generic;
using System.IO;

namespace LandauDistribution {
    class Program {
        static readonly string results_dir = "../../../../results/";

        static void Main(string[] args) {
            List<MultiPrecision<Pow2.N8>> xs = new();

            for (decimal x = -3.5m; x >= -4m; x -= 1 / 8m) {
                xs.Add(x);
            }

            for (decimal x = -4.25m; x >= -8m; x -= 1 / 4m) {
                xs.Add(x);
            }

            using (StreamWriter sw = new StreamWriter(results_dir + "diff_pdf_n8_neg26_pos20.csv")) {
                sw.WriteLine("x,pdf'(x),error,accurate_bits");

                foreach (MultiPrecision<Pow2.N8> x in xs) {
                    MultiPrecision<Pow2.N8> y, error;
                    long accurate_bits;

                    (y, error, accurate_bits) = DiffPDFNegativeSide<Pow2.N8>.Value(x, intergrate_iterations: 26);

                    Console.WriteLine(y);
                    Console.WriteLine(error);
                    Console.WriteLine(accurate_bits);

                    sw.WriteLine($"{x},{y},{error:E8},{accurate_bits}");
                    sw.Flush();
                }
            }

            Console.WriteLine("END");
            Console.ReadLine();
        }
    }
}
