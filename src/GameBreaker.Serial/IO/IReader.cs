using System;

namespace GameBreaker.Serial.IO;

public interface IReader : IPositionable,
                           IEncodable,
                           IDisposable {
    byte ReadByte();

    byte[] ReadBytes(int count);

    bool ReadBool(bool wide = false);

    char ReadChar();

    char[] ReadChars(int count);

    short ReadInt16();

    ushort ReadUInt16();

    int ReadInt24();

    uint ReadUInt24();

    int ReadInt32();

    uint ReadUInt32();

    long ReadInt64();

    ulong ReadUInt64();

    float ReadSingle();

    double ReadDouble();

    void ReadSerializable(ISerializable serializable);
}
