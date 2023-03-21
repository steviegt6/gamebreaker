using System;
using BenchmarkDotNet.Attributes;
using GameBreaker.Serial;
using GameBreaker.Serial.Numerics;

namespace GameBreaker.Benchmarks.Serial.BinaryBuffer;

public class Write {
    // ReSharper disable once UnassignedField.Global
    [Params(1_000, 100_000, 1_000_000)]
    public int N;

    private bool b;
    private byte u8;
    private short i16;
    private ushort u16;
    private Int24 i24;
    private UInt24 u24;
    private int i32;
    private uint u32;
    private long i64;
    private ulong u64;
    private float f32;
    private double f64;
    private IBinaryWriter directBufferWriter = null!;
    private IBinaryWriter getBytesArrayCopyWriter = null!;
    private IBinaryWriter getBytesBufferBlockCopyWriter = null!;
    private IBinaryWriter tryWriteBytesArrayCopyWriter = null!;
    private IBinaryWriter tryWriteBytesBlockCopyWriter = null!;
    private IBinaryWriter unsafeAsPointerWriter = null!;

    [IterationSetup]
    public void Setup() {
        const int size = sizeof(bool)
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
                       + sizeof(double) * 100;
        directBufferWriter = new BufferBinaryWriter(size);
        getBytesArrayCopyWriter = new BufferBinaryWriter(size);
        getBytesBufferBlockCopyWriter = new BufferBinaryWriter(size);
        tryWriteBytesArrayCopyWriter = new BufferBinaryWriter(size);
        tryWriteBytesBlockCopyWriter = new BufferBinaryWriter(size);
        unsafeAsPointerWriter = new BufferBinaryWriter(size);

        var rand = new Random();
        b = rand.Next(0, 2) == 0;
        u8 = (byte)rand.Next(byte.MinValue, byte.MaxValue);
        i16 = (short)rand.Next(short.MinValue, short.MaxValue);
        u16 = (ushort)rand.Next(ushort.MinValue, ushort.MaxValue);
        i24 = new Int24(rand.Next(Int24.MinValue, Int24.MaxValue));
        u24 = new UInt24((uint) rand.Next(0, (int) UInt24.MaxValue));
        i32 = rand.Next(int.MinValue, int.MaxValue);
        u32 = (uint)rand.Next(int.MinValue, int.MaxValue);
        i64 = (long)rand.Next(int.MinValue, int.MaxValue) << 32
            | (uint)rand.Next(int.MinValue, int.MaxValue);
        u64 = (ulong)rand.Next(int.MinValue, int.MaxValue) << 32
            | (uint)rand.Next(int.MinValue, int.MaxValue);
        f32 = (float)rand.NextDouble();
        f64 = rand.NextDouble();
    }

    [Benchmark]
    public void DirectBufferWrite() {
        for (var i = 0; i < N; i++) {
            directBufferWriter.Write(b, wide: false);
            directBufferWriter.Write(b, wide: true);
            directBufferWriter.Write(u8);
            directBufferWriter.Write(i16);
            directBufferWriter.Write(u16);
            directBufferWriter.Write(i24);
            directBufferWriter.Write(u24);
            directBufferWriter.Write(i32);
            directBufferWriter.Write(u32);
            directBufferWriter.Write(i64);
            directBufferWriter.Write(u64);
            directBufferWriter.Write(f32);
            directBufferWriter.Write(f64);
        }
    }

    [Benchmark]
    public void BitConverterGetBytesArrayCopy() {
        for (var i = 0; i < N; i++) {
            getBytesArrayCopyWriter.Write(b, wide: false);
            getBytesArrayCopyWriter.Write(b, wide: true);
            getBytesArrayCopyWriter.Write(u8);
            getBytesArrayCopyWriter.Write(i16);
            getBytesArrayCopyWriter.Write(u16);
            getBytesArrayCopyWriter.Write(i24);
            getBytesArrayCopyWriter.Write(u24);
            getBytesArrayCopyWriter.Write(i32);
            getBytesArrayCopyWriter.Write(u32);
            getBytesArrayCopyWriter.Write(i64);
            getBytesArrayCopyWriter.Write(u64);
            getBytesArrayCopyWriter.Write(f32);
            getBytesArrayCopyWriter.Write(f64);
        }
    }

    [Benchmark]
    public void BitConverterGetBytesBufferBlockCopy() {
        for (var i = 0; i < N; i++) {
            getBytesBufferBlockCopyWriter.Write(b, wide: false);
            getBytesBufferBlockCopyWriter.Write(b, wide: true);
            getBytesBufferBlockCopyWriter.Write(u8);
            getBytesBufferBlockCopyWriter.Write(i16);
            getBytesBufferBlockCopyWriter.Write(u16);
            getBytesBufferBlockCopyWriter.Write(i24);
            getBytesBufferBlockCopyWriter.Write(u24);
            getBytesBufferBlockCopyWriter.Write(i32);
            getBytesBufferBlockCopyWriter.Write(u32);
            getBytesBufferBlockCopyWriter.Write(i64);
            getBytesBufferBlockCopyWriter.Write(u64);
            getBytesBufferBlockCopyWriter.Write(f32);
            getBytesBufferBlockCopyWriter.Write(f64);
        }
    }

    [Benchmark]
    public void BitConverterTryWriteBytesByteArrayArrayCopy() {
        for (var i = 0; i < N; i++) {
            tryWriteBytesArrayCopyWriter.Write(b, wide: false);
            tryWriteBytesArrayCopyWriter.Write(b, wide: true);
            tryWriteBytesArrayCopyWriter.Write(u8);
            tryWriteBytesArrayCopyWriter.Write(i16);
            tryWriteBytesArrayCopyWriter.Write(u16);
            tryWriteBytesArrayCopyWriter.Write(i24);
            tryWriteBytesArrayCopyWriter.Write(u24);
            tryWriteBytesArrayCopyWriter.Write(i32);
            tryWriteBytesArrayCopyWriter.Write(u32);
            tryWriteBytesArrayCopyWriter.Write(i64);
            tryWriteBytesArrayCopyWriter.Write(u64);
            tryWriteBytesArrayCopyWriter.Write(f32);
            tryWriteBytesArrayCopyWriter.Write(f64);
        }
    }

    [Benchmark]
    public void BitConverterTryWriteBytesByteArrayBufferBlockCopy() {
        for (var i = 0; i < N; i++) {
            tryWriteBytesBlockCopyWriter.Write(b, wide: false);
            tryWriteBytesBlockCopyWriter.Write(b, wide: true);
            tryWriteBytesBlockCopyWriter.Write(u8);
            tryWriteBytesBlockCopyWriter.Write(i16);
            tryWriteBytesBlockCopyWriter.Write(u16);
            tryWriteBytesBlockCopyWriter.Write(i24);
            tryWriteBytesBlockCopyWriter.Write(u24);
            tryWriteBytesBlockCopyWriter.Write(i32);
            tryWriteBytesBlockCopyWriter.Write(u32);
            tryWriteBytesBlockCopyWriter.Write(i64);
            tryWriteBytesBlockCopyWriter.Write(u64);
            tryWriteBytesBlockCopyWriter.Write(f32);
            tryWriteBytesBlockCopyWriter.Write(f64);
        }
    }

    [Benchmark]
    public void UnsafeAsPointerWrite() {
        for (var i = 0; i < N; i++) {
            unsafeAsPointerWriter.Write(b, wide: false);
            unsafeAsPointerWriter.Write(b, wide: true);
            unsafeAsPointerWriter.Write(u8);
            unsafeAsPointerWriter.Write(i16);
            unsafeAsPointerWriter.Write(u16);
            unsafeAsPointerWriter.Write(i24);
            unsafeAsPointerWriter.Write(u24);
            unsafeAsPointerWriter.Write(i32);
            unsafeAsPointerWriter.Write(u32);
            unsafeAsPointerWriter.Write(i64);
            unsafeAsPointerWriter.Write(u64);
            unsafeAsPointerWriter.Write(f32);
            unsafeAsPointerWriter.Write(f64);
        }
    }
}
