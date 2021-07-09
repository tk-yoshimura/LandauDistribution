using MultiPrecision;
using System;
using System.Collections.Generic;
using System.IO;

namespace LandauDistribution {
    class Program {
        static readonly string results_dir = "../../../../results/";

        static void Main(string[] args) {
            List<MultiPrecision<Pow2.N4>> xs = new() {
                //"-2.62916563729442102561",
                //"-2.44950010103172525499",
                //"-2.29064564961382595297",
                //"-2.10489790934939769338",
                //"-1.79957402870039992314",
                //"-1.49825415177780273396",
                //"-1.09225452805484635423",
                //"-0.77188422256801273027",
                //"-0.48277711307393456125",
                //"-0.20464065154575316905",
                //"0.073877362330763778128",
                //"0.361135283101730744896",
                //"0.664768389237266480219",
                "0.992995803490664167419",
                "1.35578042099080132503",
                "1.76629275098246523993",
                "2.24319477339787288381",
                "2.81468113996303570812",
                "3.52636966712139778598",
                "4.45839461019464834851",
                "5.76761029823977006164",
                "7.81274710480049833585",
                "11.649284684474405569996",
                "22.450278078872781782888",
                "43.202525183847707366482",
                "104.156361812207433543595",
                "204.862416150123003839536",
                "405.562095010331006820385",
                "1006.482330369225631256342",
            };

            using (StreamWriter sw = new StreamWriter(results_dir + "diff_pdf_n4_neg24_pos21_quantiles.csv", append: true)) {
                //sw.WriteLine("x,pdf'(x),error,accurate_bits");

                foreach (MultiPrecision<Pow2.N4> x in xs) {
                    MultiPrecision<Pow2.N4> y, error;
                    long accurate_bits;

                    if (x < 0) {
                        (y, error, accurate_bits) = DiffPDFNegativeSide<Pow2.N4>.Value(x, intergrate_iterations: 24);
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
