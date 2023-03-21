using System;
using BenchmarkDotNet.Attributes;
using GameBreaker.Serial;
using GameBreaker.Tests.Serial;

namespace GameBreaker.Benchmarks.Serial.BinaryBuffer;

[MemoryDiagnoser]
public class WriteByteArray {
    // ReSharper disable once UnassignedField.Global
    [Params(1_000, 100_000)]
    public int N;

    private byte[][] regions = null!;
    private IBinaryWriter byteArrayArrayCopyWriter = null!;
    private IBinaryWriter byteArrayBufferBlockCopyWriter = null!;
    private IBinaryWriter byteArrayUnsafePointerWriter = null!;
    private IBinaryWriter byteArrayUnsafeCopyBlockWriter = null!;

    [IterationSetup]
    public void Setup() {
        var size = sizeof(byte) * 1000 * N;
        byteArrayArrayCopyWriter = new ByteArrayArrayCopyWriter(size);
        byteArrayBufferBlockCopyWriter =
            new ByteArrayBufferBlockCopyWriter(size);
        byteArrayUnsafePointerWriter = new ByteArrayUnsafePointerWriter(size);
        byteArrayUnsafeCopyBlockWriter =
            new ByteArrayUnsafeCopyBlockWriter(size);

        regions = new byte[N][];

        var rand = new Random();

        for (var i = 0; i < N; i++) {
            regions[i] = new byte[1000];
            rand.NextBytes(regions[i]);
        }
    }

    [Benchmark]
    public void ByteArrayArrayCopy() {
        for (var i = 0; i < N; i++)
            byteArrayArrayCopyWriter.Write(regions[i]);
    }

    [Benchmark]
    public void ByteArrayBufferBlockCopy() {
        for (var i = 0; i < N; i++)
            byteArrayBufferBlockCopyWriter.Write(regions[i]);
    }

    [Benchmark]
    public void ByteArrayUnsafePointer() {
        for (var i = 0; i < N; i++)
            byteArrayUnsafePointerWriter.Write(regions[i]);
    }

    [Benchmark]
    public void ByteArrayUnsafeCopyBlock() {
        for (var i = 0; i < N; i++)
            byteArrayUnsafeCopyBlockWriter.Write(regions[i]);
    }
}
