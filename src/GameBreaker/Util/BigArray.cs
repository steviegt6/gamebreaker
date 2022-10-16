// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GameBreaker.Util
{
    // TODO: Enumerable?
    public class BigArray<T>
    {
        // TODO: Good max size?
        public const int MAXIMUM_SIZE = int.MaxValue;

        public List<T[]> Arrays { get; } = new();

        public ulong Size { get; }

        public T this[ulong index] {
            get => Arrays[(int) (index / Size)][(int) (index % Size)];
            set => Arrays[(int) (index / Size)][(int) (index % Size)] = value;
        }

        public BigArray(ulong size) {
            Size = size;

            while (size >= MAXIMUM_SIZE) {
                Arrays.Add(new T[MAXIMUM_SIZE]);
                size -= MAXIMUM_SIZE;
            }

            if (size > 0) Arrays.Add(new T[size]);

            Debug.Assert(Size == Arrays.Sum(x => (decimal) x.Length));
        }

        public static void Resize(ref BigArray<T> array, ulong newSize) {
            if (newSize < array.Size) throw new InvalidOperationException();

            BigArray<T> newArray = new(newSize);
            for (int i = 0; i < array.Arrays.Count; i++) Array.Copy(array.Arrays[i], newArray.Arrays[i], array.Arrays[i].Length);
            array = newArray;
        }
    }
}