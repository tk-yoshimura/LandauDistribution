using System.Numerics;
using SymbolicArithmetic;

using StreamReader stream = new("../../../../../results_disused/asymp_minus_poly_frac.txt");
using StreamWriter sw = new("../../../../../results_disused/asymp_minus_cdf_poly.txt");

List<Fraction> coefs = new();
while (!stream.EndOfStream) {
    string? line = stream.ReadLine();

    if (string.IsNullOrWhiteSpace(line)) {
        break;
    }

    string[] item = line.Split('/');

    Fraction coef = item.Length <= 1 ? BigInteger.Parse(item[0]) : new Fraction(BigInteger.Parse(item[0]), BigInteger.Parse(item[1]));
    coefs.Add(coef);
}

Poly poly = new(new Fraction(BigInteger.Zero));

for (int n = 0; n < coefs.Count; n++) {

    List<Fraction> p = new();
    for (int j = 0; j < n; j++) {
        p.Add(0);
    }

    Fraction f = 1;
    p.Add(f);
    for (int j = n; j < coefs.Count; j++) {
        f *= -j - new Fraction(1, 2);
        p.Add(f);
    }

    poly += new Poly(p.ToArray()) * coefs[n];
}

sw.WriteLine(poly);