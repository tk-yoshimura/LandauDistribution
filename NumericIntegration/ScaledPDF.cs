using MultiPrecision;

namespace NumericIntegration {
    public static class ScaledPDF<N> where N : struct, IConstant {
        public static (MultiPrecision<N> value, MultiPrecision<N> error) Value(MultiPrecision<N> x, MultiPrecision<N> eps) {
            if (x.Sign == Sign.Plus) {
                return PDFPositiveSide<N>.ScaledValue(x, eps);
            }
            else {
                return PDFNegativeSide<N>.ScaledValue(x, eps);
            }
        }
    }
}
