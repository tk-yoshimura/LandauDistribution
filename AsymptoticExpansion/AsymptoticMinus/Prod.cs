﻿using System.Numerics;

namespace AsymptoticMinus {
    internal static class Prod {
        private readonly static List<BigInteger> table = new() {
            1, 1
        };

        public static BigInteger Value(int n) {
            for (int k = table.Count; k <= n; k++) {
                table.Add(table[^1] * checked(2 * k - 1));
            }

            return table[n];
        }
    }
}