using System.Collections.ObjectModel;
using System.Diagnostics;

namespace LandauSymbolicArithmetic {
    [DebuggerDisplay("{ToString(),nq}")]
    public class Symbol : IComparable, IComparable<Symbol>, IEquatable<Symbol> {
        public Symbol(int pi, int gamma, params (int n, int pow)[] zeta) {
            if (zeta.Select(item => item.n).Any(n => n <= 1 || (n & 1) == 0)) {
                throw new ArgumentException("zeta(n) must be odd integer zeta term", nameof(zeta));
            }

            this.PI = pi;
            this.Gamma = gamma;
            this.ZetaList = new ReadOnlyDictionary<int, int>(new Dictionary<int, int>(zeta.Select(item => new KeyValuePair<int, int>(item.n, item.pow))));
            this.MaxZetaN = zeta.Length >= 1 ? zeta.Select(z => z.n).Max() : 0;
        }

        private Symbol(int pi, int gamma, Dictionary<int, int> zeta) {
            this.PI = pi;
            this.Gamma = gamma;
            this.ZetaList = new ReadOnlyDictionary<int, int>(zeta);
            this.MaxZetaN = zeta.Count >= 1 ? zeta.Select(z => z.Key).Max() : 0;
        }

        public int PI { get; }
        public int Gamma { get; }
        public int MaxZetaN { get; }
        public ReadOnlyDictionary<int, int> ZetaList { get; }
        public int Zeta(int n) => ZetaList.TryGetValue(n, out int value) ? value : 0;

        public static Symbol None { private set; get; } = new Symbol(pi: 0, gamma: 0);


        public static Symbol operator *(Symbol s1, Symbol s2) {
            int ret_pi = checked(s1.PI + s2.PI);
            int ret_gamma = checked(s1.Gamma + s2.Gamma);
            Dictionary<int, int> ret_zeta = new(s1.ZetaList);

            foreach (int n in s2.ZetaList.Keys) {
                int pow = s2.ZetaList[n];

                if (ret_zeta.TryGetValue(n, out int value)) {
                    ret_zeta[n] = checked(value + pow);
                }
                else {
                    ret_zeta.Add(n, pow);
                }
            }

            return new Symbol(ret_pi, ret_gamma, ret_zeta);
        }

        public static bool operator <(Symbol s1, Symbol s2) {
            if (s1.Gamma < s2.Gamma) {
                return true;
            }
            if (s1.Gamma > s2.Gamma) {
                return false;
            }

            for (int n = Math.Max(s1.MaxZetaN, s2.MaxZetaN); n >= 3; n -= 2) {
                int z1 = s1.Zeta(n), z2 = s2.Zeta(n);

                if (z1 < z2) {
                    return true;
                }
                if (z1 > z2) {
                    return false;
                }
            }

            if (s1.PI < s2.PI) {
                return true;
            }
            if (s1.PI > s2.PI) {
                return false;
            }

            return false;
        }

        public static bool operator >(Symbol s1, Symbol s2) {
            if (s1.Gamma > s2.Gamma) {
                return true;
            }
            if (s1.Gamma < s2.Gamma) {
                return false;
            }

            for (int n = Math.Max(s1.MaxZetaN, s2.MaxZetaN); n >= 3; n -= 2) {
                int z1 = s1.Zeta(n), z2 = s2.Zeta(n);

                if (z1 > z2) {
                    return true;
                }
                if (z1 < z2) {
                    return false;
                }
            }

            if (s1.PI > s2.PI) {
                return true;
            }
            if (s1.PI < s2.PI) {
                return false;
            }

            return false;
        }

        public static bool operator <=(Symbol s1, Symbol s2) {
            return !(s1 > s2);
        }

        public static bool operator >=(Symbol s1, Symbol s2) {
            return !(s1 < s2);
        }

        public static bool operator ==(Symbol s1, Symbol s2) {
            if (ReferenceEquals(s1, s2)) {
                return true;
            }

            if (s1.Gamma != s2.Gamma) {
                return false;
            }

            for (int n = Math.Max(s1.MaxZetaN, s2.MaxZetaN); n >= 3; n -= 2) {
                if (s1.Zeta(n) != s2.Zeta(n)) {
                    return false;
                }
            }

            if (s1.PI != s2.PI) {
                return false;
            }

            return true;
        }

        public static bool operator !=(Symbol s1, Symbol s2) {
            return !(s1 == s2);
        }

        public int CompareTo(Symbol? value) {
            ArgumentNullException.ThrowIfNull(value);

            if (this < value) {
                return -1;
            }
            if (this > value) {
                return +1;
            }
            return 0;
        }

        public int CompareTo(object? obj) {
            if (obj is not Symbol symbol) {
                throw new ArgumentException(nameof(obj));
            }

            return this.CompareTo(symbol);
        }

        public override bool Equals(object? obj) {
            if (obj is null || obj is not Symbol s) {
                return false;
            }

            if (ReferenceEquals(this, obj)) {
                return true;
            }

            return this == s;
        }

        public bool Equals(Symbol? other) {
            return other is not null && this == other;
        }

        public override int GetHashCode() {
            int hash = PI.GetHashCode() ^ Gamma.GetHashCode();

            hash ^= ZetaList.Count.GetHashCode();

            for (int n = 3, nmax = checked(2 * ZetaList.Count + 1); n <= nmax; n += 2) {
                hash ^= Zeta(n).GetHashCode();
            }

            return hash;
        }

        public override string ToString() {
            List<string> strs = new();

            if (PI != 0) {
                strs.Add(PI == 1 ? "pi" : $"pi^{PI}");
            }

            foreach (int n in ZetaList.Keys) {
                int pow = ZetaList[n];

                strs.Add(pow == 1 ? $"zeta({n})" : $"zeta({n})^{pow}");
            }

            if (Gamma != 0) {
                strs.Add(Gamma == 1 ? "gamma" : $"gamma^{Gamma}");
            }

            if (strs.Count == 0) {
                return "None";
            }

            return string.Join('*', strs);
        }
    }
}
