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
