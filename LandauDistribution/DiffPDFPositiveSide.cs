using MultiPrecision;
using System;

namespace LandauDistribution {
    static class DiffPDFPositiveSide<N> where N : struct, IConstant {
        public static (MultiPrecision<N> value, MultiPrecision<N> error, long accurate_bits) Value(MultiPrecision<N> x, int intergrate_iterations = 20) {
            if (!(x >= 0)) {
                throw new ArgumentOutOfRangeException(nameof(x), "Must be non-negative.");
            }

            const int needs_bits = 64;

            MultiPrecision<N> exp_mx = MultiPrecision<N>.Exp(-x);

            MultiPrecision<N> f(MultiPrecision<N> t) {
                return SinCosCache<N>.SinPIValue(t) * ExpCache<N>.Value(-x * t) * (t * PowCache<N>.Value(t) - exp_mx * (t + 1) * PowCache<N>.Value(t + 1));
            }

            MultiPrecision<N> sum, error, eps;
            double t_peak = FloorPeakT((double)x);

            if (t_peak < 0.125) {
                (MultiPrecision<N> s1, MultiPrecision<N> e1) = MultiPrecisionUtil.RombergIntegrate<N>(f, 0, t_peak * 4, max_iterations: intergrate_iterations);

                eps = MultiPrecision<N>.Ldexp(s1, -needs_bits - 8);

                (MultiPrecision<N> s2, MultiPrecision<N> e2) = MultiPrecisionUtil.RombergIntegrate(f, t_peak * 4, 1, max_iterations: intergrate_iterations, epsilon: eps);

                sum = s1 + s2;
                error = e1 + e2;
            }
            else {
                (sum, error) = MultiPrecisionUtil.RombergIntegrate<N>(f, 0, 1, max_iterations: intergrate_iterations);
            }

            eps = MultiPrecision<N>.Ldexp(sum, -needs_bits - 2);

            for (long t = 2; t < long.MaxValue - 1; t += 2) {
                (MultiPrecision<N> s, MultiPrecision<N> e) = MultiPrecisionUtil.RombergIntegrate(f, t, t + 1, max_iterations: intergrate_iterations, epsilon: eps);

                sum += s;
                error += e;

                if (sum.Exponent - s.Exponent < needs_bits + 8) {
                    continue;
                }

                break;
            }

            long accurate_bits = sum.Exponent - error.Exponent;

            return (-sum / MultiPrecision<N>.PI, error / MultiPrecision<N>.PI, accurate_bits);
        }

        public static double PeakT(double x) {
            if (x < 1) {
                return double.NaN;
            }

            double f(double t) {
                return Math.PI / Math.Tan(Math.PI * t) - Math.Log(t) + 1 / t - 1 - x;
            };

            static double df(double t) {
                double s = Math.PI / Math.Sin(Math.PI * t);

                return -s * s - 1 / t - 1 / (t * t);
            };

            double t = Math.Min(1 / x, 0.5);
            double dt = 1;
            int iter = 0;

            while (iter < 256 && t != t - dt / 2) {
                dt = f(t) / df(t);
                t -= dt;
                iter++;
            }

            return t;
        }

        public static double FloorPeakT(double x) {
            double t = PeakT(x);

            if (double.IsNaN(t)) {
                return double.NaN;
            }

            double ft = 1;

            for (int i = 0; i < 256; i++, ft /= 2) {
                if (t * 2 >= ft) {
                    break;
                }
            }

            return ft;
        }
    }
}
