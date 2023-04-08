using AsymptoticMinus;
using SymbolicArithmetic;

using StreamWriter sw = new("../../../../../results_disused/asymp_minus_poly.txt");

List<Fraction> poly = new();

for (int k = 0; k <= 512; k++) {
    poly.Add(0);

    Poly c = CTable.Value(2 * k).p * Prod.Value(k);

    for (int i = 0; i < c.Length; i++) {
        poly[^(i + 1)] += c[i];
    }

    Poly p = new(poly.ToArray()[..k]);

    Console.WriteLine(p);
    sw.WriteLine(p);
    sw.Flush();
}

sw.Close();

Console.WriteLine("END");
Console.Read();
