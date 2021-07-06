using MultiPrecision;
using System;
using System.Collections.Generic;
using System.IO;

namespace LandauDistribution {
    class Program {
        static readonly string results_dir = "../../../../results_disused/";

        static void Main(string[] args) {
            List<MultiPrecision<Pow2.N4>> xs = new();

            using (StreamReader sr = new StreamReader(results_dir + "table.csv")) {
                while (!sr.EndOfStream) {
                    string line = sr.ReadLine();

                    if (string.IsNullOrWhiteSpace(line)) {
                        break;
                    }

                    string[] items = line.Split(',');

                    if (items.Length < 4) {
                        break;
                    }

                    string xstr = items[0];
                    MultiPrecision<Pow2.N4> x;

                    if (!xstr.Contains('^')) {
                        x = xstr;
                    }
                    else {
                        string[] x_split = xstr.Split('^');

                        if (x_split[0] != "2") {
                            throw new FormatException();
                        }

                        x = MultiPrecision<Pow2.N4>.Ldexp(1, int.Parse(x_split[1]));
                    }

                    xs.Add(x);
                }
            }


            using (StreamWriter sw = new StreamWriter(results_dir + "diff_pdf.csv")) {
                sw.WriteLine("x,pdf'(x),error,accurate_bits");

                foreach(MultiPrecision<Pow2.N4> x in xs) {
                    MultiPrecision<Pow2.N4> y, error;
                    long accurate_bits;

                    if (x < 0) {
                        (y, error, accurate_bits) = DiffPDFNegativeSide<Pow2.N4>.Value(x, intergrate_iterations: 24);
                    }
                    else { 
                        (y, error, accurate_bits) = DiffPDFPositiveSide<Pow2.N4>.Value(x, intergrate_iterations: 20);
                    }

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
