using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;
using GameBreaker.Serial;
using GameBreaker.Serial.Numerics;

namespace GameBreaker.Tests.Serial;

public abstract class AbstractBinaryWriter : IBinaryWriter {
    protected int Offset;
    protected byte[] Buffer;

    int IPositionable.Offset {
        get => Offset;
        set => Offset = value;
    }

    public int Length { get; set; }

    byte[] IPositionable.Buffer => Buffer;

    public Encoding Encoding { get ; } = new UTF8Encoding(false);

    protected AbstractBinaryWriter(int size) {
        Buffer = new byte[size];
    }

    protected void EnsureCapacity(int size) {
        if (Buffer.Length >= size)
            return;

        var newSize = Math.Max(Buffer.Length * 2, size);
        Array.Resize(ref Buffer, newSize);
        Length = size;
    }

    public abstract void Write(byte value);

    public void Write(bool value, bool wide) {
        if (wide)
            Write(value ? 1 : 0);
        else
            Write((byte) (value ? 1 : 0));
    }

    public abstract void Write(Memory<byte> value);

    public abstract void Write(byte[] value);

    public abstract void Write(char[] value);

    public abstract void Write(short value);

    public abstract void Write(ushort value);

    public abstract void Write(Int24 value);

    public abstract void Write(UInt24 value);

    public abstract void Write(int value);

    public abstract void Write(uint value);

    public abstract void Write(long value);

    public abstract void Write(ulong value);

    public abstract void Write(float value);

    public abstract void Write(double value);

    void IBinaryWriter.Flush(Stream stream) { }

    void IDisposable.Dispose() {
        GC.SuppressFinalize(this);
    }
}

public sealed class DirectBufferWriter : AbstractBinaryWriter {
    public DirectBufferWriter(int size) : base(size) { }

    public override void Write(byte value) {
        EnsureCapacity(Offset + sizeof(byte));
        Buffer[Offset++] = value;
    }

    public override void Write(Memory<byte> value) {
        throw new NotImplementedException();
    }

    public override void Write(byte[] value) {
        throw new NotImplementedException();
    }

    public override void Write(char[] value) {
        throw new NotImplementedException();
    }

    public override void Write(short value) {
        EnsureCapacity(Offset + sizeof(short));
        Buffer[Offset++] = (byte)(value & 0xFF);
        Buffer[Offset++] = (byte)((value >> 8) & 0xFF);
    }

    public override void Write(ushort value) {
        EnsureCapacity(Offset + sizeof(ushort));
        Buffer[Offset++] = (byte)(value & 0xFF);
        Buffer[Offset++] = (byte)((value >> 8) & 0xFF);
    }

    public override void Write(Int24 value) {
        EnsureCapacity(Offset + Int24.SIZE);
        Buffer[Offset++] = (byte)(value & 0xFF);
        Buffer[Offset++] = (byte)((value >> 8) & 0xFF);
        Buffer[Offset++] = (byte)((value >> 16) & 0xFF);
    }

    public override void Write(UInt24 value) {
        EnsureCapacity(Offset + UInt24.SIZE);
        Buffer[Offset++] = (byte)(value & 0xFF);
        Buffer[Offset++] = (byte)((value >> 8) & 0xFF);
        Buffer[Offset++] = (byte)((value >> 16) & 0xFF);
    }

    public override void Write(int value) {
        EnsureCapacity(Offset + sizeof(int));
        Buffer[Offset++] = (byte)(value & 0xFF);
        Buffer[Offset++] = (byte)((value >> 8) & 0xFF);
        Buffer[Offset++] = (byte)((value >> 16) & 0xFF);
        Buffer[Offset++] = (byte)((value >> 24) & 0xFF);
    }

    public override void Write(uint value) {
        EnsureCapacity(Offset + sizeof(uint));
        Buffer[Offset++] = (byte)(value & 0xFF);
        Buffer[Offset++] = (byte)((value >> 8) & 0xFF);
        Buffer[Offset++] = (byte)((value >> 16) & 0xFF);
        Buffer[Offset++] = (byte)((value >> 24) & 0xFF);
    }

    public override void Write(long value) {
        EnsureCapacity(Offset + sizeof(long));
        Buffer[Offset++] = (byte)(value & 0xFF);
        Buffer[Offset++] = (byte)((value >> 8) & 0xFF);
        Buffer[Offset++] = (byte)((value >> 16) & 0xFF);
        Buffer[Offset++] = (byte)((value >> 24) & 0xFF);
        Buffer[Offset++] = (byte)((value >> 32) & 0xFF);
        Buffer[Offset++] = (byte)((value >> 40) & 0xFF);
        Buffer[Offset++] = (byte)((value >> 48) & 0xFF);
        Buffer[Offset++] = (byte)((value >> 56) & 0xFF);
    }

    public override void Write(ulong value) {
        EnsureCapacity(Offset + sizeof(ulong));
        Buffer[Offset++] = (byte)(value & 0xFF);
        Buffer[Offset++] = (byte)((value >> 8) & 0xFF);
        Buffer[Offset++] = (byte)((value >> 16) & 0xFF);
        Buffer[Offset++] = (byte)((value >> 24) & 0xFF);
        Buffer[Offset++] = (byte)((value >> 32) & 0xFF);
        Buffer[Offset++] = (byte)((value >> 40) & 0xFF);
        Buffer[Offset++] = (byte)((value >> 48) & 0xFF);
        Buffer[Offset++] = (byte)((value >> 56) & 0xFF);
    }

    public override void Write(float value) {
        EnsureCapacity(Offset + sizeof(float));
        var bytes = BitConverter.GetBytes(value);
        Buffer[Offset++] = bytes[0];
        Buffer[Offset++] = bytes[1];
        Buffer[Offset++] = bytes[2];
        Buffer[Offset++] = bytes[3];
    }

    public override void Write(double value) {
        EnsureCapacity(Offset + sizeof(double));
        var bytes = BitConverter.GetBytes(value);
        Buffer[Offset++] = bytes[0];
        Buffer[Offset++] = bytes[1];
        Buffer[Offset++] = bytes[2];
        Buffer[Offset++] = bytes[3];
        Buffer[Offset++] = bytes[4];
        Buffer[Offset++] = bytes[5];
        Buffer[Offset++] = bytes[6];
        Buffer[Offset++] = bytes[7];
    }
}

public sealed class GetBytesArrayCopyWriter : AbstractBinaryWriter {
    public GetBytesArrayCopyWriter(int size) : base(size) { }

    public override void Write(byte value) {
        EnsureCapacity(Offset + sizeof(byte));
        var arr = new[] {
            value,
        };
        Array.Copy(arr, 0, Buffer, Offset, arr.Length);
        Offset += sizeof(byte);
    }

    public override void Write(Memory<byte> value) {
        throw new NotImplementedException();
    }

    public override void Write(byte[] value) {
        throw new NotImplementedException();
    }

    public override void Write(char[] value) {
        throw new NotImplementedException();
    }

    public override void Write(short value) {
        EnsureCapacity(Offset + sizeof(short));
        var bytes = BitConverter.GetBytes(value);
        Array.Copy(bytes, 0, Buffer, Offset, bytes.Length);
        Offset += sizeof(short);
    }

    public override void Write(ushort value) {
        EnsureCapacity(Offset + sizeof(ushort));
        var bytes = BitConverter.GetBytes(value);
        Array.Copy(bytes, 0, Buffer, Offset, bytes.Length);
        Offset += sizeof(ushort);
    }

    public override void Write(Int24 value) {
        EnsureCapacity(Offset + Int24.SIZE);
        var bytes = GmBitConverter.GetBytes(value);
        Array.Copy(bytes, 0, Buffer, Offset, bytes.Length);
        Offset += Int24.SIZE;
    }

    public override void Write(UInt24 value) {
        EnsureCapacity(Offset + UInt24.SIZE);
        var bytes = GmBitConverter.GetBytes(value);
        Array.Copy(bytes, 0, Buffer, Offset, bytes.Length);
        Offset += UInt24.SIZE;
    }

    public override void Write(int value) {
        EnsureCapacity(Offset + sizeof(int));
        var bytes = BitConverter.GetBytes(value);
        Array.Copy(bytes, 0, Buffer, Offset, bytes.Length);
        Offset += sizeof(int);
    }

    public override void Write(uint value) {
        EnsureCapacity(Offset + sizeof(uint));
        var bytes = BitConverter.GetBytes(value);
        Array.Copy(bytes, 0, Buffer, Offset, bytes.Length);
        Offset += sizeof(uint);
    }

    public override void Write(long value) {
        EnsureCapacity(Offset + sizeof(long));
        var bytes = BitConverter.GetBytes(value);
        Array.Copy(bytes, 0, Buffer, Offset, bytes.Length);
        Offset += sizeof(long);
    }

    public override void Write(ulong value) {
        EnsureCapacity(Offset + sizeof(ulong));
        var bytes = BitConverter.GetBytes(value);
        Array.Copy(bytes, 0, Buffer, Offset, bytes.Length);
        Offset += sizeof(ulong);
    }

    public override void Write(float value) {
        EnsureCapacity(Offset + sizeof(float));
        var bytes = BitConverter.GetBytes(value);
        Array.Copy(bytes, 0, Buffer, Offset, bytes.Length);
        Offset += sizeof(float);
    }

    public override void Write(double value) {
        EnsureCapacity(Offset + sizeof(double));
        var bytes = BitConverter.GetBytes(value);
        Array.Copy(bytes, 0, Buffer, Offset, bytes.Length);
        Offset += sizeof(double);
    }
}

public sealed class GetBytesBufferBlockCopyWriter : AbstractBinaryWriter {
    public GetBytesBufferBlockCopyWriter(int size) : base(size) { }

    public override void Write(byte value) {
        EnsureCapacity(Offset + sizeof(byte));
        var arr = new[] {
            value,
        };
        System.Buffer.BlockCopy(arr, 0, Buffer, Offset, sizeof(byte));
        Offset += sizeof(byte);
    }

    public override void Write(Memory<byte> value) {
        throw new NotImplementedException();
    }

    public override void Write(byte[] value) {
        throw new NotImplementedException();
    }

    public override void Write(char[] value) {
        throw new NotImplementedException();
    }

    public override void Write(short value) {
        EnsureCapacity(Offset + sizeof(short));
        var bytes = BitConverter.GetBytes(value);
        System.Buffer.BlockCopy(bytes, 0, Buffer, Offset, sizeof(short));
        Offset += sizeof(short);
    }

    public override void Write(ushort value) {
        EnsureCapacity(Offset + sizeof(ushort));
        var bytes = BitConverter.GetBytes(value);
        System.Buffer.BlockCopy(bytes, 0, Buffer, Offset, sizeof(ushort));
        Offset += sizeof(ushort);
    }

    public override void Write(Int24 value) {
        EnsureCapacity(Offset + Int24.SIZE);
        var bytes = GmBitConverter.GetBytes(value);
        System.Buffer.BlockCopy(bytes, 0, Buffer, Offset, Int24.SIZE);
        Offset += Int24.SIZE;
    }

    public override void Write(UInt24 value) {
        EnsureCapacity(Offset + UInt24.SIZE);
        var bytes = GmBitConverter.GetBytes(value);
        System.Buffer.BlockCopy(bytes, 0, Buffer, Offset, UInt24.SIZE);
        Offset += UInt24.SIZE;
    }

    public override void Write(int value) {
        EnsureCapacity(Offset + sizeof(int));
        var bytes = BitConverter.GetBytes(value);
        System.Buffer.BlockCopy(bytes, 0, Buffer, Offset, sizeof(int));
        Offset += sizeof(int);
    }

    public override void Write(uint value) {
        EnsureCapacity(Offset + sizeof(uint));
        var bytes = BitConverter.GetBytes(value);
        System.Buffer.BlockCopy(bytes, 0, Buffer, Offset, sizeof(uint));
        Offset += sizeof(uint);
    }

    public override void Write(long value) {
        EnsureCapacity(Offset + sizeof(long));
        var bytes = BitConverter.GetBytes(value);
        System.Buffer.BlockCopy(bytes, 0, Buffer, Offset, sizeof(long));
        Offset += sizeof(long);
    }

    public override void Write(ulong value) {
        EnsureCapacity(Offset + sizeof(ulong));
        var bytes = BitConverter.GetBytes(value);
        System.Buffer.BlockCopy(bytes, 0, Buffer, Offset, sizeof(ulong));
        Offset += sizeof(ulong);
    }

    public override void Write(float value) {
        EnsureCapacity(Offset + sizeof(float));
        var bytes = BitConverter.GetBytes(value);
        System.Buffer.BlockCopy(bytes, 0, Buffer, Offset, sizeof(float));
        Offset += sizeof(float);
    }

    public override void Write(double value) {
        EnsureCapacity(Offset + sizeof(double));
        var bytes = BitConverter.GetBytes(value);
        System.Buffer.BlockCopy(bytes, 0, Buffer, Offset, sizeof(double));
        Offset += sizeof(double);
    }
}

public sealed class TryWriteBytesArrayCopyWriter : AbstractBinaryWriter {
    public TryWriteBytesArrayCopyWriter(int size) : base(size) { }

    public override void Write(byte value) {
        EnsureCapacity(Offset + sizeof(byte));
        var buf = new[] {
            value,
        };
        Array.Copy(buf, 0, Buffer, Offset, buf.Length);
        Offset += sizeof(byte);
    }

    public override void Write(Memory<byte> value) {
        throw new NotImplementedException();
    }

    public override void Write(byte[] value) {
        throw new NotImplementedException();
    }

    public override void Write(char[] value) {
        throw new NotImplementedException();
    }

    public override void Write(short value) {
        EnsureCapacity(Offset + sizeof(short));
        var buf = new byte[sizeof(short)];
        BitConverter.TryWriteBytes(buf, value);
        Array.Copy(buf, 0, Buffer, Offset, buf.Length);
        Offset += sizeof(short);
    }

    public override void Write(ushort value) {
        EnsureCapacity(Offset + sizeof(ushort));
        var buf = new byte[sizeof(ushort)];
        BitConverter.TryWriteBytes(buf, value);
        Array.Copy(buf, 0, Buffer, Offset, buf.Length);
        Offset += sizeof(ushort);
    }

    public override void Write(Int24 value) {
        EnsureCapacity(Offset + Int24.SIZE);
        var buf = new byte[Int24.SIZE];
        GmBitConverter.TryWriteBytes(buf, value);
        Array.Copy(buf, 0, Buffer, Offset, buf.Length);
        Offset += Int24.SIZE;
    }

    public override void Write(UInt24 value) {
        EnsureCapacity(Offset + UInt24.SIZE);
        var buf = new byte[UInt24.SIZE];
        GmBitConverter.TryWriteBytes(buf, value);
        Array.Copy(buf, 0, Buffer, Offset, buf.Length);
        Offset += UInt24.SIZE;
    }

    public override void Write(int value) {
        EnsureCapacity(Offset + sizeof(int));
        var buf = new byte[sizeof(int)];
        BitConverter.TryWriteBytes(buf, value);
        Array.Copy(buf, 0, Buffer, Offset, buf.Length);
        Offset += sizeof(int);
    }

    public override void Write(uint value) {
        EnsureCapacity(Offset + sizeof(uint));
        var buf = new byte[sizeof(uint)];
        BitConverter.TryWriteBytes(buf, value);
        Array.Copy(buf, 0, Buffer, Offset, buf.Length);
        Offset += sizeof(uint);
    }

    public override void Write(long value) {
        EnsureCapacity(Offset + sizeof(long));
        var buf = new byte[sizeof(long)];
        BitConverter.TryWriteBytes(buf, value);
        Array.Copy(buf, 0, Buffer, Offset, buf.Length);
        Offset += sizeof(long);
    }

    public override void Write(ulong value) {
        EnsureCapacity(Offset + sizeof(ulong));
        var buf = new byte[sizeof(ulong)];
        BitConverter.TryWriteBytes(buf, value);
        Array.Copy(buf, 0, Buffer, Offset, buf.Length);
        Offset += sizeof(ulong);
    }

    public override void Write(float value) {
        EnsureCapacity(Offset + sizeof(float));
        var buf = new byte[sizeof(float)];
        BitConverter.TryWriteBytes(buf, value);
        Array.Copy(buf, 0, Buffer, Offset, buf.Length);
        Offset += sizeof(float);
    }

    public override void Write(double value) {
        EnsureCapacity(Offset + sizeof(double));
        var buf = new byte[sizeof(double)];
        BitConverter.TryWriteBytes(buf, value);
        Array.Copy(buf, 0, Buffer, Offset, buf.Length);
        Offset += sizeof(double);
    }
}

public sealed class TryWriteBytesBlockCopyWriter : AbstractBinaryWriter {
    public TryWriteBytesBlockCopyWriter(int size) : base(size) { }

    public override void Write(byte value) {
        EnsureCapacity(Offset + sizeof(byte));
        var buf = new[] {
            value,
        };
        System.Buffer.BlockCopy(buf, 0, Buffer, Offset, buf.Length);
        Offset += sizeof(byte);
    }

    public override void Write(Memory<byte> value) {
        throw new NotImplementedException();
    }

    public override void Write(byte[] value) {
        throw new NotImplementedException();
    }

    public override void Write(char[] value) {
        throw new NotImplementedException();
    }

    public override void Write(short value) {
        EnsureCapacity(Offset + sizeof(short));
        var buf = new byte[sizeof(short)];
        BitConverter.TryWriteBytes(buf, value);
        System.Buffer.BlockCopy(buf, 0, Buffer, Offset, buf.Length);
        Offset += sizeof(short);
    }

    public override void Write(ushort value) {
        EnsureCapacity(Offset + sizeof(ushort));
        var buf = new byte[sizeof(ushort)];
        BitConverter.TryWriteBytes(buf, value);
        System.Buffer.BlockCopy(buf, 0, Buffer, Offset, buf.Length);
        Offset += sizeof(ushort);
    }

    public override void Write(Int24 value) {
        EnsureCapacity(Offset + Int24.SIZE);
        var buf = new byte[Int24.SIZE];
        GmBitConverter.TryWriteBytes(buf, value);
        System.Buffer.BlockCopy(buf, 0, Buffer, Offset, buf.Length);
        Offset += Int24.SIZE;
    }

    public override void Write(UInt24 value) {
        EnsureCapacity(Offset + UInt24.SIZE);
        var buf = new byte[UInt24.SIZE];
        GmBitConverter.TryWriteBytes(buf, value);
        System.Buffer.BlockCopy(buf, 0, Buffer, Offset, buf.Length);
        Offset += UInt24.SIZE;
    }

    public override void Write(int value) {
        EnsureCapacity(Offset + sizeof(int));
        var buf = new byte[sizeof(int)];
        BitConverter.TryWriteBytes(buf, value);
        System.Buffer.BlockCopy(buf, 0, Buffer, Offset, buf.Length);
        Offset += sizeof(int);
    }

    public override void Write(uint value) {
        EnsureCapacity(Offset + sizeof(uint));
        var buf = new byte[sizeof(uint)];
        BitConverter.TryWriteBytes(buf, value);
        System.Buffer.BlockCopy(buf, 0, Buffer, Offset, buf.Length);
        Offset += sizeof(uint);
    }

    public override void Write(long value) {
        EnsureCapacity(Offset + sizeof(long));
        var buf = new byte[sizeof(long)];
        BitConverter.TryWriteBytes(buf, value);
        System.Buffer.BlockCopy(buf, 0, Buffer, Offset, buf.Length);
        Offset += sizeof(long);
    }

    public override void Write(ulong value) {
        EnsureCapacity(Offset + sizeof(ulong));
        var buf = new byte[sizeof(ulong)];
        BitConverter.TryWriteBytes(buf, value);
        System.Buffer.BlockCopy(buf, 0, Buffer, Offset, buf.Length);
        Offset += sizeof(ulong);
    }

    public override void Write(float value) {
        EnsureCapacity(Offset + sizeof(float));
        var buf = new byte[sizeof(float)];
        BitConverter.TryWriteBytes(buf, value);
        System.Buffer.BlockCopy(buf, 0, Buffer, Offset, buf.Length);
        Offset += sizeof(float);
    }

    public override void Write(double value) {
        EnsureCapacity(Offset + sizeof(double));
        var buf = new byte[sizeof(double)];
        BitConverter.TryWriteBytes(buf, value);
        System.Buffer.BlockCopy(buf, 0, Buffer, Offset, buf.Length);
        Offset += sizeof(double);
    }
}

public sealed class UnsafeAsPointerWriter : AbstractBinaryWriter {
    public UnsafeAsPointerWriter(int size) : base(size) { }

    public override void Write(byte value) {
        EnsureCapacity(Offset + sizeof(byte));
        ref var b = ref Buffer[Offset];
        Unsafe.As<byte, byte>(ref b) = value;
        Offset += sizeof(byte);
    }

    public override void Write(Memory<byte> value) {
        throw new NotImplementedException();
    }

    public override void Write(byte[] value) {
        throw new NotImplementedException();
    }

    public override void Write(char[] value) {
        throw new NotImplementedException();
    }

    public override void Write(short value) {
        EnsureCapacity(Offset + sizeof(short));
        ref var b = ref Buffer[Offset];
        Unsafe.As<byte, short>(ref b) = value;
        Offset += sizeof(short);
    }

    public override void Write(ushort value) {
        EnsureCapacity(Offset + sizeof(ushort));
        ref var b = ref Buffer[Offset];
        Unsafe.As<byte, ushort>(ref b) = value;
        Offset += sizeof(ushort);
    }

    public override void Write(Int24 value) {
        EnsureCapacity(Offset + Int24.SIZE);
        ref var b = ref Buffer[Offset];
        Unsafe.As<byte, Int24>(ref b) = value;
        Offset += Int24.SIZE;
    }

    public override void Write(UInt24 value) {
        EnsureCapacity(Offset + UInt24.SIZE);
        ref var b = ref Buffer[Offset];
        Unsafe.As<byte, UInt24>(ref b) = value;
        Offset += UInt24.SIZE;
    }

    public override void Write(int value) {
        EnsureCapacity(Offset + sizeof(int));
        ref var b = ref Buffer[Offset];
        Unsafe.As<byte, int>(ref b) = value;
        Offset += sizeof(int);
    }

    public override void Write(uint value) {
        EnsureCapacity(Offset + sizeof(uint));
        ref var b = ref Buffer[Offset];
        Unsafe.As<byte, uint>(ref b) = value;
        Offset += sizeof(uint);
    }

    public override void Write(long value) {
        EnsureCapacity(Offset + sizeof(long));
        ref var b = ref Buffer[Offset];
        Unsafe.As<byte, long>(ref b) = value;
        Offset += sizeof(long);
    }

    public override void Write(ulong value) {
        EnsureCapacity(Offset + sizeof(ulong));
        ref var b = ref Buffer[Offset];
        Unsafe.As<byte, ulong>(ref b) = value;
        Offset += sizeof(ulong);
    }

    public override void Write(float value) {
        EnsureCapacity(Offset + sizeof(float));
        ref var b = ref Buffer[Offset];
        Unsafe.As<byte, float>(ref b) = value;
        Offset += sizeof(float);
    }

    public override void Write(double value) {
        EnsureCapacity(Offset + sizeof(double));
        ref var b = ref Buffer[Offset];
        Unsafe.As<byte, double>(ref b) = value;
        Offset += sizeof(double);
    }
}

public sealed class BufferRegionCopyToWriter : AbstractBinaryWriter {
    public BufferRegionCopyToWriter(int size) : base(size) { }

    public override void Write(byte value) {
        throw new NotImplementedException();
    }

    public override void Write(Memory<byte> value) {
        EnsureCapacity(Offset + value.Length);
        value.CopyTo(Buffer.AsMemory().Slice(Offset, value.Length));
        Offset += value.Length;
    }

    public override void Write(byte[] value) {
        throw new NotImplementedException();
    }

    public override void Write(char[] value) {
        throw new NotImplementedException();
    }

    public override void Write(short value) {
        throw new NotImplementedException();
    }

    public override void Write(ushort value) {
        throw new NotImplementedException();
    }

    public override void Write(Int24 value) {
        throw new NotImplementedException();
    }

    public override void Write(UInt24 value) {
        throw new NotImplementedException();
    }

    public override void Write(int value) {
        throw new NotImplementedException();
    }

    public override void Write(uint value) {
        throw new NotImplementedException();
    }

    public override void Write(long value) {
        throw new NotImplementedException();
    }

    public override void Write(ulong value) {
        throw new NotImplementedException();
    }

    public override void Write(float value) {
        throw new NotImplementedException();
    }

    public override void Write(double value) {
        throw new NotImplementedException();
    }
}

public sealed class BufferRegionUnsafePointerWriter : AbstractBinaryWriter {
    public BufferRegionUnsafePointerWriter(int size) : base(size) { }

    public override void Write(byte value) {
        throw new NotImplementedException();
    }

    public override unsafe void Write(Memory<byte> value) {
        EnsureCapacity(Offset + value.Length);

        fixed (byte* bufferPtr = Buffer)
        fixed (byte* valuePtr = value.Span) {
            var destPtr = bufferPtr + Offset;
            var srcPtr = valuePtr;

            var length = value.Length;
            for (var i = 0; i < length; i++)
                *destPtr++ = *srcPtr++;

            Offset += length;
        }
    }

    public override void Write(byte[] value) {
        throw new NotImplementedException();
    }

    public override void Write(char[] value) {
        throw new NotImplementedException();
    }

    public override void Write(short value) {
        throw new NotImplementedException();
    }

    public override void Write(ushort value) {
        throw new NotImplementedException();
    }

    public override void Write(Int24 value) {
        throw new NotImplementedException();
    }

    public override void Write(UInt24 value) {
        throw new NotImplementedException();
    }

    public override void Write(int value) {
        throw new NotImplementedException();
    }

    public override void Write(uint value) {
        throw new NotImplementedException();
    }

    public override void Write(long value) {
        throw new NotImplementedException();
    }

    public override void Write(ulong value) {
        throw new NotImplementedException();
    }

    public override void Write(float value) {
        throw new NotImplementedException();
    }

    public override void Write(double value) {
        throw new NotImplementedException();
    }
}

public sealed class BufferRegionUnsafeSimdWriter : AbstractBinaryWriter {
    public BufferRegionUnsafeSimdWriter(int size) : base(size) { }

    public override void Write(byte value) {
        throw new NotImplementedException();
    }

    public override unsafe void Write(Memory<byte> value) {
        fixed (byte* pBuffer = Buffer)
        fixed (byte* pSpan = value.Span) {
            var pValue = pSpan;
            var length = value.Length;
            var remaining = length;
            var offset = Offset;

            while (remaining >= 64) {
                Sse2.LoadVector128(pValue).Store(pBuffer + offset);
                Sse2.LoadVector128(pValue + 16).Store(pBuffer + offset + 16);
                Sse2.LoadVector128(pValue + 32).Store(pBuffer + offset + 32);
                Sse2.LoadVector128(pValue + 48).Store(pBuffer + offset + 48);
                
                offset += 64;
                pValue += 64;
                remaining -= 64;
            }

            while (remaining > 0) {
                pBuffer[offset] = *pValue;
                offset++;
                pValue++;
                remaining--;
            }

            Offset = offset;
        }
    }

    public override void Write(byte[] value) {
        throw new NotImplementedException();
    }

    public override void Write(char[] value) {
        throw new NotImplementedException();
    }

    public override void Write(short value) {
        throw new NotImplementedException();
    }

    public override void Write(ushort value) {
        throw new NotImplementedException();
    }

    public override void Write(Int24 value) {
        throw new NotImplementedException();
    }

    public override void Write(UInt24 value) {
        throw new NotImplementedException();
    }

    public override void Write(int value) {
        throw new NotImplementedException();
    }

    public override void Write(uint value) {
        throw new NotImplementedException();
    }

    public override void Write(long value) {
        throw new NotImplementedException();
    }

    public override void Write(ulong value) {
        throw new NotImplementedException();
    }

    public override void Write(float value) {
        throw new NotImplementedException();
    }

    public override void Write(double value) {
        throw new NotImplementedException();
    }
}

[TestFixture]
public static class WriteTests {
    [Test]
    public static void TestWriteBasicData() {
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
        IBinaryWriter one = new DirectBufferWriter(size);
        IBinaryWriter two = new GetBytesArrayCopyWriter(size);
        IBinaryWriter three = new GetBytesBufferBlockCopyWriter(size);
        IBinaryWriter four = new TryWriteBytesArrayCopyWriter(size);
        IBinaryWriter five = new TryWriteBytesBlockCopyWriter(size);
        IBinaryWriter six = new UnsafeAsPointerWriter(size);

        var rand = new Random();

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

            one.Write(b, wide: false);
            one.Write(b, wide: true);
            one.Write(u8);
            one.Write(i16);
            one.Write(u16);
            one.Write(i24);
            one.Write(u24);
            one.Write(i32);
            one.Write(u32);
            one.Write(i64);
            one.Write(u64);
            one.Write(f32);
            one.Write(f64);

            two.Write(b, wide: false);
            two.Write(b, wide: true);
            two.Write(u8);
            two.Write(i16);
            two.Write(u16);
            two.Write(i24);
            two.Write(u24);
            two.Write(i32);
            two.Write(u32);
            two.Write(i64);
            two.Write(u64);
            two.Write(f32);
            two.Write(f64);

            three.Write(b, wide: false);
            three.Write(b, wide: true);
            three.Write(u8);
            three.Write(i16);
            three.Write(u16);
            three.Write(i24);
            three.Write(u24);
            three.Write(i32);
            three.Write(u32);
            three.Write(i64);
            three.Write(u64);
            three.Write(f32);
            three.Write(f64);

            four.Write(b, wide: false);
            four.Write(b, wide: true);
            four.Write(u8);
            four.Write(i16);
            four.Write(u16);
            four.Write(i24);
            four.Write(u24);
            four.Write(i32);
            four.Write(u32);
            four.Write(i64);
            four.Write(u64);
            four.Write(f32);
            four.Write(f64);

            five.Write(b, wide: false);
            five.Write(b, wide: true);
            five.Write(u8);
            five.Write(i16);
            five.Write(u16);
            five.Write(i24);
            five.Write(u24);
            five.Write(i32);
            five.Write(u32);
            five.Write(i64);
            five.Write(u64);
            five.Write(f32);
            five.Write(f64);

            six.Write(b, wide: false);
            six.Write(b, wide: true);
            six.Write(u8);
            six.Write(i16);
            six.Write(u16);
            six.Write(i24);
            six.Write(u24);
            six.Write(i32);
            six.Write(u32);
            six.Write(i64);
            six.Write(u64);
            six.Write(f32);
            six.Write(f64);
        }

        var oneBytes = one.Buffer;
        var twoBytes = two.Buffer;
        var threeBytes = three.Buffer;
        var fourBytes = four.Buffer;
        var fiveBytes = five.Buffer;
        var sixBytes = six.Buffer;
        Assert.Multiple(() => {
            Assert.That(twoBytes, Is.EqualTo(oneBytes));
            Assert.That(threeBytes, Is.EqualTo(oneBytes));
            Assert.That(fourBytes, Is.EqualTo(oneBytes));
            Assert.That(fiveBytes, Is.EqualTo(oneBytes));
            Assert.That(sixBytes, Is.EqualTo(oneBytes));
        });
    }
    
    [Test]
    public static void TestWriteBufferRegion() {
        const int size = sizeof(byte) * 1000 * 100;
        IBinaryWriter one = new BufferRegionCopyToWriter(size);
        IBinaryWriter two = new BufferRegionUnsafePointerWriter(size);
        IBinaryWriter three = new BufferRegionUnsafeSimdWriter(size);

        var rand = new Random();

        for (var i = 0; i < 1000; i++) {
            var buffer = new byte[100];
            rand.NextBytes(buffer);
            var memory = buffer.AsMemory();

            one.Write(memory);
            two.Write(memory);
            three.Write(memory);
        }
        
        var oneBytes = one.Buffer;
        var twoBytes = two.Buffer;
        var threeBytes = three.Buffer;
        Assert.Multiple(() => {
            Assert.That(twoBytes, Is.EqualTo(oneBytes));
            Assert.That(threeBytes, Is.EqualTo(oneBytes));
        });
    }
}
