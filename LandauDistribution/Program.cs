using MultiPrecision;

namespace LandauDistribution {
    class Program {
        static readonly string results_dir = "../../../../results/";

        static void Main() {
            List<MultiPrecision<Pow2.N4>> xs = new() {
                -2.875m,
                -2.75m,
                -2.625m,
                -0.84375m,
                -0.8125m,
                -0.78125m,
                -0.75m,
                -0.71875m,
                -0.6875m,
                -0.65625m,
                -0.625m,
                -0.59375m,
                -0.5625m,
                -0.53125m,
                -0.5m,
                -0.46875m,
                -0.4375m,
                -0.40625m,
                -0.375m,
                -0.34375m,
                -0.09375m,
                -0.0625m,
                -0.03125m,
            };

            using (StreamWriter sw = new(results_dir + "diff_pdf_n4_neg25.csv")) {
                sw.WriteLine("x,pdf'(x),error,accurate_bits");

                foreach (MultiPrecision<Pow2.N4> x in xs) {
                    MultiPrecision<Pow2.N4> y, error;
                    long accurate_bits;

                    if (x < 0) {
                        (y, error, accurate_bits) = DiffPDFNegativeSide<Pow2.N4>.Value(x, intergrate_iterations: 25);
                    }
                    else {
                        (y, error, accurate_bits) = DiffPDFPositiveSide<Pow2.N4>.Value(x, intergrate_iterations: 21);
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
