using MultiPrecision;

namespace LandauNegativeSideRoot {
    class Program {
        static void Main() {
            for (decimal x = -2.1447298858494000m; x >= -2.1447298858494010m; x -= 1e-16m) {
                List<(MultiPrecision<Pow2.N4> t, int n)> ts = LandauNegativeSideRootMP<Pow2.N4>.EnumHalfPIValues((MultiPrecision<Pow2.N4>)x).ToList();

                Console.WriteLine(x);

                foreach ((MultiPrecision<Pow2.N4> t, int n) in ts) {
                    MultiPrecision<Pow2.N4> v = LandauNegativeSideRootMP<Pow2.N4>.Func((MultiPrecision<Pow2.N4>)x, t).v;

                    Console.WriteLine($"{t}\t{(v / MultiPrecision<Pow2.N4>.PI)}");
                }
            }

            for (decimal x = -2.144m; x >= -2.145m; x -= 1e-5m) {
                List<(MultiPrecision<Pow2.N4> t, int n)> ts = LandauNegativeSideRootMP<Pow2.N4>.EnumHalfPIValues((MultiPrecision<Pow2.N4>)x).ToList();

                Console.WriteLine(x);

                foreach ((MultiPrecision<Pow2.N4> t, int n) in ts) {
                    MultiPrecision<Pow2.N4> v = LandauNegativeSideRootMP<Pow2.N4>.Func((MultiPrecision<Pow2.N4>)x, t).v;

                    Console.WriteLine($"{t}\t{(v / MultiPrecision<Pow2.N4>.PI)}");
                }
            }

            for (decimal x = -2.0m; x >= -12.0m; x -= 0.01m) {
                List<(MultiPrecision<Pow2.N4> t, int n)> ts = LandauNegativeSideRootMP<Pow2.N4>.EnumHalfPIValues((MultiPrecision<Pow2.N4>)x).ToList();

                Console.WriteLine(x);

                foreach ((MultiPrecision<Pow2.N4> t, int n) in ts) {
                    MultiPrecision<Pow2.N4> v = LandauNegativeSideRootMP<Pow2.N4>.Func((MultiPrecision<Pow2.N4>)x, t).v;

                    Console.WriteLine($"{t}\t{(v / MultiPrecision<Pow2.N4>.PI)}");
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}