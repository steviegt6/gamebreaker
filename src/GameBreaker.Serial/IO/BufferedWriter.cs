using System;
using System.Text;

namespace GameBreaker.Serial.IO;

public class BufferedWriter : IWriter {
#region IPositionable Impl
    public int Position { get; set; }

    public int Length { get; protected set; }
#endregion

#region IEncodable Impl
    public Encoding Encoding { get; }
#endregion

    protected byte[] Buffer { get; private set; }

    public BufferedWriter(
        byte[] buffer,
        int position = 0,
        Encoding? encoding = null
    ) {
        Encoding = encoding ?? new UTF8Encoding(false); // don't emit BOM

        // In concept, position can be non-zero if a parent stream or something
        // is at a different position.
        Position = position;
        Length = buffer.Length;

        Buffer = buffer;
    }

    protected virtual void Resize(int size) {
        while (size > Buffer.Length) {
            var buf = Buffer;
            Array.Resize(ref buf, buf.Length * 2);
            Buffer = buf;
        }

        if (Length < size)
            Length = size;
    }

    protected virtual void ResizeRelative(int size) {
        Resize(Position + size);
    }

#region IWriter Impl
    public virtual void Write(byte value) {
        ResizeRelative(1);
        Buffer[Position++] = value;
    }

    public virtual void Write(byte[] buffer) {
        ResizeRelative(buffer.Length);
        Array.Copy(buffer, 0, Buffer, Position, buffer.Length);
        Position += buffer.Length;
    }

    public virtual void Write(char value) {
        ResizeRelative(1);
        Buffer[Position++] = Convert.ToByte(value);
    }

    public virtual void Write(char[] buffer) {
        ResizeRelative(buffer.Length);
        foreach (var c in buffer)
            Buffer[Position++] = Convert.ToByte(c);
    }

    public virtual void Write(bool value, bool wide = false) {
        if (wide) {
            ResizeRelative(4);
            Write(value ? 1 : 0);
        }
        else {
            ResizeRelative(1);
            Write((byte)(value ? 1 : 0));
        }
    }

    public virtual void Write(short value) {
        ResizeRelative(2);
        var bytes = BitConverter.GetBytes(value);
        Array.Copy(bytes, 0, Buffer, Position, bytes.Length);
        Position += bytes.Length;
    }

    public virtual void Write(ushort value) {
        ResizeRelative(2);
        var bytes = BitConverter.GetBytes(value);
        Array.Copy(bytes, 0, Buffer, Position, bytes.Length);
        Position += bytes.Length;
    }

    public virtual void WriteInt24(int value) {
        ResizeRelative(3);
        Buffer[Position++] = (byte)(value & 0xFF);
        Buffer[Position++] = (byte)((value >> 8) & 0xFF);
        Buffer[Position++] = (byte)((value >> 16) & 0xFF);
    }

    public virtual void WriteUInt24(uint value) {
        ResizeRelative(3);
        Buffer[Position++] = (byte)(value & 0xFF);
        Buffer[Position++] = (byte)((value >> 8) & 0xFF);
        Buffer[Position++] = (byte)((value >> 16) & 0xFF);
    }

    public virtual void Write(int value) {
        ResizeRelative(4);
        var bytes = BitConverter.GetBytes(value);
        Array.Copy(bytes, 0, Buffer, Position, bytes.Length);
        Position += bytes.Length;
    }

    public virtual void Write(uint value) {
        ResizeRelative(4);
        var bytes = BitConverter.GetBytes(value);
        Array.Copy(bytes, 0, Buffer, Position, bytes.Length);
        Position += bytes.Length;
    }

    public virtual void Write(long value) {
        ResizeRelative(8);
        var bytes = BitConverter.GetBytes(value);
        Array.Copy(bytes, 0, Buffer, Position, bytes.Length);
        Position += bytes.Length;
    }

    public virtual void Write(ulong value) {
        ResizeRelative(8);
        var bytes = BitConverter.GetBytes(value);
        Array.Copy(bytes, 0, Buffer, Position, bytes.Length);
        Position += bytes.Length;
    }

    public virtual void Write(float value) {
        ResizeRelative(4);
        var bytes = BitConverter.GetBytes(value);
        Array.Copy(bytes, 0, Buffer, Position, bytes.Length);
        Position += bytes.Length;
    }

    public virtual void Write(double value) {
        ResizeRelative(8);
        var bytes = BitConverter.GetBytes(value);
        Array.Copy(bytes, 0, Buffer, Position, bytes.Length);
        Position += bytes.Length;
    }

    public virtual void Write(ISerializable serializable) {
        serializable.Serialize(this);
    }
#endregion

#region IDisposable Impl
    protected virtual void Dispose(bool disposing) {
        if (disposing) { }
    }

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
#endregion
}
