namespace SymbolicArithmetic {
    public class Poly {
        private readonly Fraction[] coef;

        public int Length => coef.Length;

        public Poly(Fraction coef) {
            this.coef = new Fraction[] { coef };
        }

        public Poly(Fraction[] coef) {
            this.coef = (Fraction[])coef.Clone();
        }

        public Fraction this[int index] => coef[index];

        public static Poly operator +(Poly p1, Poly p2) {
            Poly p;

            if (p1.Length >= p2.Length) {
                p = new Poly(p1.coef);
                for (int i = 0; i < p2.Length; i++) {
                    p.coef[i] += p2.coef[i];
                }
            }
            else {
                p = new Poly(p2.coef);
                for (int i = 0; i < p1.Length; i++) {
                    p.coef[i] += p1.coef[i];
                }
            }

            return p;
        }

        public static Poly operator *(Poly p, Fraction f) {
            Poly ret = new(p.coef);
            for (int i = 0; i < ret.Length; i++) {
                ret.coef[i] *= f;
            }

            return ret;
        }

        public override string ToString() {
            if (Length < 1) {
                return "0";
            }

            string str = string.Join('+',
                this.coef.Select((c, idx) => idx > 1 ? $"{c}x^{idx}" : idx > 0 ? $"{c}x" : $"{c}")
            ).Replace("+-", "-");

            return str;
        }
    }
}
