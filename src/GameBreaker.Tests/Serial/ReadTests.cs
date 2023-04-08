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

using System.Text;
using GameBreaker.Serial;
using GameBreaker.Serial.Numerics;

namespace GameBreaker.Tests.Serial;

public abstract class AbstractBinaryReader : IBinaryReader {
    protected int Offset;
    protected readonly byte[] Buffer;

    int IPositionable.Offset {
        get => Offset;
        set => Offset = value;
    }

    int IPositionable.Length => Buffer.Length;

    byte[] IPositionable.Buffer => Buffer;

    public Encoding Encoding { get ; } = new UTF8Encoding(false);

    protected AbstractBinaryReader(int length) {
        Buffer = new byte[length];
    }

    protected AbstractBinaryReader(byte[] buffer) {
        Buffer = buffer;
    }

    public abstract byte ReadByte();

    public bool ReadBoolean(bool wide) {
        return wide ? ReadInt32() != 0 : ReadByte() != 0;
    }

    public abstract string ReadChars(int count);

    public abstract Memory<byte> ReadBytes(int count);

    public abstract short ReadInt16();

    public abstract ushort ReadUInt16();

    public abstract Int24 ReadInt24();

    public abstract UInt24 ReadUInt24();

    public abstract int ReadInt32();

    public abstract uint ReadUInt32();

    public abstract long ReadInt64();

    public abstract ulong ReadUInt64();

    public abstract float ReadSingle();

    public abstract double ReadDouble();

    void IDisposable.Dispose() {
        GC.SuppressFinalize(this);
    }
}

public sealed class BitConverterReader : AbstractBinaryReader {
    public BitConverterReader(int length) : base(length) { }

    public BitConverterReader(byte[] buffer) : base(buffer) { }

    public override byte ReadByte() {
        return Buffer[Offset++];
    }

    public override string ReadChars(int count) {
        throw new NotImplementedException();
    }

    public override Memory<byte> ReadBytes(int count) {
        throw new NotImplementedException();
    }

    public override short ReadInt16() {
        return GmBitConverter.ToInt16(Buffer, ref Offset);
    }

    public override ushort ReadUInt16() {
        return GmBitConverter.ToUInt16(Buffer, ref Offset);
    }

    public override Int24 ReadInt24() {
        return GmBitConverter.ToInt24(Buffer, ref Offset);
    }

    public override UInt24 ReadUInt24() {
        return GmBitConverter.ToUInt24(Buffer, ref Offset);
    }

    public override int ReadInt32() {
        return GmBitConverter.ToInt32(Buffer, ref Offset);
    }

    public override uint ReadUInt32() {
        return GmBitConverter.ToUInt32(Buffer, ref Offset);
    }

    public override long ReadInt64() {
        return GmBitConverter.ToInt64(Buffer, ref Offset);
    }

    public override ulong ReadUInt64() {
        return GmBitConverter.ToUInt64(Buffer, ref Offset);
    }

    public override float ReadSingle() {
        return GmBitConverter.ToSingle(Buffer, ref Offset);
    }

    public override double ReadDouble() {
        return GmBitConverter.ToDouble(Buffer, ref Offset);
    }
}

public sealed class PointerCastReader : AbstractBinaryReader {
    public PointerCastReader(int length) : base(length) { }

    public PointerCastReader(byte[] buffer) : base(buffer) { }

    public override unsafe byte ReadByte() {
        fixed (byte* ptr = &Buffer[Offset]) {
            Offset += sizeof(byte);
            return *ptr;
        }
    }

    public override string ReadChars(int count) {
        throw new NotImplementedException();
    }

    public override Memory<byte> ReadBytes(int count) {
        throw new NotImplementedException();
    }

    public override unsafe short ReadInt16() {
        fixed (byte* ptr = &Buffer[Offset]) {
            Offset += sizeof(short);
            return *(short*)ptr;
        }
    }

    public override unsafe ushort ReadUInt16() {
        fixed (byte* ptr = &Buffer[Offset]) {
            Offset += sizeof(ushort);
            return *(ushort*)ptr;
        }
    }

    public override unsafe Int24 ReadInt24() {
        fixed (byte* ptr = &Buffer[Offset]) {
            Offset += sizeof(Int24);
            return *(Int24*)ptr;
        }
    }

    public override unsafe UInt24 ReadUInt24() {
        fixed (byte* ptr = &Buffer[Offset]) {
            Offset += sizeof(UInt24);
            return *(UInt24*)ptr;
        }
    }

    public override unsafe int ReadInt32() {
        fixed (byte* ptr = &Buffer[Offset]) {
            Offset += sizeof(int);
            return *(int*)ptr;
        }
    }

    public override unsafe uint ReadUInt32() {
        fixed (byte* ptr = &Buffer[Offset]) {
            Offset += sizeof(uint);
            return *(uint*)ptr;
        }
    }

    public override unsafe long ReadInt64() {
        fixed (byte* ptr = &Buffer[Offset]) {
            Offset += sizeof(long);
            return *(long*)ptr;
        }
    }

    public override unsafe ulong ReadUInt64() {
        fixed (byte* ptr = &Buffer[Offset]) {
            Offset += sizeof(ulong);
            return *(ulong*)ptr;
        }
    }

    public override unsafe float ReadSingle() {
        fixed (byte* ptr = &Buffer[Offset]) {
            Offset += sizeof(float);
            return *(float*)ptr;
        }
    }

    public override unsafe double ReadDouble() {
        fixed (byte* ptr = &Buffer[Offset]) {
            Offset += sizeof(double);
            return *(double*)ptr;
        }
    }
}

[TestFixture]
public static class ReadTests {
    [Test]
    public static void TestReadBasicData() {
        const int size = sizeof(bool) // narrow boolean
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
                       + sizeof(double) * 100;
        var writer = new GmDataWriter(size, null!, null!);
        var rand = new Random();
        var b = new bool[100];
        var u8 = new byte[100];
        var i16 = new short[100];
        var u16 = new ushort[100];
        var i24 = new Int24[100];
        var u24 = new UInt24[100];
        var i32 = new int[100];
        var u32 = new uint[100];
        var i64 = new long[100];
        var u64 = new ulong[100];
        var f32 = new float[100];
        var f64 = new double[100];

        for (var i = 0; i < 100; i++) {
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
        IBinaryReader one = new BitConverterReader(buffer.ToArray());
        IBinaryReader two = new PointerCastReader(buffer.ToArray());

        for (var i = 0; i < 100; i++) {
            Assert.Multiple(() => {
                Assert.That(one.ReadBoolean(wide: false), Is.EqualTo(b[i]));
                Assert.That(one.ReadBoolean(wide: true), Is.EqualTo(b[i]));
                Assert.That(one.ReadByte(), Is.EqualTo(u8[i]));
                Assert.That(one.ReadInt16(), Is.EqualTo(i16[i]));
                Assert.That(one.ReadUInt16(), Is.EqualTo(u16[i]));
                Assert.That(one.ReadInt24(), Is.EqualTo(i24[i]));
                Assert.That(one.ReadUInt24(), Is.EqualTo(u24[i]));
                Assert.That(one.ReadInt32(), Is.EqualTo(i32[i]));
                Assert.That(one.ReadUInt32(), Is.EqualTo(u32[i]));
                Assert.That(one.ReadInt64(), Is.EqualTo(i64[i]));
                Assert.That(one.ReadUInt64(), Is.EqualTo(u64[i]));
                Assert.That(one.ReadSingle(), Is.EqualTo(f32[i]));
                Assert.That(one.ReadDouble(), Is.EqualTo(f64[i]));
            });

            Assert.Multiple(() => {
                Assert.That(two.ReadBoolean(wide: false), Is.EqualTo(b[i]));
                Assert.That(two.ReadBoolean(wide: true), Is.EqualTo(b[i]));
                Assert.That(two.ReadByte(), Is.EqualTo(u8[i]));
                Assert.That(two.ReadInt16(), Is.EqualTo(i16[i]));
                Assert.That(two.ReadUInt16(), Is.EqualTo(u16[i]));
                Assert.That(two.ReadInt24(), Is.EqualTo(i24[i]));
                Assert.That(two.ReadUInt24(), Is.EqualTo(u24[i]));
                Assert.That(two.ReadInt32(), Is.EqualTo(i32[i]));
                Assert.That(two.ReadUInt32(), Is.EqualTo(u32[i]));
                Assert.That(two.ReadInt64(), Is.EqualTo(i64[i]));
                Assert.That(two.ReadUInt64(), Is.EqualTo(u64[i]));
                Assert.That(two.ReadSingle(), Is.EqualTo(f32[i]));
                Assert.That(two.ReadDouble(), Is.EqualTo(f64[i]));
            });
        }
    }
}
