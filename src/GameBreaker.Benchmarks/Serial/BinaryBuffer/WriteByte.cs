/*
 * Copyright (c) 2023 Tomat & GameBreaker Contributors
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using BenchmarkDotNet.Attributes;
using GameBreaker.Serial;
using GameBreaker.Tests.Serial;

namespace GameBreaker.Benchmarks.Serial.BinaryBuffer;

[MemoryDiagnoser]
public class WriteByte {
    // ReSharper disable once UnassignedField.Global
    [Params(1_000, 100_000, 1_000_000)]
    public int N;

    private byte[] u8 = null!;

    private IBinaryWriter directBufferWriter = null!;
    private IBinaryWriter unsafeAsPointerWriter = null!;

    [IterationSetup]
    public void Setup() {
        var size = sizeof(byte) * N;
        directBufferWriter = new DirectBufferWriter(size);
        unsafeAsPointerWriter = new UnsafeAsPointerWriter(size);

        u8 = new byte[N];

        var rand = new Random();

        for (var i = 0; i < N; i++)
            u8[i] = (byte)rand.Next(byte.MinValue, byte.MaxValue);
    }

    [Benchmark]
    public void DirectBufferWriteByte() {
        for (var i = 0; i < N; i++)
            directBufferWriter.Write(u8[i]);
    }

    [Benchmark]
    public void UnsafeAsPointerWriteByte() {
        for (var i = 0; i < N; i++)
            unsafeAsPointerWriter.Write(u8[i]);
    }
}
