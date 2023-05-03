using MultiPrecision;

namespace Digits100 {
    internal struct Plus1<N> : IConstant where N : struct, IConstant {
        public int Value => checked(default(N).Value + 1);
    }
}
