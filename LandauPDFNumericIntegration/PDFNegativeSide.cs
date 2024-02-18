using LandauBasicFunctionCache;
using MultiPrecision;
using MultiPrecisionIntegrate;

namespace LandauPDFNumericIntegration {
    public static class PDFNegativeSide<N> where N : struct, IConstant {
        public static (MultiPrecision<N> value, MultiPrecision<N> error) ScaledValue(MultiPrecision<N> x, MultiPrecision<N> eps) {
            if (!(x <= 0)) {
                throw new ArgumentOutOfRangeException(nameof(x), "Must be non-positive.");
            }

            MultiPrecision<N> sigma = ExpCache<N>.Exp(-x - 1), c = MultiPrecision<N>.Sqrt(MultiPrecision<N>.PI * sigma / 2);

            MultiPrecision<N> f(MultiPrecision<N> u) {
                MultiPrecision<N> cu = c * u, cu_sigma = cu / sigma;
                MultiPrecision<N> at = MultiPrecision<N>.Atan(cu_sigma);
                MultiPrecision<N> lg = LogCache<N>.Log(1 + cu_sigma * cu_sigma);

                MultiPrecision<N> exp = sigma * lg / 2 - cu * at;
                MultiPrecision<N> cos = cu * (lg / 2 - 1) + sigma * at;

                return ExpCache<N>.Exp(exp) * SinCosCache<N>.Cos(cos);
            }

            (MultiPrecision<N> sum, MultiPrecision<N> error)
                = GaussKronrodIntegral<N>.AdaptiveIntegrate(
                    f, 0, MultiPrecision<N>.PositiveInfinity, eps, GaussKronrodOrder.G32K65, depth: 64
            );

            return (sum, error);
        }
    }
}
