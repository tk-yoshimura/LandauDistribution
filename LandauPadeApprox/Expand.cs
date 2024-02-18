using MultiPrecision;

namespace LandauPadeApprox {
    public struct N12 : IConstant {
        public readonly int Value => 12;
    }

    internal struct Plus1<N> : IConstant where N : struct, IConstant {
        public readonly int Value => checked(default(N).Value + 1);
    }
}
