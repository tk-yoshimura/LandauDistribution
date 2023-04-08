using SymbolicArithmetic;

using StreamReader sr = new("../../../../../results_disused/asymp_plus_poly.txt");
using StreamWriter sw = new("../../../../../results_disused/asymp_plus_cdf_poly_digits64.txt");

List<string> lines = new();

while (!sr.EndOfStream) {
    string? line = sr.ReadLine();

    if (string.IsNullOrWhiteSpace(line)) {
        break;
    }

    lines.Add(line);
}

SymbolicPoly poly_prev = SymbolicPoly.Parse(lines[0]);
sw.WriteLine(poly_prev);

for (int n = 1; n < lines.Count; n++) {
    SymbolicPoly poly = SymbolicPoly.Parse(lines[n]);
    SymbolicPoly poly_cdf = (poly + poly_prev) * new Fraction(1, n + 1);

    poly_prev = poly;

    sw.WriteLine(poly_cdf);
    sw.Flush();
}