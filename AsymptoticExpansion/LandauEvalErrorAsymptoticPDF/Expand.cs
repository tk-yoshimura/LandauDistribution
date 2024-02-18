using MultiPrecision;

namespace LandauEvalErrorAsymptoticPDF {
    internal struct N12 : IConstant {
        public readonly int Value => 12;
    }

    internal struct N24 : IConstant {
        public readonly int Value => 24;
    }

    internal struct Plus1<N> : IConstant where N : struct, IConstant {
        public readonly int Value => checked(default(N).Value + 1);
    }
}
