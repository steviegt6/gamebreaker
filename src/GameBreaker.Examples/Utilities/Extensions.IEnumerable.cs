// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

namespace GameBreaker.Utilities;

partial class Extensions
{
    /*public static IOrderedEnumerable<TSource> Shuffle<TSource, TKey>(this IEnumerable<TSource> source, params int[] indexesToPreserve) {
        List<TSource> list = source.ToList();
        List<(int index, TSource value)> indexed = list.Select((t, i) => (i, t)).ToList();

        List<(int index, TSource value)> preservedValues = new();
        foreach ((int i, var value) in indexed)
            if (indexesToPreserve.Contains(i))
                preservedValues.Add((i, value));

        var rand = new Random();
        list.RemoveAll(x => preservedValues.Any(y => y.value!.Equals(x)));
        List<TSource> shuffled = list.OrderBy(_ => rand.Next()).ToList();
    }*/

    public static IOrderedEnumerable<TSource> Shuffle<TSource>(this IEnumerable<TSource> source) {
        var rand = new Random();
        return source.OrderBy(_ => rand.Next());
    }

    public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source)
        where TKey : notnull {
        return source.ToDictionary(x => x.Key, x => x.Value);
    }
}