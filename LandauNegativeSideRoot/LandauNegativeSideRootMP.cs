using MultiPrecision;

namespace LandauNegativeSideRoot {
    public static class LandauNegativeSideRootMP<N> where N: struct, IConstant {
        static readonly MultiPrecision<N> c = 2 / MultiPrecision<N>.PI, logc = MultiPrecision<N>.Log(c);

        /// <summary>solve for t : v = 2t/pi (x + log(t) + log(2/pi))</summary>
        private static MultiPrecision<N> SearchRoot(MultiPrecision<N> x, MultiPrecision<N> v, MultiPrecision<N> t0) {
            MultiPrecision<N> t = t0, dt, dt_prev = MultiPrecision<N>.NaN;

            for (int i = 0; i < 256; i++) {
                (MultiPrecision<N> f, MultiPrecision<N> df) = Func(x, t);

                MultiPrecision<N> dv = v - f;
                dt = dv / df;

                if (dt_prev.IsFinite) {
                    if (MultiPrecision<N>.Abs(dt) > MultiPrecision<N>.Abs(dt_prev)) {
                        dt = MultiPrecision<N>.Abs(dt_prev) * (dt.Sign == Sign.Plus ? +1 : -1);
                    }
                    if(dt * dt_prev < 0) {
                        dt /= 4;
                    }
                }

                dt_prev = dt;

                t += dt;

                if (dt.Exponent < t.Exponent - (MultiPrecision<N>.Bits - 8)) {
                    return t;
                }
                if ((i >= 32) && dt.Exponent < t.Exponent - (MultiPrecision<N>.Bits - 16)) {
                    return t;
                }
                if ((i >= 64) && dt.Exponent < t.Exponent - (MultiPrecision<N>.Bits - 32)) {
                    return t;
                }
            }

            return MultiPrecision<N>.NaN;
        }

        /// <summary>value and diff : 2t/pi (x + log(t) + log(2/pi))</summary>
        public static (MultiPrecision<N> v, MultiPrecision<N> dv) Func(MultiPrecision<N> x, MultiPrecision<N> t) {
            if (!(t >= 0)) {
                throw new ArgumentOutOfRangeException(nameof(t));
            }

            MultiPrecision<N> u = x + MultiPrecision<N>.Max(-1000d, MultiPrecision<N>.Log(t)) + logc;

            return (t * c * u, c * (u + 1d));
        }

        /// <summary>peak t : 2t/pi (x + log(t) + log(2/pi))</summary>
        public static MultiPrecision<N> Peak(MultiPrecision<N> x) {
            return 1d / (c * MultiPrecision<N>.Exp(x + 1d));
        }

        /// <summary>Enumerate cos curve waypoints</summary>
        public static IEnumerable<(MultiPrecision<N> t, int n)> EnumHalfPIValues(MultiPrecision<N> x, int maxpoints = 32) {
            (double t, int n)[] pts = LandauNegativeSideRootFP64.EnumHalfPIValues((double)x, maxpoints).ToArray();

            yield return (MultiPrecision<N>.Zero, 0);
            int i;

            for (i = 1; i < maxpoints; i++) {
                MultiPrecision<N> t = LandauNegativeSideRootMP<Plus1<N>>.SearchRoot(
                    x.Convert<Plus1<N>>(), pts[i].n * MultiPrecision<Plus1<N>>.PI / 2, pts[i].t).Convert<N>();

                yield return (t, pts[i].n);
            }

            yield break;
        }
    }
}
