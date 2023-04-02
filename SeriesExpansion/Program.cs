using SeriesExpansion;

for (int n = 0; n <= 4; n++) {
    Console.WriteLine(BTable.Value(n));
}

for (int k = 0; k <= 4; k++) {
    for (int j = 1; j <= 4; j++) {
        Console.WriteLine($"R_({j},{k}) = {RTable.Value(j, k)}");
    }

    Console.WriteLine("");
}

Console.WriteLine("END");
Console.Read();
