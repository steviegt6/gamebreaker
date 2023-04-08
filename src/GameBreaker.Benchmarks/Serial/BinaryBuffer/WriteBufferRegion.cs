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
public class WriteBufferRegion {
    // ReSharper disable once UnassignedField.Global
    [Params(1_000, 100_000)]
    public int N;

    private byte[][] regions = null!;
    private IBinaryWriter bufferRegionCopyToWriter = null!;
    private IBinaryWriter bufferRegionUnsafePointerWriter = null!;
    private IBinaryWriter bufferRegionUnsafeSimdWriter = null!;

    [IterationSetup]
    public void Setup() {
        var size = sizeof(byte) * 1000 * N;
        bufferRegionCopyToWriter = new BufferRegionCopyToWriter(size);
        bufferRegionUnsafePointerWriter =
            new BufferRegionUnsafePointerWriter(size);
        bufferRegionUnsafeSimdWriter = new BufferRegionUnsafeSimdWriter(size);
        
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
    
    [Benchmark]
    public void BufferRegionUnsafeSimd() {
        for (var i = 0; i < N; i++)
            bufferRegionUnsafeSimdWriter.Write(regions[i].AsMemory());
    }
}
