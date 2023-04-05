using SeriesExpansion;

using StreamWriter sw = new("../../../../../results_disused/asymp_plus_poly.txt");

for (int j = 1; j <= 64; j++) {
    SymbolicPoly p = new();

    for (int k = 0; k < j; k++) {
        if (((j + k) & 1) == 1) {
            p += RTable.Value(j, k) * CTable.Value(j, k);
        }
    }

    string str = p.ToString();

    Console.WriteLine(str.Length > 128 ? $"{j}: {str[..128]}..." : $"{j}: {str}");
    sw.WriteLine(str);
    sw.Flush();
}

sw.Close();

Console.WriteLine("END");
Console.Read();