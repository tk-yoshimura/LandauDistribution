using MultiPrecision;

namespace LandauDistribution {
    internal struct Plus1<N> : IConstant where N : struct, IConstant {
        public int Value => checked(default(N).Value + 1);
    }

    internal struct N5 : IConstant {
        public int Value => 5;
    }
}
