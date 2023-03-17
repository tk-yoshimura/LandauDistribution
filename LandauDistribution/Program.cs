using MultiPrecision;

namespace LandauDistribution {
    class Program {
        static readonly string results_dir = "../../../../results/";

        static void Main() {
            List<MultiPrecision<N6>> xs = new();

            for (double x = -8; x < -4; x += 1d / 32) {
                xs.Add(x);
            }
            for (double x = -4; x < -2; x += 1d / 64) {
                xs.Add(x);
            }
            for (double x = -2; x < -1; x += 1d / 128) {
                xs.Add(x);
            }
            for (double x = -1; x < 1; x += 1d / 256) {
                xs.Add(x);
            }
            for (double x = 1; x < 4; x += 1d / 128) {
                xs.Add(x);
            }
            for (double x = 4; x < 16; x += 1d / 64) {
                xs.Add(x);
            }
            for (double x = 16; x < 64; x += 1d / 32) {
                xs.Add(x);
            }
            for (double x = 64; x < 256; x += 1d / 16) {
                xs.Add(x);
            }
            for (double x = 256; x < 1024; x += 1d / 8) {
                xs.Add(x);
            }
            for (double x = 1024; x < 4096; x += 1d / 4) {
                xs.Add(x);
            }
            for (double x = 4096; x < Math.ScaleB(1, 16); x += 1d / 2) {
                xs.Add(x);
            }
            for (double x = Math.ScaleB(1, 16); x <= Math.ScaleB(1, 18); x++) {
                xs.Add(x);
            }

            using (StreamWriter sw = new(results_dir + "landau_pdf_precision40.csv")) {
                sw.WriteLine("x,pdf(x),error,accurate_bits,romberg_iter");

                int intergrate_iterations = 25;
                int needs_bit = (int)(40 / 0.30103) + 4;
                if (needs_bit + 32 > MultiPrecision<N6>.Bits) {
                    throw new Exception();
                }

                int i = 0;

                foreach (MultiPrecision<N6> x in xs) {
                    MultiPrecision<N6> y, error;
                    long accurate_bits;

                    Console.WriteLine(x);

                    if (x < 0) {
                        (y, error, accurate_bits) = PDFNegativeSide<N6>.Value(x, intergrate_iterations);
                    }
                    else {
                        (y, error, accurate_bits) = PDFPositiveSide<N6>.Value(x, intergrate_iterations);
                    }

                    Console.Write(".");

                    while(intergrate_iterations < needs_bit){
                        if (x < 0) {
                            (y, error, accurate_bits) = PDFNegativeSide<N6>.Value(x, intergrate_iterations);
                        }
                        else {
                            (y, error, accurate_bits) = PDFPositiveSide<N6>.Value(x, intergrate_iterations);
                        }

                        if (accurate_bits < needs_bit) {
                            intergrate_iterations += 2;
                        }

                        Console.Write(".");
                    }

                    Console.Write("\n");
                    Console.WriteLine(y);
                    Console.WriteLine(error);
                    Console.WriteLine(accurate_bits);
                    Console.WriteLine(intergrate_iterations);

                    sw.WriteLine($"{x},{y:40},{error:e8},{accurate_bits},{intergrate_iterations}");
                    sw.Flush();

                    if ((i % 8) == 0) {
                        intergrate_iterations--;
                    }

                    i++;
                }
            }

            Console.WriteLine("END");
            Console.ReadLine();
        }
    }
}
