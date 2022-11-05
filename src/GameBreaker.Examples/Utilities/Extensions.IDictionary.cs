// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

namespace GameBreaker.Utilities;

partial class Extensions
{
    public static void SetTo<TKey, TElement>(this IDictionary<TKey, TElement> source, IDictionary<TKey, TElement> value) {
        source.Clear();
        foreach (var (key, element) in value) source.Add(key, element);
    }
}