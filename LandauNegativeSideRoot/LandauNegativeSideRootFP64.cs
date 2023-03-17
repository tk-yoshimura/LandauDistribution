namespace LandauNegativeSideRoot {
    public static class LandauNegativeSideRootFP64 {
        static readonly double c = 2 / Math.PI, logc = Math.Log(c);

        /// <summary>solve for t : v = 2t/pi (x + log(t) + log(2/pi))</summary>
        private static double SearchRootDecreasePhase(double x, double v, double t0, double t_peak) {
            double t = t0, dt, dt_prev = double.NaN;

            for (int i = 0; i < 256; i++) {
                (double f, double df) = Func(x, t);

                double dv = v - f;
                dt = dv / df;

                if (double.IsFinite(dt_prev)) {
                    if (Math.Abs(dt) > Math.Abs(dt_prev)) {
                        dt = Math.CopySign(dt_prev, dt);
                    }
                    if(dt * dt_prev < 0) {
                        dt /= 4;
                    }
                }

                dt_prev = dt;

                t += dt;

                if (Math.Abs(dt / t) < 1e-15 || (i >= 32 && Math.Abs(dt / t) < 1e-14) || (i >= 64 && Math.Abs(dt / t) < 1e-13)) {
                    return t;
                }

                if (t > t_peak) {
                    t = t_peak;
                }
            }

            return double.NaN;
        }

        /// <summary>solve for t : v = 2t/pi (x + log(t) + log(2/pi))</summary>
        private static double SearchRootIncreasePhase(double x, double v, double t0, double t_peak) {
            double t = t0, dt, dt_prev = double.NaN;

            for (int i = 0; i < 256; i++) {
                (double f, double df) = Func(x, t);

                double dv = v - f;
                dt = dv / df;

                if (double.IsFinite(dt_prev)) {
                    if (Math.Abs(dt) > Math.Abs(dt_prev)) {
                        dt = Math.CopySign(dt_prev, dt);
                    }
                    if(dt * dt_prev < 0) {
                        dt /= 4;
                    }
                }

                dt_prev = dt;

                t += dt;

                if (Math.Abs(dt / t) < 1e-15 || (i >= 32 && Math.Abs(dt / t) < 1e-14) || (i >= 64 && Math.Abs(dt / t) < 1e-13)) {
                    return t;
                }

                if (t < t_peak) {
                    t = t_peak;
                }
            }

            return double.NaN;
        }

        /// <summary>value and diff : 2t/pi (x + log(t) + log(2/pi))</summary>
        public static (double v, double dv) Func(double x, double t) {
            if (!(t >= 0)) {
                throw new ArgumentOutOfRangeException(nameof(t));
            }

            double u = x + Math.Max(-725d, Math.Log(t)) + logc;

            return (t * c * u, c * (u + 1d));
        }

        /// <summary>peak t : 2t/pi (x + log(t) + log(2/pi))</summary>
        public static double Peak(double x) {
            return 1d / (c * Math.Exp(x + 1d));
        }

        /// <summary>Enumerate cos curve waypoints</summary>
        public static IEnumerable<(double t, int n)> EnumHalfPIValues(double x, int maxpoints = 32) {
            double t_peak = Peak(x), t_peakval = Func(x, t_peak).v;
            int decrease_points = (int)Math.Floor(-t_peakval * c);

            double t = 0d, dt = 1e-2d;
            yield return (t, 0);
            int i, j;

            for (i = 1, j = -1; i <= decrease_points && i < maxpoints; i++, j--) {
                double t_next = SearchRootDecreasePhase(x, j * Math.PI / 2, t + dt, t_peak);
                (t, dt) = (t_next, (t_next - t) / 2);
                yield return (t_next, j);
            }

            for (j += 1; i < maxpoints; i++, j++) {
                double t_next = SearchRootIncreasePhase(x, j * Math.PI / 2, t_peak + dt, t_peak);
                (t, dt) = (t_next, (t_next - t) / 2);
                yield return (t_next, j);
            }

            yield break;
        }
    }
}
