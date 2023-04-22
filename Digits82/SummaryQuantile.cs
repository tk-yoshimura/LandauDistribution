using MultiPrecision;

namespace Digits82 {
    class SummaryQuantile {
        static void Main() {
            List<MultiPrecision<N12>> ps = new();
            
            int exp0 = -1;
            for (int h = 8192; h >= 1; exp0 *= 2, h /= 2) {
                for (int exp = exp0; exp > exp0 * 2; exp--) {
                    for (double v = 1; v > 0.5; v -= 1d / h) {
                        ps.Add(MultiPrecision<N12>.Ldexp(v, exp));
                    }
                }
            }
            for (int exp = exp0; exp >= -16384; exp--) { 
                ps.Add(MultiPrecision<N12>.Ldexp(1, exp));
            }

            ps.Reverse();

            {
                using StreamWriter sw = new("../../../../results_disused/quantile_cdf_precision82_evalerr.csv");
                
                sw.WriteLine("cdf,lambda,error");
            
                foreach(MultiPrecision<N12> p in ps){
                    MultiPrecision<N12> lambda = Quantile.Value(p);
                    MultiPrecision<N12> q = CDF.Value(lambda);
                    MultiPrecision<N12> err = MultiPrecision<N12>.Abs(p - q) / p;
            
                    Console.WriteLine($"{p:e16},{lambda:e16},{err:e8}");
                    sw.WriteLine($"{p},{lambda},{err:e8}");
                    sw.Flush();
                }
            }

            //{
            //    using StreamWriter sw = new("../../../../results_disused/quantile_ccdf_precision82_evalerr.csv");
            //    
            //    sw.WriteLine("ccdf,lambda,error");
            //
            //    foreach(MultiPrecision<N12> p in ps){
            //        MultiPrecision<N12> lambda = Quantile.Value(p, is_complementary: true);
            //        MultiPrecision<N12> q = CDF.Value(lambda, is_complementary: true);
            //        MultiPrecision<N12> err = MultiPrecision<N12>.Abs(p - q) / p;
            //
            //        Console.WriteLine($"{p:e16},{lambda:e16},{err:e8}");
            //        sw.WriteLine($"{p},{lambda},{err:e8}");
            //        sw.Flush();
            //    }
            //}

            Console.WriteLine("END");
            Console.Read();
        }
    }
}