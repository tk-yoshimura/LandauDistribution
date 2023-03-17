namespace LandauNegativeSideRoot {
    class Program {
        static void Main() {
            for (decimal x = -2.1447298858494000m; x >= -2.1447298858494010m; x -= 1e-16m) {
                List<double> ts = LandauNegativeSideInverse.EnumHalfPIValues((double)x).ToList();

                Console.WriteLine(x);

                foreach (double t in ts) {
                    double v = LandauNegativeSideInverse.Func((double)x, t).v;

                    Console.WriteLine($"{t}\t{(v / Math.PI):e15}");
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}