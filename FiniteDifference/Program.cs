using MultiPrecision;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FiniteDifference {
    class Program {
        static readonly string results_dir = "../../../../results/";

        static void Main(string[] args) {
            MultiPrecision<Pow2.N8> h = MultiPrecision<Pow2.N8>.Ldexp(1, -36);

            List<(decimal x, MultiPrecision<Pow2.N8>[] ps)> items = ReadTxt();

            using (StreamWriter sw = new(results_dir + "diff_pdf_wolfram_neighbor_finite_diff.txt")) {
                foreach ((decimal x, MultiPrecision<Pow2.N8>[] ps) in items) {
                    if (ps[0] >= ps[1] || ps[1] >= ps[2] || ps[2] >= ps[3] || ps[3] >= ps[4]) {
                        throw new FormatException();
                    }

                    MultiPrecision<Pow2.N8> g = ((ps[3] - ps[1]) * 2 / 3 - (ps[4] - ps[0]) / 12) / h;

                    sw.WriteLine($"{x},{g:e30}");
                }
            }
        }

        static List<(decimal x, MultiPrecision<Pow2.N8>[] ps)> ReadTxt() {
            List<(decimal x, MultiPrecision<Pow2.N8>[])> table = new();

            using (StreamReader sr = new(results_dir + "diff_pdf_wolfram_neighbor.txt")) {
                // skip header 
                sr.ReadLine();
                sr.ReadLine();

                while (!sr.EndOfStream) {
                    string[] xstr = sr.ReadLine().Split('/');
                    decimal x = xstr.Length == 1 ? decimal.Parse(xstr[0]) : (decimal.Parse(xstr[0]) / decimal.Parse(xstr[1]));

                    MultiPrecision<Pow2.N8>[] ps = sr.ReadLine()[1..^1].Split(',').Select(str => (MultiPrecision<Pow2.N8>)str.Trim()).ToArray();

                    table.Add((x, ps));
                }
            }

            return table;
        }
    }
}
