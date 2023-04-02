using System.Diagnostics;

namespace SeriesExpansion {
    [DebuggerDisplay("{ToString(),nq}")]
    public class Symbol: IComparable, IComparable<Symbol>, IEquatable<Symbol> {
        private readonly int pi, gamma, max_zeta_n;
        private readonly Dictionary<int, int> zeta_list;

        public Symbol(int pi, int gamma, params (int n, int pow)[] zeta) {
            if (zeta.Select(item => item.n).Any(n => n <= 1 || (n & 1) == 0)) {
                throw new ArgumentException("zeta(n) must be odd integer zeta term", nameof(zeta));
            }

            this.pi = pi;
            this.gamma = gamma;
            this.zeta_list = new Dictionary<int, int>(zeta.Select(item => new KeyValuePair<int, int>(item.n, item.pow)));
            this.max_zeta_n = zeta.Length >= 1 ? zeta.Select(z => z.n).Max() : 0;
        }

        private Symbol(int pi, int gamma, Dictionary<int, int> zeta) {
            this.pi = pi;
            this.gamma = gamma;
            this.zeta_list = zeta;
            this.max_zeta_n = zeta.Count >= 1 ? zeta.Select(z => z.Key).Max() : 0;
        }

        public int PI => pi;
        public int Gamma => gamma;
        public int Zeta(int n) => zeta_list.ContainsKey(n) ? zeta_list[n] : 0;

        public static Symbol None { private set; get; } = new Symbol(pi: 0, gamma: 0);

        public static Symbol operator *(Symbol s1, Symbol s2) {
            int ret_pi = checked(s1.pi + s2.pi);
            int ret_gamma = checked(s1.gamma + s2.gamma);
            Dictionary<int, int> ret_zeta = new(s1.zeta_list);

            foreach (int n in s2.zeta_list.Keys) {
                int pow = s2.zeta_list[n];

                if (ret_zeta.ContainsKey(n)) {
                    ret_zeta[n] = checked(ret_zeta[n] + pow);
                }
                else {
                    ret_zeta.Add(n, pow);
                }
            }

            return new Symbol(ret_pi, ret_gamma, ret_zeta);
        }

        public static bool operator <(Symbol s1, Symbol s2) {
            if (s1.gamma < s2.gamma) {
                return true;
            }
            if (s1.gamma > s2.gamma) {
                return false;
            }
            
            for (int n = 3, nmax = Math.Max(s1.max_zeta_n, s2.max_zeta_n); n <= nmax; n += 2) {
                int z1 = s1.Zeta(n), z2 = s2.Zeta(n);

                if (z1 < z2) {
                    return true;
                }
                if (z1 > z2) {
                    return false;
                }
            }
                        
            if (s1.pi < s2.pi) {
                return true;
            }
            if (s1.pi > s2.pi) {
                return false;
            }

            return false;
        }

        public static bool operator >(Symbol s1, Symbol s2) {
            if (s1.gamma > s2.gamma) {
                return true;
            }
            if (s1.gamma < s2.gamma) {
                return false;
            }
            
            for (int n = 3, nmax = Math.Max(s1.max_zeta_n, s2.max_zeta_n); n <= nmax; n += 2) {
                int z1 = s1.Zeta(n), z2 = s2.Zeta(n);

                if (z1 > z2) {
                    return true;
                }
                if (z1 < z2) {
                    return false;
                }
            }

            if (s1.pi > s2.pi) {
                return true;
            }
            if (s1.pi < s2.pi) {
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

            if (s1.gamma != s2.gamma) {
                return false;
            }

            for (int n = 3, nmax = Math.Max(s1.max_zeta_n, s2.max_zeta_n); n <= nmax; n += 2) {
                if (s1.Zeta(n) != s2.Zeta(n)) {
                    return false;
                }
            }
                        
            if (s1.pi != s2.pi) {
                return false;
            }

            return true;
        }

        public static bool operator !=(Symbol s1, Symbol s2) {
            return !(s1 == s2);
        }

        public int CompareTo(Symbol? value) {
            if (value is null) {
                throw new ArgumentNullException(nameof(value));
            }

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
            int hash = pi.GetHashCode() ^ gamma.GetHashCode();

            hash ^= zeta_list.Count.GetHashCode();

            for (int n = 3, nmax = checked(2 * zeta_list.Count + 1); n <= nmax; n += 2) {
                hash ^= Zeta(n).GetHashCode();
            }

            return hash;
        }

        public override string ToString() {
            List<string> strs = new();

            if (pi != 0) {
                strs.Add(pi == 1 ? "pi" : $"pi^{pi}");
            }

            foreach (int n in zeta_list.Keys) {
                int pow = zeta_list[n];

                strs.Add(pow == 1 ? $"zeta({n})" : $"zeta({n})^{pow}");
            }

            if (gamma != 0) {
                strs.Add(gamma == 1 ? "gamma" : $"gamma^{gamma}");
            }

            if (strs.Count == 0) {
                return "None";
            }

            return string.Join('*', strs);
        }
    }
}
