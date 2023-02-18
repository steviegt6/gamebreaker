using System;
using System.Diagnostics;
using System.Text;

namespace GameBreaker.Serial.IO;

public class BufferedReader : IReader {
#region IPositionable Impl
    public int Position { get; set; }

    public int Length { get; }
#endregion

#region IEncodable Impl
    public Encoding Encoding { get; }
#endregion

    protected byte[] Buffer { get; }

    public BufferedReader(
        byte[] buffer,
        int position,
        Encoding? encoding
    ) {
        Encoding = encoding ?? new UTF8Encoding(false); // don't emit BOM

        // In concept, position can be non-zero if a parent stream or something
        // is at a different position.
        Position = position;
        Length = buffer.Length;

        Buffer = buffer;
    }

#region IReader Impl
    public virtual byte ReadByte() {
        Debug.Assert(Position < Length);
        return Buffer[Position++];
    }

    public virtual byte[] ReadBytes(int count) {
        Debug.Assert(Position + count <= Length);
        var bytes = new byte[count];
        for (var i = 0; i < count; i++)
            bytes[i] = ReadByte();

        return bytes;
    }

    public virtual bool ReadBool(bool wide = false) {
        Debug.Assert(wide ? Position + 4 <= Length : Position < Length);
        return (wide ? ReadInt32() : ReadByte()) != 0;
    }

    public virtual char ReadChar() {
        Debug.Assert(Position < Length);
        return Convert.ToChar(ReadByte());
    }

    public virtual char[] ReadChars(int count) {
        Debug.Assert(Position + count <= Length);
        var chars = new char[count];
        for (var i = 0; i < count; i++)
            chars[i] = ReadChar();

        return chars;
    }

    public virtual short ReadInt16() {
        const int amount = 2;
        Debug.Assert(Position + amount <= Length);

        var val = BitConverter.ToInt16(Buffer, Position);
        Position += amount;
        return val;
    }

    public virtual ushort ReadUInt16() {
        const int amount = 2;
        Debug.Assert(Position + amount <= Length);

        var val = BitConverter.ToUInt16(Buffer, Position);
        Position += amount;
        return val;
    }

    public virtual int ReadInt24() {
        Debug.Assert(Position + 3 <= Length);
        return ReadByte() | ReadByte() << 8 | ReadByte() << 16;
    }

    public virtual uint ReadUInt24() {
        Debug.Assert(Position + 3 <= Length);
        return (uint)(ReadByte() | ReadByte() << 8 | ReadByte() << 16);
    }

    public virtual int ReadInt32() {
        const int amount = 4;
        Debug.Assert(Position + amount <= Length);

        var val = BitConverter.ToInt32(Buffer, Position);
        Position += amount;
        return val;
    }

    public virtual uint ReadUInt32() {
        const int amount = 4;
        Debug.Assert(Position + amount <= Length);

        var val = BitConverter.ToUInt32(Buffer, Position);
        Position += amount;
        return val;
    }

    public virtual long ReadInt64() {
        const int amount = 8;
        Debug.Assert(Position + amount <= Length);

        var val = BitConverter.ToInt64(Buffer, Position);
        Position += amount;
        return val;
    }

    public virtual ulong ReadUInt64() {
        const int amount = 8;
        Debug.Assert(Position + amount <= Length);

        var val = BitConverter.ToUInt64(Buffer, Position);
        Position += amount;
        return val;
    }

    public virtual float ReadSingle() {
        const int amount = 4;
        Debug.Assert(Position + amount <= Length);

        var val = BitConverter.ToSingle(Buffer, Position);
        Position += amount;
        return val;
    }

    public virtual double ReadDouble() {
        const int amount = 8;
        Debug.Assert(Position + amount <= Length);

        var val = BitConverter.ToDouble(Buffer, Position);
        Position += amount;
        return val;
    }

    public void ReadSerializable(ISerializable serializable) {
        serializable.Deserialize(this);
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
