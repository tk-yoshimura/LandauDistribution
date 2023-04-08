using MultiPrecision;
using SeriesExpansion;
using ToNumeric;

using StreamReader sr = new("../../../../results_disused/asymp_plus_poly.txt");
using StreamWriter sw = new("../../../../results_disused/asymp_plus_poly_digits64.txt");
using BinaryWriter swb = new(File.Open($"../../../../results_disused/asymp_plus_poly_bits256.bin", FileMode.Create));

sw.WriteLine("n,a_n,mpplus");

int n = 0, stage = 0;

while (!sr.EndOfStream) {
    string? line = sr.ReadLine();

    if (string.IsNullOrWhiteSpace(line)) {
        break;
    }

    sw.Flush();
    swb.Flush();

    Console.WriteLine($"Processing {n}...");

    SymbolicPoly poly = SymbolicPoly.Parse(line);

    if (stage < 1) {
        MultiPrecision<Pow2.N8> v8_p1 = SymbolNumeric<Pow2.N8>.Value<Plus1<Pow2.N8>>(poly);
        MultiPrecision<Pow2.N8> v8_p2 = SymbolNumeric<Pow2.N8>.Value<Plus2<Pow2.N8>>(poly);

        if (v8_p1 == v8_p2) {
            Console.WriteLine($"value: {v8_p1}");
            Console.WriteLine($"stage: {stage}");

            sw.WriteLine($"{n},{v8_p1:e64},1");
            swb.Write(v8_p1);
            n++;
            continue;
        }

        stage = 1;
    }

    if (stage < 2) {
        MultiPrecision<Pow2.N8> v8_p2 = SymbolNumeric<Pow2.N8>.Value<Plus2<Pow2.N8>>(poly);
        MultiPrecision<Pow2.N8> v8_p4 = SymbolNumeric<Pow2.N8>.Value<Plus4<Pow2.N8>>(poly);

        if (v8_p2 == v8_p4) {
            Console.WriteLine($"value: {v8_p2}");
            Console.WriteLine($"stage: {stage}");

            sw.WriteLine($"{n},{v8_p2:e64},2");
            swb.Write(v8_p2);
            n++;
            continue;
        }

        stage = 2;
    }

    if (stage < 4) {
        MultiPrecision<Pow2.N8> v8_p4 = SymbolNumeric<Pow2.N8>.Value<Plus4<Pow2.N8>>(poly);
        MultiPrecision<Pow2.N8> v8_p8 = SymbolNumeric<Pow2.N8>.Value<Plus8<Pow2.N8>>(poly);

        if (v8_p4 == v8_p8) {
            Console.WriteLine($"value: {v8_p4}");
            Console.WriteLine($"stage: {stage}");

            sw.WriteLine($"{n},{v8_p4:e64},4");
            swb.Write(v8_p4);
            n++;
            continue;
        }

        stage = 4;
    }

    if (stage < 8) {
        MultiPrecision<Pow2.N8> v8_p8 = SymbolNumeric<Pow2.N8>.Value<Plus8<Pow2.N8>>(poly);
        MultiPrecision<Pow2.N8> v8_p16 = SymbolNumeric<Pow2.N8>.Value<Plus16<Pow2.N8>>(poly);

        if (v8_p8 == v8_p16) {
            Console.WriteLine($"value: {v8_p8}");
            Console.WriteLine($"stage: {stage}");

            sw.WriteLine($"{n},{v8_p8:e64},8");
            swb.Write(v8_p8);
            n++;
            continue;
        }

        stage = 8;
    }

    if (stage <= 16) {
        MultiPrecision<Pow2.N8> v8_p16 = SymbolNumeric<Pow2.N8>.Value<Plus16<Pow2.N8>>(poly);
        MultiPrecision<Pow2.N8> v8_p32 = SymbolNumeric<Pow2.N8>.Value<Plus32<Pow2.N8>>(poly);

        if (v8_p16 == v8_p32) {
            Console.WriteLine($"value: {v8_p16}");
            Console.WriteLine($"stage: {stage}");

            sw.WriteLine($"{n},{v8_p16:e64},16");
            swb.Write(v8_p16);
            n++;
            continue;
        }

        stage = 16;

        Console.WriteLine($"value: {v8_p32}");
        Console.WriteLine($"stage: {stage}");

        sw.WriteLine($"{n},{v8_p32:e64},32+");
        swb.Write(v8_p32);
        n++;
        continue;
    }
}

sw.Close();
swb.Close();