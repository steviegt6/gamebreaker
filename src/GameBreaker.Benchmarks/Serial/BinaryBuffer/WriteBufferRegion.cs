using System;
using BenchmarkDotNet.Attributes;

namespace GameBreaker.Benchmarks.Serial.BinaryBuffer; 

[MemoryDiagnoser]
public class WriteBufferRegion {
    // ReSharper disable once UnassignedField.Global
    [Params(1_000, 100_000, 1_000_000)]
    public int N;
    
    private Memory<byte>[] regions = null!;
    
    [IterationSetup]
    public void Setup() {
        regions = new Memory<byte>[N];
        
        for (var i = 0; i < N; i++)
            regions[i] = new Memory<byte>(new byte[1]);
    }
}
