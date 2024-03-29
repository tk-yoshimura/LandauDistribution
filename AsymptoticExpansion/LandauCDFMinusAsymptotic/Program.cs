﻿using LandauSymbolicArithmetic;
using System.Numerics;

using StreamReader sr = new("../../../../../results_disused/asymp_minus_pdf_poly_frac.txt");
using StreamWriter sw = new("../../../../../results_disused/asymp_minus_cdf_poly.txt");

List<Fraction> coefs = [];
while (!sr.EndOfStream) {
    string? line = sr.ReadLine();

    if (string.IsNullOrWhiteSpace(line)) {
        break;
    }

    string[] item = line.Split('/');

    Fraction coef = item.Length <= 1 ? BigInteger.Parse(item[0]) : new Fraction(BigInteger.Parse(item[0]), BigInteger.Parse(item[1]));
    coefs.Add(coef);
}

Poly poly = new(new Fraction(BigInteger.Zero));

for (int n = 0; n < coefs.Count; n++) {

    List<Fraction> p = [];
    for (int j = 0; j < n; j++) {
        p.Add(0);
    }

    Fraction f = 1;
    p.Add(f);
    for (int j = n; j < coefs.Count; j++) {
        f *= -j - new Fraction(1, 2);
        p.Add(f);
    }

    poly += new Poly([.. p]) * coefs[n];
}

sw.WriteLine(poly);