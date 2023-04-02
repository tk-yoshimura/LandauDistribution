using SeriesExpansion;

using StreamWriter sw = new("../../../../../results_disused/asymp_plus_poly.txt");

for (int j = 1; j <= 256; j++) {
    SymbolicPoly p = new();

    for (int k = 0; k < j; k++) {
        if (((j + k) & 1) == 1) {
            p += RTable.Value(j, k) * CTable.Value(j, k);
        }
    }

    Console.WriteLine(p);
    sw.WriteLine(p);
    sw.Flush();
}

sw.Close();

Console.WriteLine("END");
Console.Read();