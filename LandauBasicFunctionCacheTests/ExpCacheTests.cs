using MultiPrecision;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using LandauBasicFunctionCache;

namespace LandauBasicFunctionCacheTests {
    [TestClass()]
    public class ExpCacheTests {
        [TestMethod()]
        public void Pow2Test() {
            for (double xs = -8 + Math.ScaleB(1, -31); xs <= 8; xs += 0.125) {
                MultiPrecision<Pow2.N16> x = xs;

                MultiPrecision<Pow2.N16> expected = MultiPrecision<Pow2.N16>.Pow2(x);
                MultiPrecision<Pow2.N16> actual = ExpCache<Pow2.N16>.Pow2(x);

                Console.WriteLine(x);
                Console.WriteLine(expected);
                Console.WriteLine(actual);

                MultiPrecision<Pow2.N16> error = (expected - actual) / expected;

                Assert.IsTrue(error.Exponent < -MultiPrecision<Pow2.N16>.Bits + 2, $"{error}");
            }

            for (double xs = -8; xs <= 8; xs += 0.125) {
                MultiPrecision<Pow2.N16> x = xs;

                MultiPrecision<Pow2.N16> expected = MultiPrecision<Pow2.N16>.Pow2(x);
                MultiPrecision<Pow2.N16> actual = ExpCache<Pow2.N16>.Pow2(x);

                Console.WriteLine(x);
                Console.WriteLine(expected);
                Console.WriteLine(actual);

                MultiPrecision<Pow2.N16> error = (expected - actual) / expected;

                Assert.IsTrue(error.Exponent < -MultiPrecision<Pow2.N16>.Bits + 2, $"{error}");
            }

            for (double xs = -8; xs <= 8; xs += 0.125) {
                MultiPrecision<Pow2.N16> x = xs + MultiPrecision<Pow2.N16>.E;

                MultiPrecision<Pow2.N16> expected = MultiPrecision<Pow2.N16>.Pow2(x);
                MultiPrecision<Pow2.N16> actual = ExpCache<Pow2.N16>.Pow2(x);

                Console.WriteLine(x);
                Console.WriteLine(expected);
                Console.WriteLine(actual);

                MultiPrecision<Pow2.N16> error = (expected - actual) / expected;

                Assert.IsTrue(error.Exponent < -MultiPrecision<Pow2.N16>.Bits + 3, $"{error}");
            }
        }

        [TestMethod()]
        public void ExpTest() {
            for (double xs = -8; xs <= 8; xs += 0.125) {
                MultiPrecision<Pow2.N16> x = xs;

                MultiPrecision<Pow2.N16> expected = MultiPrecision<Pow2.N16>.Exp(x);
                MultiPrecision<Pow2.N16> actual = ExpCache<Pow2.N16>.Exp(x);

                Console.WriteLine(x);
                Console.WriteLine(expected);
                Console.WriteLine(actual);

                MultiPrecision<Pow2.N16> error = (expected - actual) / expected;

                Assert.IsTrue(error.Exponent < -MultiPrecision<Pow2.N16>.Bits + 4, $"{error}");
            }

            for (double xs = -8; xs <= 8; xs += 0.125) {
                MultiPrecision<Pow2.N16> x = xs + MultiPrecision<Pow2.N16>.E;

                MultiPrecision<Pow2.N16> expected = MultiPrecision<Pow2.N16>.Exp(x);
                MultiPrecision<Pow2.N16> actual = ExpCache<Pow2.N16>.Exp(x);

                Console.WriteLine(x);
                Console.WriteLine(expected);
                Console.WriteLine(actual);

                MultiPrecision<Pow2.N16> error = (expected - actual) / expected;

                Assert.IsTrue(error.Exponent < -MultiPrecision<Pow2.N16>.Bits + 4, $"{error}");
            }
        }
    }
}