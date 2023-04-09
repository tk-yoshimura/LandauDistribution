using MultiPrecision;
using MultiPrecisionIntegrate;

namespace NumericIntegration {
    public static class PDFNegativeSide<N> where N : struct, IConstant {
        static readonly MultiPrecision<N> c = 1 / MultiPrecision<N>.Sqrt(2 * MultiPrecision<N>.PI);

        public static (MultiPrecision<N> value, MultiPrecision<N> error) ScaledValue(MultiPrecision<N> x, MultiPrecision<N> eps) {
            if (!(x <= 0)) {
                throw new ArgumentOutOfRangeException(nameof(x), "Must be non-positive.");
            }

            MultiPrecision<N> sigma = MultiPrecision<N>.Exp(-x - 1), c = MultiPrecision<N>.Sqrt(MultiPrecision<N>.PI * sigma / 2);

            MultiPrecision<N> f(MultiPrecision<N> u) {
                MultiPrecision<N> cu = c * u, cu_sigma = cu / sigma;
                MultiPrecision<N> at = MultiPrecision<N>.Atan(cu_sigma);
                MultiPrecision<N> lg = MultiPrecision<N>.Log(1 + cu_sigma * cu_sigma);

                MultiPrecision<N> exp = sigma * lg / 2 - cu * at;
                MultiPrecision<N> cos = cu * (lg / 2 - 1) + sigma * at;

                return MultiPrecision<N>.Exp(exp) * MultiPrecision<N>.Cos(cos);
            }

            (MultiPrecision<N> sum, MultiPrecision<N> error)
                = GaussKronrodIntegral<N>.AdaptiveIntegrate(
                    f, 0, MultiPrecision<N>.PositiveInfinity, eps, GaussKronrodOrder.G32K65, depth: 16
            );

            return (sum, error);
        }

        public static (MultiPrecision<N> value, MultiPrecision<N> error) Value(MultiPrecision<N> x, MultiPrecision<N> eps) {
            (MultiPrecision<N> value, MultiPrecision<N> error) = ScaledValue(x, eps);

            MultiPrecision<N> sigma = MultiPrecision<N>.Exp(-x - 1);
            MultiPrecision<N> scale = c * MultiPrecision<N>.Sqrt(sigma) * MultiPrecision<N>.Exp(-sigma);

            return (value * scale, error * scale);
        }
    }
}
