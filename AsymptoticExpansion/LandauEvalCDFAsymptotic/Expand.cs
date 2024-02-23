﻿using MultiPrecision;

namespace LandauEvalCDFAsymptotic {
    internal struct N18 : IConstant {
        public readonly int Value => 18;
    }

    internal struct N24 : IConstant {
        public readonly int Value => 24;
    }

    internal struct Plus1<N> : IConstant where N : struct, IConstant {
        public readonly int Value => checked(default(N).Value + 1);
    }
}
