using System;
using System.Collections.Generic;

namespace GameBreaker.Serial.IO;

public interface IReader : IPositionable,
                           IEncodable,
                           IDisposable {
    Dictionary<int, IPointerSerializable> Pointers { get; }

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

    IPointerSerializable? ReadPointer(
        int? ptr,
        Func<IPointerSerializable> fallback
    );

    IPointerSerializable<T>? ReadPointer<T>(
        int? ptr,
        Func<IPointerSerializable<T>> fallback
    );

    IPointerSerializable? ReadPointerObject(
        int ptr,
        Func<IPointerSerializable> fallback
    );

    IPointerSerializable<T>? ReadPointerObject<T>(
        int ptr,
        Func<IPointerSerializable<T>> fallback
    );
}
