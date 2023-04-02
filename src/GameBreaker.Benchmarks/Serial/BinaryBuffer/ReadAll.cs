using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using GameBreaker.Serial;
using GameBreaker.Serial.Numerics;
using GameBreaker.Tests.Serial;

namespace GameBreaker.Benchmarks.Serial.BinaryBuffer; 

[MemoryDiagnoser]
public class ReadAll {
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
    private IBinaryReader bitConverterReader;
    private IBinaryReader pointerCastReader;

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
        var writer = new GmDataWriter(size, null!, null!);
        var rand = new Random();
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

            writer.Write(b[i], wide: false);
            writer.Write(b[i], wide: true);
            writer.Write(u8[i]);
            writer.Write(i16[i]);
            writer.Write(u16[i]);
            writer.Write(i24[i]);
            writer.Write(u24[i]);
            writer.Write(i32[i]);
            writer.Write(u32[i]);
            writer.Write(i64[i]);
            writer.Write(u64[i]);
            writer.Write(f32[i]);
            writer.Write(f64[i]);
        }
        
        var buffer = writer.Buffer;
        bitConverterReader = new BitConverterReader(buffer.ToArray());
        pointerCastReader = new PointerCastReader(buffer.ToArray());
    }

    [Benchmark]
    public void BitConverterReadAll() {
        for (var i = 0; i < N; i++) {
            bitConverterReader.ReadBoolean(wide: false);
            bitConverterReader.ReadBoolean(wide: true);
            bitConverterReader.ReadByte();
            bitConverterReader.ReadInt16();
            bitConverterReader.ReadUInt16();
            bitConverterReader.ReadInt24();
            bitConverterReader.ReadUInt24();
            bitConverterReader.ReadInt32();
            bitConverterReader.ReadUInt32();
            bitConverterReader.ReadInt64();
            bitConverterReader.ReadUInt64();
            bitConverterReader.ReadSingle();
            bitConverterReader.ReadDouble();
        }
    }
    
    [Benchmark]
    public void PointerCastReadAll() {
        for (var i = 0; i < N; i++) {
            pointerCastReader.ReadBoolean(wide: false);
            pointerCastReader.ReadBoolean(wide: true);
            pointerCastReader.ReadByte();
            pointerCastReader.ReadInt16();
            pointerCastReader.ReadUInt16();
            pointerCastReader.ReadInt24();
            pointerCastReader.ReadUInt24();
            pointerCastReader.ReadInt32();
            pointerCastReader.ReadUInt32();
            pointerCastReader.ReadInt64();
            pointerCastReader.ReadUInt64();
            pointerCastReader.ReadSingle();
            pointerCastReader.ReadDouble();
        }
    }
}
