using MultiPrecision;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;

namespace LandauBasicFunctionCache {
    internal abstract class CacheTable<N, TRet> where N : struct, IConstant {
        readonly (bool computed, TRet ret)[] table;
        public int Exponent { get; }

        public CacheTable(int xmax_exponent, uint n = 1024) {
            if (!IsPower2(n)) {
                throw new ArgumentException("must be power 2 number", nameof(n));
            }

            Exponent = checked(Power2(n) - xmax_exponent);
            table = new (bool, TRet)[n + 1];
        }

        public (TRet ret, int index, MultiPrecision<N> x0, MultiPrecision<N> xr) Value(MultiPrecision<N> x) {
            int index = (int)MultiPrecision<N>.Floor(MultiPrecision<N>.Ldexp(x, Exponent));

            if (index < 0 || index >= table.Length) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            MultiPrecision<N> x0 = MultiPrecision<N>.Ldexp(index, -Exponent);
            MultiPrecision<N> xr = x - x0;

            if (!table[index].computed) {
                table[index] = (computed: true, ret: Compute(x0));
            }

            return (table[index].ret, index, x0, xr);
        }

        public virtual TRet Compute(MultiPrecision<N> x) => throw new NotImplementedException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPower2(uint value) {
            return (value >= 1u) && ((value & (value - 1u)) == 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Power2(uint value) {
            return 31 - (int)Lzcnt.LeadingZeroCount(value);
        }
    }
}
