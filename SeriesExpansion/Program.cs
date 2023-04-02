using SeriesExpansion;

for (int n = 0; n <= 4; n++) {
    Console.WriteLine(BTable.Value(n));
}

for (int n = 1; n <= 4; n++) {
    for (int k = 0; k <= n; k++) {
        Console.WriteLine($"s_({n},{k}) = {StirlingNumberTable.SignedValue(n, k)}");
    }

    Console.WriteLine("");
}

for (int k = 0; k <= 8; k++) {
    for (int j = 1; j <= 8; j++) {
        Console.WriteLine($"R_({j},{k}) = {RTable.Value(j, k)}");
    }

    Console.WriteLine("");
}

for (int j = 1; j < 8; j++) {
    for (int k = 0; k < j; k++) {
        Console.WriteLine($"c_({j},{k}) = {CTable.Value(j, k)}");
    }

    Console.WriteLine("");
}

Console.WriteLine("END");
Console.Read();
