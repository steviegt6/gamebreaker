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
using GameBreaker.Serial.Numerics;
using GameBreaker.Tests.Serial;

namespace GameBreaker.Benchmarks.Serial.BinaryBuffer;

[MemoryDiagnoser]
public class WriteAll {
    // ReSharper disable once UnassignedField.Global
    [Params(1_000, 100_000, 1_000_000)]
    public int N;

    private bool[] b = null!;
    private byte[] u8 = null!;
    private short[] i16 = null!;
    private ushort[] u16 = null!;
    private Int24[] i24 = null!;
    private UInt24[] u24 = null!;
    private int[] i32 = null!;
    private uint[] u32 = null!;
    private long[] i64 = null!;
    private ulong[] u64 = null!;
    private float[] f32 = null!;
    private double[] f64 = null!;
    private IBinaryWriter directBufferWriter = null!;
    private IBinaryWriter getBytesArrayCopyWriter = null!;
    private IBinaryWriter getBytesBufferBlockCopyWriter = null!;
    private IBinaryWriter tryWriteBytesArrayCopyWriter = null!;
    private IBinaryWriter tryWriteBytesBlockCopyWriter = null!;
    private IBinaryWriter unsafeAsPointerWriter = null!;

    [IterationSetup]
    public void Setup() {
        var size = sizeof(bool) // narrow boolean
                 + sizeof(int) // wide boolean
                 + sizeof(byte)
                 + sizeof(short)
                 + sizeof(ushort)
                 + Int24.SIZE
                 + UInt24.SIZE
                 + sizeof(int)
                 + sizeof(uint)
                 + sizeof(long)
                 + sizeof(ulong)
                 + sizeof(float)
                 + sizeof(double) * N;
        directBufferWriter = new DirectBufferWriter(size);
        getBytesArrayCopyWriter = new GetBytesArrayCopyWriter(size);
        getBytesBufferBlockCopyWriter = new GetBytesBufferBlockCopyWriter(size);
        tryWriteBytesArrayCopyWriter = new TryWriteBytesArrayCopyWriter(size);
        tryWriteBytesBlockCopyWriter = new TryWriteBytesBlockCopyWriter(size);
        unsafeAsPointerWriter = new UnsafeAsPointerWriter(size);

        b = new bool[N];
        u8 = new byte[N];
        i16 = new short[N];
        u16 = new ushort[N];
        i24 = new Int24[N];
        u24 = new UInt24[N];
        i32 = new int[N];
        u32 = new uint[N];
        i64 = new long[N];
        u64 = new ulong[N];
        f32 = new float[N];
        f64 = new double[N];

        var rand = new Random();

        for (var i = 0; i < N; i++) {
            b[i] = rand.Next(0, 2) == 0;
            u8[i] = (byte)rand.Next(byte.MinValue, byte.MaxValue);
            i16[i] = (short)rand.Next(short.MinValue, short.MaxValue);
            u16[i] = (ushort)rand.Next(ushort.MinValue, ushort.MaxValue);
            i24[i] = new Int24(rand.Next(Int24.MinValue, Int24.MaxValue));
            u24[i] = new UInt24((uint) rand.Next(0, (int) UInt24.MaxValue));
            i32[i] = rand.Next(int.MinValue, int.MaxValue);
            u32[i] = (uint)rand.Next(int.MinValue, int.MaxValue);
            i64[i] = (long)rand.Next(int.MinValue, int.MaxValue) << 32
                   | (uint)rand.Next(int.MinValue, int.MaxValue);
            u64[i] = (ulong)rand.Next(int.MinValue, int.MaxValue) << 32
                   | (uint)rand.Next(int.MinValue, int.MaxValue);
            f32[i] = (float)rand.NextDouble();
            f64[i] = rand.NextDouble();
        }
    }

    [Benchmark]
    public void DirectBufferWriteAll() {
        for (var i = 0; i < N; i++) {
            directBufferWriter.Write(b[i], wide: false);
            directBufferWriter.Write(b[i], wide: true);
            directBufferWriter.Write(u8[i]);
            directBufferWriter.Write(i16[i]);
            directBufferWriter.Write(u16[i]);
            directBufferWriter.Write(i24[i]);
            directBufferWriter.Write(u24[i]);
            directBufferWriter.Write(i32[i]);
            directBufferWriter.Write(u32[i]);
            directBufferWriter.Write(i64[i]);
            directBufferWriter.Write(u64[i]);
            directBufferWriter.Write(f32[i]);
            directBufferWriter.Write(f64[i]);
        }
    }

    [Benchmark]
    public void BitConverterGetBytesArrayCopyAll() {
        for (var i = 0; i < N; i++) {
            getBytesArrayCopyWriter.Write(b[i], wide: false);
            getBytesArrayCopyWriter.Write(b[i], wide: true);
            getBytesArrayCopyWriter.Write(u8[i]);
            getBytesArrayCopyWriter.Write(i16[i]);
            getBytesArrayCopyWriter.Write(u16[i]);
            getBytesArrayCopyWriter.Write(i24[i]);
            getBytesArrayCopyWriter.Write(u24[i]);
            getBytesArrayCopyWriter.Write(i32[i]);
            getBytesArrayCopyWriter.Write(u32[i]);
            getBytesArrayCopyWriter.Write(i64[i]);
            getBytesArrayCopyWriter.Write(u64[i]);
            getBytesArrayCopyWriter.Write(f32[i]);
            getBytesArrayCopyWriter.Write(f64[i]);
        }
    }

    [Benchmark]
    public void BitConverterGetBytesBufferBlockCopyAll() {
        for (var i = 0; i < N; i++) {
            getBytesBufferBlockCopyWriter.Write(b[i], wide: false);
            getBytesBufferBlockCopyWriter.Write(b[i], wide: true);
            getBytesBufferBlockCopyWriter.Write(u8[i]);
            getBytesBufferBlockCopyWriter.Write(i16[i]);
            getBytesBufferBlockCopyWriter.Write(u16[i]);
            getBytesBufferBlockCopyWriter.Write(i24[i]);
            getBytesBufferBlockCopyWriter.Write(u24[i]);
            getBytesBufferBlockCopyWriter.Write(i32[i]);
            getBytesBufferBlockCopyWriter.Write(u32[i]);
            getBytesBufferBlockCopyWriter.Write(i64[i]);
            getBytesBufferBlockCopyWriter.Write(u64[i]);
            getBytesBufferBlockCopyWriter.Write(f32[i]);
            getBytesBufferBlockCopyWriter.Write(f64[i]);
        }
    }

    [Benchmark]
    public void BitConverterTryWriteBytesByteArrayArrayCopyAll() {
        for (var i = 0; i < N; i++) {
            tryWriteBytesArrayCopyWriter.Write(b[i], wide: false);
            tryWriteBytesArrayCopyWriter.Write(b[i], wide: true);
            tryWriteBytesArrayCopyWriter.Write(u8[i]);
            tryWriteBytesArrayCopyWriter.Write(i16[i]);
            tryWriteBytesArrayCopyWriter.Write(u16[i]);
            tryWriteBytesArrayCopyWriter.Write(i24[i]);
            tryWriteBytesArrayCopyWriter.Write(u24[i]);
            tryWriteBytesArrayCopyWriter.Write(i32[i]);
            tryWriteBytesArrayCopyWriter.Write(u32[i]);
            tryWriteBytesArrayCopyWriter.Write(i64[i]);
            tryWriteBytesArrayCopyWriter.Write(u64[i]);
            tryWriteBytesArrayCopyWriter.Write(f32[i]);
            tryWriteBytesArrayCopyWriter.Write(f64[i]);
        }
    }

    [Benchmark]
    public void BitConverterTryWriteBytesByteArrayBufferBlockCopyAll() {
        for (var i = 0; i < N; i++) {
            tryWriteBytesBlockCopyWriter.Write(b[i], wide: false);
            tryWriteBytesBlockCopyWriter.Write(b[i], wide: true);
            tryWriteBytesBlockCopyWriter.Write(u8[i]);
            tryWriteBytesBlockCopyWriter.Write(i16[i]);
            tryWriteBytesBlockCopyWriter.Write(u16[i]);
            tryWriteBytesBlockCopyWriter.Write(i24[i]);
            tryWriteBytesBlockCopyWriter.Write(u24[i]);
            tryWriteBytesBlockCopyWriter.Write(i32[i]);
            tryWriteBytesBlockCopyWriter.Write(u32[i]);
            tryWriteBytesBlockCopyWriter.Write(i64[i]);
            tryWriteBytesBlockCopyWriter.Write(u64[i]);
            tryWriteBytesBlockCopyWriter.Write(f32[i]);
            tryWriteBytesBlockCopyWriter.Write(f64[i]);
        }
    }

    [Benchmark]
    public void UnsafeAsPointerWriteAll() {
        for (var i = 0; i < N; i++) {
            unsafeAsPointerWriter.Write(b[i], wide: false);
            unsafeAsPointerWriter.Write(b[i], wide: true);
            unsafeAsPointerWriter.Write(u8[i]);
            unsafeAsPointerWriter.Write(i16[i]);
            unsafeAsPointerWriter.Write(u16[i]);
            unsafeAsPointerWriter.Write(i24[i]);
            unsafeAsPointerWriter.Write(u24[i]);
            unsafeAsPointerWriter.Write(i32[i]);
            unsafeAsPointerWriter.Write(u32[i]);
            unsafeAsPointerWriter.Write(i64[i]);
            unsafeAsPointerWriter.Write(u64[i]);
            unsafeAsPointerWriter.Write(f32[i]);
            unsafeAsPointerWriter.Write(f64[i]);
        }
    }
}
