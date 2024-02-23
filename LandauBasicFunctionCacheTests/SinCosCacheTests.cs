using LandauBasicFunctionCache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiPrecision;

namespace LandauBasicFunctionCacheTests {
    [TestClass()]
    public class SinCosCacheTests {
        [TestMethod()]
        public void SinTest() {
            for (double xs = -8; xs <= 8; xs += 0.125) {
                MultiPrecision<Pow2.N16> x = xs;

                MultiPrecision<Pow2.N16> expected = MultiPrecision<Pow2.N16>.Sin(x);
                MultiPrecision<Pow2.N16> actual = SinCosCache<Pow2.N16>.Sin(x);

                Console.WriteLine(x);
                Console.WriteLine(expected);
                Console.WriteLine(actual);

                MultiPrecision<Pow2.N16> error = expected - actual;

                Assert.IsTrue(error.Exponent < -MultiPrecision<Pow2.N16>.Bits + 2);
            }

            for (double xs = -8; xs <= 8; xs += 0.125) {
                MultiPrecision<Pow2.N16> x = xs + MultiPrecision<Pow2.N16>.Sqrt2;

                MultiPrecision<Pow2.N16> expected = MultiPrecision<Pow2.N16>.Sin(x);
                MultiPrecision<Pow2.N16> actual = SinCosCache<Pow2.N16>.Sin(x);

                Console.WriteLine(x);
                Console.WriteLine(expected);
                Console.WriteLine(actual);

                MultiPrecision<Pow2.N16> error = expected - actual;

                Assert.IsTrue(error.Exponent < -MultiPrecision<Pow2.N16>.Bits + 2);
            }
        }

        [TestMethod()]
        public void CosTest() {
            for (double xs = -8; xs <= 8; xs += 0.125) {
                MultiPrecision<Pow2.N16> x = xs;

                MultiPrecision<Pow2.N16> expected = MultiPrecision<Pow2.N16>.Cos(x);
                MultiPrecision<Pow2.N16> actual = SinCosCache<Pow2.N16>.Cos(x);

                Console.WriteLine(x);
                Console.WriteLine(expected);
                Console.WriteLine(actual);

                MultiPrecision<Pow2.N16> error = expected - actual;

                Assert.IsTrue(error.Exponent < -MultiPrecision<Pow2.N16>.Bits + 2);
            }

            for (double xs = -8; xs <= 8; xs += 0.125) {
                MultiPrecision<Pow2.N16> x = xs + MultiPrecision<Pow2.N16>.Sqrt2;

                MultiPrecision<Pow2.N16> expected = MultiPrecision<Pow2.N16>.Cos(x);
                MultiPrecision<Pow2.N16> actual = SinCosCache<Pow2.N16>.Cos(x);

                Console.WriteLine(x);
                Console.WriteLine(expected);
                Console.WriteLine(actual);

                MultiPrecision<Pow2.N16> error = expected - actual;

                Assert.IsTrue(error.Exponent < -MultiPrecision<Pow2.N16>.Bits + 2);
            }
        }

        [TestMethod()]
        public void SinPITest() {
            for (double xs = -8 + Math.ScaleB(1, -32); xs <= 8; xs += 0.125) {
                MultiPrecision<Pow2.N16> x = xs;

                MultiPrecision<Pow2.N16> expected = MultiPrecision<Pow2.N16>.SinPI(x);
                MultiPrecision<Pow2.N16> actual = SinCosCache<Pow2.N16>.SinPI(x);

                Console.WriteLine(x);
                Console.WriteLine(expected);
                Console.WriteLine(actual);

                MultiPrecision<Pow2.N16> error = expected - actual;

                if (expected == 0 && actual == 0) {
                    continue;
                }

                Assert.IsTrue(error.Exponent < -MultiPrecision<Pow2.N16>.Bits + 2);
            }

            for (double xs = -8; xs <= 8; xs += 0.125) {
                MultiPrecision<Pow2.N16> x = xs;

                MultiPrecision<Pow2.N16> expected = MultiPrecision<Pow2.N16>.SinPI(x);
                MultiPrecision<Pow2.N16> actual = SinCosCache<Pow2.N16>.SinPI(x);

                Console.WriteLine(x);
                Console.WriteLine(expected);
                Console.WriteLine(actual);

                MultiPrecision<Pow2.N16> error = expected - actual;

                if (expected == 0 && actual == 0) {
                    continue;
                }

                Assert.IsTrue(error.Exponent < -MultiPrecision<Pow2.N16>.Bits + 2);
            }

            for (double xs = -8; xs <= 8; xs += 0.125) {
                MultiPrecision<Pow2.N16> x = xs + MultiPrecision<Pow2.N16>.Sqrt2;

                MultiPrecision<Pow2.N16> expected = MultiPrecision<Pow2.N16>.SinPI(x);
                MultiPrecision<Pow2.N16> actual = SinCosCache<Pow2.N16>.SinPI(x);

                Console.WriteLine(x);
                Console.WriteLine(expected);
                Console.WriteLine(actual);

                MultiPrecision<Pow2.N16> error = expected - actual;

                Assert.IsTrue(error.Exponent < -MultiPrecision<Pow2.N16>.Bits + 2);
            }
        }

        [TestMethod()]
        public void CosPITest() {
            for (double xs = -8 + Math.ScaleB(1, -32); xs <= 8; xs += 0.125) {
                MultiPrecision<Pow2.N16> x = xs;

                MultiPrecision<Pow2.N16> expected = MultiPrecision<Pow2.N16>.CosPI(x);
                MultiPrecision<Pow2.N16> actual = SinCosCache<Pow2.N16>.CosPI(x);

                Console.WriteLine(x);
                Console.WriteLine(expected);
                Console.WriteLine(actual);

                MultiPrecision<Pow2.N16> error = expected - actual;

                if (expected == 0 && actual == 0) {
                    continue;
                }

                Assert.IsTrue(error.Exponent < -MultiPrecision<Pow2.N16>.Bits + 2);
            }

            for (double xs = -8; xs <= 8; xs += 0.125) {
                MultiPrecision<Pow2.N16> x = xs;

                MultiPrecision<Pow2.N16> expected = MultiPrecision<Pow2.N16>.CosPI(x);
                MultiPrecision<Pow2.N16> actual = SinCosCache<Pow2.N16>.CosPI(x);

                Console.WriteLine(x);
                Console.WriteLine(expected);
                Console.WriteLine(actual);

                MultiPrecision<Pow2.N16> error = expected - actual;

                if (expected == 0 && actual == 0) {
                    continue;
                }

                Assert.IsTrue(error.Exponent < -MultiPrecision<Pow2.N16>.Bits + 2);
            }

            for (double xs = -8; xs <= 8; xs += 0.125) {
                MultiPrecision<Pow2.N16> x = xs + MultiPrecision<Pow2.N16>.Sqrt2;

                MultiPrecision<Pow2.N16> expected = MultiPrecision<Pow2.N16>.CosPI(x);
                MultiPrecision<Pow2.N16> actual = SinCosCache<Pow2.N16>.CosPI(x);

                Console.WriteLine(x);
                Console.WriteLine(expected);
                Console.WriteLine(actual);

                MultiPrecision<Pow2.N16> error = expected - actual;

                Assert.IsTrue(error.Exponent < -MultiPrecision<Pow2.N16>.Bits + 2);
            }
        }
    }
}