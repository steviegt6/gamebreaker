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

        var writer = new BufferBinaryWriter(size);
        var rand = new Random();
        var bWArr = new bool[100];
        var bNArr = new bool[100];
        var u8Arr = new byte[100];
        var i16Arr = new short[100];
        var u16Arr = new ushort[100];
        var i24Arr = new Int24[100];
        var u24Arr = new UInt24[100];
        var i32Arr = new int[100];
        var u32Arr = new uint[100];
        var i64Arr = new long[100];
        var u64Arr = new ulong[100];
        var f32Arr = new float[100];
        var f64Arr = new double[100];

        for (var i = 0; i < 100; i++) {
            var b = rand.Next(0, 2) == 0;
            var u8 = (byte)rand.Next(byte.MinValue, byte.MaxValue);
            var i16 = (short)rand.Next(short.MinValue, short.MaxValue);
            var u16 = (ushort)rand.Next(ushort.MinValue, ushort.MaxValue);
            var i24 = new Int24(rand.Next(Int24.MinValue, Int24.MaxValue));
            var u24 = new UInt24((uint) rand.Next(0, (int) UInt24.MaxValue));
            var i32 = rand.Next(int.MinValue, int.MaxValue);
            var u32 = (uint)rand.Next(int.MinValue, int.MaxValue);
            var i64 = (long)rand.Next(int.MinValue, int.MaxValue) << 32
                    | (uint)rand.Next(int.MinValue, int.MaxValue);
            var u64 = (ulong)rand.Next(int.MinValue, int.MaxValue) << 32
                    | (uint)rand.Next(int.MinValue, int.MaxValue);
            var f32 = (float)rand.NextDouble();
            var f64 = rand.NextDouble();

            bWArr[i] = b;
            bNArr[i] = b;
            u8Arr[i] = u8;
            i16Arr[i] = i16;
            u16Arr[i] = u16;
            i24Arr[i] = i24;
            u24Arr[i] = u24;
            i32Arr[i] = i32;
            u32Arr[i] = u32;
            i64Arr[i] = i64;
            u64Arr[i] = u64;
            f32Arr[i] = f32;
            f64Arr[i] = f64;

            writer.Write(b, wide: false);
            writer.Write(b, wide: true);
            writer.Write(u8);
            writer.Write(i16);
            writer.Write(u16);
            writer.Write(i24);
            writer.Write(u24);
            writer.Write(i32);
            writer.Write(u32);
            writer.Write(i64);
            writer.Write(u64);
            writer.Write(f32);
            writer.Write(f64);
        }

        var buffer = writer.Buffer;
        IBinaryReader one = new BitConverterReader(buffer.ToArray());
        IBinaryReader two = new PointerCastReader(buffer.ToArray());

        for (var i = 0; i < 100; i++) {
            Assert.Multiple(() => {
                Assert.That(one.ReadBoolean(false), Is.EqualTo(bWArr[i]));
                Assert.That(one.ReadBoolean(true), Is.EqualTo(bNArr[i]));
                Assert.That(one.ReadByte(), Is.EqualTo(u8Arr[i]));
                Assert.That(one.ReadInt16(), Is.EqualTo(i16Arr[i]));
                Assert.That(one.ReadUInt16(), Is.EqualTo(u16Arr[i]));
                Assert.That(one.ReadInt24(), Is.EqualTo(i24Arr[i]));
                Assert.That(one.ReadUInt24(), Is.EqualTo(u24Arr[i]));
                Assert.That(one.ReadInt32(), Is.EqualTo(i32Arr[i]));
                Assert.That(one.ReadUInt32(), Is.EqualTo(u32Arr[i]));
                Assert.That(one.ReadInt64(), Is.EqualTo(i64Arr[i]));
                Assert.That(one.ReadUInt64(), Is.EqualTo(u64Arr[i]));
                Assert.That(one.ReadSingle(), Is.EqualTo(f32Arr[i]));
                Assert.That(one.ReadDouble(), Is.EqualTo(f64Arr[i]));
            });

            Assert.Multiple(() => {
                Assert.That(two.ReadBoolean(false), Is.EqualTo(bWArr[i]));
                Assert.That(two.ReadBoolean(true), Is.EqualTo(bNArr[i]));
                Assert.That(two.ReadByte(), Is.EqualTo(u8Arr[i]));
                Assert.That(two.ReadInt16(), Is.EqualTo(i16Arr[i]));
                Assert.That(two.ReadUInt16(), Is.EqualTo(u16Arr[i]));
                Assert.That(two.ReadInt24(), Is.EqualTo(i24Arr[i]));
                Assert.That(two.ReadUInt24(), Is.EqualTo(u24Arr[i]));
                Assert.That(two.ReadInt32(), Is.EqualTo(i32Arr[i]));
                Assert.That(two.ReadUInt32(), Is.EqualTo(u32Arr[i]));
                Assert.That(two.ReadInt64(), Is.EqualTo(i64Arr[i]));
                Assert.That(two.ReadUInt64(), Is.EqualTo(u64Arr[i]));
                Assert.That(two.ReadSingle(), Is.EqualTo(f32Arr[i]));
                Assert.That(two.ReadDouble(), Is.EqualTo(f64Arr[i]));
            });
        }
    }
}
