using System;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace GameBreaker.Benchmarks.Serial.BinaryBuffer;

public class Write {
    // ReSharper disable once UnassignedField.Global
    [Params(100, 100_000, 100_000_000, 1_000_000_000)]
    public int N;

    private int offset;
    private byte[] buffer = Array.Empty<byte>();

    [IterationSetup]
    public void Setup() {
        buffer = new byte[N * sizeof(int)];
        offset = 0;
    }

    [Benchmark]
    public void DirectBufferWrite() {
        // ReSharper disable once ConvertToConstant.Local
        var val = 42;

        for (var i = 0; i < N; i++) {
            buffer[offset++] = (byte)(val & 0xFF);
            buffer[offset++] = (byte)((val >> 8) & 0xFF);
            buffer[offset++] = (byte)((val >> 16) & 0xFF);
            buffer[offset++] = (byte)((val >> 24) & 0xFF);
        }
    }

    [Benchmark]
    public void BitConverterGetBytesArrayCopy() {
        // ReSharper disable once ConvertToConstant.Local
        var val = 42;

        for (var i = 0; i < N; i++) {
            var bytes = BitConverter.GetBytes(val);
            Array.Copy(bytes, 0, buffer, offset, bytes.Length);
            offset += sizeof(int);
        }
    }

    [Benchmark]
    public void BitConverterGetBytesBufferBlockCopy() {
        // ReSharper disable once ConvertToConstant.Local
        var val = 42;

        for (var i = 0; i < N; i++) {
            var bytes = BitConverter.GetBytes(val);
            Buffer.BlockCopy(bytes, 0, buffer, offset, sizeof(int));
            offset += sizeof(int);
        }
    }

    [Benchmark]
    public void BitConverterTryWriteBytesByteArrayArrayCopy() {
        // ReSharper disable once ConvertToConstant.Local
        var val = 42;

        for (var i = 0; i < N; i++) {
            var buf = new byte[sizeof(int)];
            BitConverter.TryWriteBytes(buf, val);
            Array.Copy(buf, 0, buffer, offset, buf.Length);
            offset += sizeof(int);
        }
    }

    [Benchmark]
    public void BitConverterTryWriteBytesByteArrayBufferBlockCopy() {
        // ReSharper disable once ConvertToConstant.Local
        var val = 42;

        for (var i = 0; i < N; i++) {
            var buf = new byte[sizeof(int)];
            BitConverter.TryWriteBytes(buf, val);
            Buffer.BlockCopy(buf, 0, buffer, offset, sizeof(int));
            offset += sizeof(int);
        }
    }

    [Benchmark]
    public void UnsafeAsPointerWrite() {
        // ReSharper disable once ConvertToConstant.Local
        var val = 42;

        for (var i = 0; i < N; i++) {
            ref var b = ref buffer[offset];
            Unsafe.As<byte, int>(ref b) = val;
            offset += sizeof(int);
        }
    }
}
