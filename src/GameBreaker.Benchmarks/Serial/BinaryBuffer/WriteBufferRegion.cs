using System;
using BenchmarkDotNet.Attributes;
using GameBreaker.Serial;
using GameBreaker.Tests.Serial;

namespace GameBreaker.Benchmarks.Serial.BinaryBuffer;

[MemoryDiagnoser]
public class WriteBufferRegion {
    // ReSharper disable once UnassignedField.Global
    [Params(1_000, 100_000)]
    public int N;

    private byte[][] regions = null!;
    private IBinaryWriter bufferRegionCopyToWriter = null!;
    private IBinaryWriter bufferRegionUnsafePointerWriter = null!;

    [IterationSetup]
    public void Setup() {
        var size = sizeof(byte) * 1000 * N;
        bufferRegionCopyToWriter = new BufferRegionCopyToWriter(size);
        bufferRegionUnsafePointerWriter =
            new BufferRegionUnsafePointerWriter(size);
        
        regions = new byte[N][];

        var rand = new Random();

        for (var i = 0; i < N; i++) {
            regions[i] = new byte[1000];
            rand.NextBytes(regions[i]);
        }
    }

    [Benchmark]
    public void BufferRegionCopyTo() {
        for (var i = 0; i < N; i++)
            bufferRegionCopyToWriter.Write(regions[i].AsMemory());
    }

    [Benchmark]
    public void BufferRegionUnsafePointer() {
        for (var i = 0; i < N; i++)
            bufferRegionUnsafePointerWriter.Write(regions[i].AsMemory());
    }
}
