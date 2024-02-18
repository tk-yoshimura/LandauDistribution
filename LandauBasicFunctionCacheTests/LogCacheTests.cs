using LandauBasicFunctionCache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiPrecision;

namespace LandauBasicFunctionCacheTests {
    [TestClass()]
    public class LogCacheTests {
        [TestMethod()]
        public void ApproxTest() {
            foreach (MultiPrecision<Pow2.N32> x in new double[] { 1, 1 / 1024d, 1 / 2048d, 1 / 4096d, 1 / 1048576d }) {

                MultiPrecision<Pow2.N32> expected = MultiPrecision<Pow2.N32>.Log1p(x);
                MultiPrecision<Pow2.N32> y32 = Log1pPadeApprox<Pow2.N32>.Log1pC32(x);
                MultiPrecision<Pow2.N32> y128 = Log1pPadeApprox<Pow2.N32>.Log1pC128(x);

                Console.WriteLine(x);
                Console.WriteLine(expected);
                Console.WriteLine(y32);
                Console.WriteLine(y128);

                Console.WriteLine($"c32 error:  {((expected - y32) / expected):e20}");
                Console.WriteLine($"c128 error: {((expected - y128) / expected):e20}");
            }
        }

        [TestMethod()]
        public void LogTest() {
            for (double xs = 0.125; xs <= 8; xs += 0.125) {
                MultiPrecision<Pow2.N16> x = xs;

                MultiPrecision<Pow2.N16> expected = MultiPrecision<Pow2.N16>.Log(x);
                MultiPrecision<Pow2.N16> actual = LogCache<Pow2.N16>.Log(x);

                Console.WriteLine(x);
                Console.WriteLine(expected);
                Console.WriteLine(actual);

                MultiPrecision<Pow2.N16> error = (expected - actual) / expected;

                if (expected == 0 && actual == 0) {
                    continue;
                }

                Assert.IsTrue(error.Exponent < -MultiPrecision<Pow2.N16>.Bits + 2, $"{error}");
            }

            for (double xs = 0.125 + 1 / 16384d; xs <= 8; xs += 0.125) {
                MultiPrecision<Pow2.N16> x = xs;

                MultiPrecision<Pow2.N16> expected = MultiPrecision<Pow2.N16>.Log(x);
                MultiPrecision<Pow2.N16> actual = LogCache<Pow2.N16>.Log(x);

                Console.WriteLine(x);
                Console.WriteLine(expected);
                Console.WriteLine(actual);

                MultiPrecision<Pow2.N16> error = (expected - actual) / expected;

                if (expected == 0 && actual == 0) {
                    continue;
                }

                Assert.IsTrue(error.Exponent < -MultiPrecision<Pow2.N16>.Bits + 8, $"{error}");
            }

            for (double xs = 0.125; xs <= 8; xs += 0.125) {
                if (xs == 1) {
                    continue;
                }

                MultiPrecision<Pow2.N16> x = MultiPrecision<Pow2.N16>.BitDecrement(xs);

                MultiPrecision<Pow2.N16> expected = MultiPrecision<Pow2.N16>.Log(x);
                MultiPrecision<Pow2.N16> actual = LogCache<Pow2.N16>.Log(x);

                Console.WriteLine(x);
                Console.WriteLine(expected);
                Console.WriteLine(actual);

                MultiPrecision<Pow2.N16> error = (expected - actual) / expected;

                Assert.IsTrue(error.Exponent < -MultiPrecision<Pow2.N16>.Bits + 4, $"{error}");
            }

            for (double xs = 0.125; xs <= 8; xs += 0.125) {
                MultiPrecision<Pow2.N16> x = xs + MultiPrecision<Pow2.N16>.E;

                MultiPrecision<Pow2.N16> expected = MultiPrecision<Pow2.N16>.Log(x);
                MultiPrecision<Pow2.N16> actual = LogCache<Pow2.N16>.Log(x);

                Console.WriteLine(x);
                Console.WriteLine(expected);
                Console.WriteLine(actual);

                MultiPrecision<Pow2.N16> error = (expected - actual) / expected;

                Assert.IsTrue(error.Exponent < -MultiPrecision<Pow2.N16>.Bits + 4, $"{error}");
            }

            for (double xs = 0.125; xs <= 8; xs += 0.125) {
                MultiPrecision<Pow2.N16> x = xs + MultiPrecision<Pow2.N16>.Sqrt2;

                MultiPrecision<Pow2.N16> expected = MultiPrecision<Pow2.N16>.Log(x);
                MultiPrecision<Pow2.N16> actual = LogCache<Pow2.N16>.Log(x);

                Console.WriteLine(x);
                Console.WriteLine(expected);
                Console.WriteLine(actual);

                MultiPrecision<Pow2.N16> error = (expected - actual) / expected;

                Assert.IsTrue(error.Exponent < -MultiPrecision<Pow2.N16>.Bits + 4, $"{error}");
            }
        }

        [TestMethod()]
        public void Log2Test() {
            for (double xs = 0.125; xs <= 8; xs += 0.125) {
                MultiPrecision<Pow2.N16> x = xs;

                MultiPrecision<Pow2.N16> expected = MultiPrecision<Pow2.N16>.Log2(x);
                MultiPrecision<Pow2.N16> actual = LogCache<Pow2.N16>.Log2(x);

                Console.WriteLine(x);
                Console.WriteLine(expected);
                Console.WriteLine(actual);

                MultiPrecision<Pow2.N16> error = (expected - actual) / expected;

                if (expected == 0 && actual == 0) {
                    continue;
                }

                Assert.IsTrue(error.Exponent < -MultiPrecision<Pow2.N16>.Bits + 2, $"{error}");
            }

            for (double xs = 0.125 + 1 / 16384d; xs <= 8; xs += 0.125) {
                MultiPrecision<Pow2.N16> x = xs;

                MultiPrecision<Pow2.N16> expected = MultiPrecision<Pow2.N16>.Log2(x);
                MultiPrecision<Pow2.N16> actual = LogCache<Pow2.N16>.Log2(x);

                Console.WriteLine(x);
                Console.WriteLine(expected);
                Console.WriteLine(actual);

                MultiPrecision<Pow2.N16> error = (expected - actual) / expected;

                if (expected == 0 && actual == 0) {
                    continue;
                }

                Assert.IsTrue(error.Exponent < -MultiPrecision<Pow2.N16>.Bits + 8, $"{error}");
            }

            for (double xs = 0.125; xs <= 8; xs += 0.125) {
                if (xs == 1) {
                    continue;
                }

                MultiPrecision<Pow2.N16> x = MultiPrecision<Pow2.N16>.BitDecrement(xs);

                MultiPrecision<Pow2.N16> expected = MultiPrecision<Pow2.N16>.Log2(x);
                MultiPrecision<Pow2.N16> actual = LogCache<Pow2.N16>.Log2(x);

                Console.WriteLine(x);
                Console.WriteLine(expected);
                Console.WriteLine(actual);

                MultiPrecision<Pow2.N16> error = (expected - actual) / expected;

                Assert.IsTrue(error.Exponent < -MultiPrecision<Pow2.N16>.Bits + 4, $"{error}");
            }

            for (double xs = 0.125; xs <= 8; xs += 0.125) {
                MultiPrecision<Pow2.N16> x = xs + MultiPrecision<Pow2.N16>.E;

                MultiPrecision<Pow2.N16> expected = MultiPrecision<Pow2.N16>.Log2(x);
                MultiPrecision<Pow2.N16> actual = LogCache<Pow2.N16>.Log2(x);

                Console.WriteLine(x);
                Console.WriteLine(expected);
                Console.WriteLine(actual);

                MultiPrecision<Pow2.N16> error = (expected - actual) / expected;

                Assert.IsTrue(error.Exponent < -MultiPrecision<Pow2.N16>.Bits + 4, $"{error}");
            }

            for (double xs = 0.125; xs <= 8; xs += 0.125) {
                MultiPrecision<Pow2.N16> x = xs + MultiPrecision<Pow2.N16>.Sqrt2;

                MultiPrecision<Pow2.N16> expected = MultiPrecision<Pow2.N16>.Log2(x);
                MultiPrecision<Pow2.N16> actual = LogCache<Pow2.N16>.Log2(x);

                Console.WriteLine(x);
                Console.WriteLine(expected);
                Console.WriteLine(actual);

                MultiPrecision<Pow2.N16> error = (expected - actual) / expected;

                Assert.IsTrue(error.Exponent < -MultiPrecision<Pow2.N16>.Bits + 4, $"{error}");
            }
        }
    }
}