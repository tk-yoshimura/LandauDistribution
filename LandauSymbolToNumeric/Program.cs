﻿using LandauSymbolicArithmetic;
using LandauSymbolToNumeric;
using MultiPrecision;

using StreamReader sr = new("../../../../results_disused/asymp_plus_poly.txt");
using StreamWriter sw = new("../../../../results_disused/asymp_plus_poly_digit227.txt");
using BinaryWriter swb = new(File.Open($"../../../../results_disused/asymp_plus_poly_bits768.bin", FileMode.Create));

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
        MultiPrecision<N24> v8_p1 = SymbolNumeric<N24>.Value<Plus1<N24>>(poly);
        MultiPrecision<N24> v8_p2 = SymbolNumeric<N24>.Value<Plus2<N24>>(poly);

        if (v8_p1 == v8_p2) {
            Console.WriteLine($"value: {v8_p1}");
            Console.WriteLine($"stage: {stage}");

            sw.WriteLine($"{n},{v8_p1:e227},1");
            swb.Write(v8_p1);
            n++;
            continue;
        }

        stage = 1;
    }

    if (stage < 2) {
        MultiPrecision<N24> v8_p2 = SymbolNumeric<N24>.Value<Plus2<N24>>(poly);
        MultiPrecision<N24> v8_p4 = SymbolNumeric<N24>.Value<Plus4<N24>>(poly);

        if (v8_p2 == v8_p4) {
            Console.WriteLine($"value: {v8_p2}");
            Console.WriteLine($"stage: {stage}");

            sw.WriteLine($"{n},{v8_p2:e227},2");
            swb.Write(v8_p2);
            n++;
            continue;
        }

        stage = 2;
    }

    if (stage < 4) {
        MultiPrecision<N24> v8_p4 = SymbolNumeric<N24>.Value<Plus4<N24>>(poly);
        MultiPrecision<N24> v8_p8 = SymbolNumeric<N24>.Value<Plus8<N24>>(poly);

        if (v8_p4 == v8_p8) {
            Console.WriteLine($"value: {v8_p4}");
            Console.WriteLine($"stage: {stage}");

            sw.WriteLine($"{n},{v8_p4:e227},4");
            swb.Write(v8_p4);
            n++;
            continue;
        }

        stage = 4;
    }

    if (stage < 8) {
        MultiPrecision<N24> v8_p8 = SymbolNumeric<N24>.Value<Plus8<N24>>(poly);
        MultiPrecision<N24> v8_p16 = SymbolNumeric<N24>.Value<Plus16<N24>>(poly);

        if (v8_p8 == v8_p16) {
            Console.WriteLine($"value: {v8_p8}");
            Console.WriteLine($"stage: {stage}");

            sw.WriteLine($"{n},{v8_p8:e227},8");
            swb.Write(v8_p8);
            n++;
            continue;
        }

        stage = 8;
    }

    if (stage <= 16) {
        MultiPrecision<N24> v8_p16 = SymbolNumeric<N24>.Value<Plus16<N24>>(poly);
        MultiPrecision<N24> v8_p32 = SymbolNumeric<N24>.Value<Plus32<N24>>(poly);

        if (v8_p16 == v8_p32) {
            Console.WriteLine($"value: {v8_p16}");
            Console.WriteLine($"stage: {stage}");

            sw.WriteLine($"{n},{v8_p16:e227},16");
            swb.Write(v8_p16);
            n++;
            continue;
        }

        stage = 16;

        Console.WriteLine($"value: {v8_p32}");
        Console.WriteLine($"stage: {stage}");

        sw.WriteLine($"{n},{v8_p32:e227},32+");
        swb.Write(v8_p32);
        n++;
        continue;
    }
}

sw.Close();
swb.Close();