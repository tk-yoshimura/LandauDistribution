using MultiPrecision;

namespace ToNumeric {
    internal struct Plus1<N> : IConstant where N : struct, IConstant {
        public int Value => checked(default(N).Value + 1);
    }

    internal struct Plus2<N> : IConstant where N : struct, IConstant {
        public int Value => checked(default(N).Value + 2);
    }

    internal struct Plus4<N> : IConstant where N : struct, IConstant {
        public int Value => checked(default(N).Value + 4);
    }

    internal struct Plus8<N> : IConstant where N : struct, IConstant {
        public int Value => checked(default(N).Value + 8);
    }

    internal struct Plus16<N> : IConstant where N : struct, IConstant {
        public int Value => checked(default(N).Value + 16);
    }

    internal struct Plus32<N> : IConstant where N : struct, IConstant {
        public int Value => checked(default(N).Value + 32);
    }

    internal struct Plus64<N> : IConstant where N : struct, IConstant {
        public int Value => checked(default(N).Value + 64);
    }
}
