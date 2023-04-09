using AsymptoticEvalError;
using MultiPrecision;

List<(MultiPrecision<Pow2.N8> lambda, MultiPrecision<Pow2.N8> cdf)> expecteds_forward = new(), expecteds_backward = new();

using StreamReader stream_forward = new("../../../../../results_disused/cdf_forward_precision70.csv");
stream_forward.ReadLine();

while (!stream_forward.EndOfStream) {
    string? line = stream_forward.ReadLine();

    if (string.IsNullOrWhiteSpace(line)) {
        break;
    }

    string[] item = line.Split(',');

    MultiPrecision<Pow2.N8> lambda = item[0], cdf = item[1];

    expecteds_forward.Add((lambda, cdf));
}

using StreamReader stream_backward = new("../../../../../results_disused/cdf_backward_precision70.csv");
stream_backward.ReadLine();

while (!stream_backward.EndOfStream) {
    string? line = stream_backward.ReadLine();

    if (string.IsNullOrWhiteSpace(line)) {
        break;
    }

    string[] item = line.Split(',');

    MultiPrecision<Pow2.N8> lambda = item[0], cdf = item[1];

    expecteds_backward.Add((lambda, cdf));
}

using StreamWriter sw_neg = new("../../../../../results_disused/cdf_negative_asymptotic_error.csv");
List<(MultiPrecision<Pow2.N8> lambda, MultiPrecision<Pow2.N8> cdf)> expecteds_neg = expecteds_forward.Where((item) => item.lambda <= -2.0).ToList();
sw_neg.Write("lambda");
foreach (int terms in new int[] { 2, 4, 6, 8, 12, 16, 24, 32, 48, 64 }) {
    sw_neg.Write($",{terms} {nameof(terms)}");
}
sw_neg.Write("\n");

foreach ((MultiPrecision<Pow2.N8> lambda, MultiPrecision<Pow2.N8> expected) in expecteds_neg) {
    sw_neg.Write($"{lambda}");

    foreach (int terms in new int[] { 2, 4, 6, 8, 12, 16, 24, 32, 48, 64 }) {
        MultiPrecision<Pow2.N8> actual = CDFNegativeSide<Pow2.N8>.Value(lambda, terms);

        MultiPrecision<Pow2.N8> relative_error = MultiPrecision<Pow2.N8>.Abs(expected - actual) / expected;

        sw_neg.Write($",{relative_error:e4}");
    }

    sw_neg.Write("\n");
}

using StreamWriter sw_pos = new("../../../../../results_disused/cdf_positive_asymptotic_error.csv");
List<(MultiPrecision<Pow2.N8> lambda, MultiPrecision<Pow2.N8> cdf)> expecteds_pos = expecteds_backward
    .Where((item) => item.lambda >= 4).OrderBy(item => item.lambda).ToList();

sw_pos.Write("lambda");
foreach (int terms in new int[] { 2, 4, 6, 8, 12, 16, 24, 32, 48, 64 }) {
    sw_pos.Write($",{terms} {nameof(terms)}");
}
sw_pos.Write("\n");

foreach ((MultiPrecision<Pow2.N8> lambda, MultiPrecision<Pow2.N8> expected) in expecteds_pos) {
    sw_pos.Write($"{lambda}");

    foreach (int terms in new int[] { 2, 4, 6, 8, 12, 16, 24, 32, 48, 64 }) {
        MultiPrecision<Pow2.N8> actual = CDFPositiveSide<Pow2.N8>.Value(lambda, terms);

        MultiPrecision<Pow2.N8> relative_error = MultiPrecision<Pow2.N8>.Abs(expected - actual) / expected;

        sw_pos.Write($",{relative_error:e4}");
    }

    sw_pos.Write("\n");
}

Console.WriteLine("END");
Console.Read();
