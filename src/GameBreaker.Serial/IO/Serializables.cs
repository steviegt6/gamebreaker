using System;
using System.Diagnostics;
using GameBreaker.Serial.IO.IFF;

namespace GameBreaker.Serial.IO;

public abstract class Serializable<T> : ISerializable<T> {
    public T? Value { get; set; }

    protected Serializable() { }

    protected Serializable(T? value) {
        Value = value;
    }

    public abstract void Serialize(
        IWriter writer,
        IChunkData chunk,
        IffFile iffFile
    );

    public abstract void Deserialize(
        IReader reader,
        IChunkData chunk,
        IffFile iffFile
    );
}

public abstract class PointerSerializable<T> : IPointerSerializable<T> {
    public virtual int PointerOffset => 0;

    public int Pointer { get; set; }

    public T? Value { get; set; }

    public abstract void Serialize(IWriter writer);

    public abstract void Deserialize(IReader reader);
}

public class SerializableByte : Serializable<byte> {
    public SerializableByte() { }

    public SerializableByte(byte value) : base(value) { }

    public override void Serialize(
        IWriter writer,
        IChunkData chunk,
        IffFile iffFile
    ) {
        writer.Write(Value);
    }

    public override void Deserialize(
        IReader reader,
        IChunkData chunk,
        IffFile iffFile
    ) {
        Value = reader.ReadByte();
    }
}

public class SerializableByteArray : Serializable<byte[]> {
    public SerializableByteArray() { }

    public SerializableByteArray(byte[] value) : base(value) { }

    public SerializableByteArray(int length) : base(new byte[length]) { }

    public override void Serialize(
        IWriter writer,
        IChunkData chunk,
        IffFile iffFile
    ) {
        Debug.Assert(Value is not null);
        writer.Write(Value);
    }

    public override void Deserialize(
        IReader reader,
        IChunkData chunk,
        IffFile iffFile
    ) {
        Debug.Assert(Value is not null);
        Value = reader.ReadBytes(Value.Length);
    }
}

public class SerializableChar : Serializable<char> {
    public SerializableChar() { }

    public SerializableChar(char value) : base(value) { }

    public override void Serialize(
        IWriter writer,
        IChunkData chunk,
        IffFile iffFile
    ) {
        writer.Write(Value);
    }

    public override void Deserialize(
        IReader reader,
        IChunkData chunk,
        IffFile iffFile
    ) {
        Value = reader.ReadChar();
    }
}

public class SerializableCharArray : Serializable<char[]> {
    public SerializableCharArray() { }

    public SerializableCharArray(char[] value) : base(value) { }

    public SerializableCharArray(int length) : base(new char[length]) { }

    public override void Serialize(
        IWriter writer,
        IChunkData chunk,
        IffFile iffFile
    ) {
        Debug.Assert(Value is not null);
        writer.Write(Value);
    }

    public override void Deserialize(
        IReader reader,
        IChunkData chunk,
        IffFile iffFile
    ) {
        Debug.Assert(Value is not null);
        Value = reader.ReadChars(Value.Length);
    }
}

public class SerializableBool : Serializable<bool> {
    public SerializableBool() { }

    public SerializableBool(bool value) : base(value) { }

    public override void Serialize(
        IWriter writer,
        IChunkData chunk,
        IffFile iffFile
    ) {
        writer.Write(Value);
    }

    public override void Deserialize(
        IReader reader,
        IChunkData chunk,
        IffFile iffFile
    ) {
        Value = reader.ReadBool();
    }
}

public class SerializableWideBool : Serializable<bool> {
    public SerializableWideBool() { }

    public SerializableWideBool(bool value) : base(value) { }

    public override void Serialize(
        IWriter writer,
        IChunkData chunk,
        IffFile iffFile
    ) {
        writer.Write(Value, true);
    }

    public override void Deserialize(
        IReader reader,
        IChunkData chunk,
        IffFile iffFile
    ) {
        Value = reader.ReadBool(true);
    }
}

public class SerializableShort : Serializable<short> {
    public SerializableShort() { }

    public SerializableShort(short value) : base(value) { }

    public override void Serialize(
        IWriter writer,
        IChunkData chunk,
        IffFile iffFile
    ) {
        writer.Write(Value);
    }

    public override void Deserialize(
        IReader reader,
        IChunkData chunk,
        IffFile iffFile
    ) {
        Value = reader.ReadInt16();
    }
}

public class SerializableUShort : Serializable<ushort> {
    public SerializableUShort() { }

    public SerializableUShort(ushort value) : base(value) { }

    public override void Serialize(
        IWriter writer,
        IChunkData chunk,
        IffFile iffFile
    ) {
        writer.Write(Value);
    }

    public override void Deserialize(
        IReader reader,
        IChunkData chunk,
        IffFile iffFile
    ) {
        Value = reader.ReadUInt16();
    }
}

public class SerializableInt24 : Serializable<int> {
    public SerializableInt24() { }

    public SerializableInt24(int value) : base(value) { }

    public override void Serialize(
        IWriter writer,
        IChunkData chunk,
        IffFile iffFile
    ) {
        writer.WriteInt24(Value);
    }

    public override void Deserialize(
        IReader reader,
        IChunkData chunk,
        IffFile iffFile
    ) {
        Value = reader.ReadInt24();
    }
}

public class SerializableUInt24 : Serializable<uint> {
    public SerializableUInt24() { }

    public SerializableUInt24(uint value) : base(value) { }

    public override void Serialize(
        IWriter writer,
        IChunkData chunk,
        IffFile iffFile
    ) {
        writer.WriteUInt24(Value);
    }

    public override void Deserialize(
        IReader reader,
        IChunkData chunk,
        IffFile iffFile
    ) {
        Value = reader.ReadUInt24();
    }
}

public class SerializableInt : Serializable<int> {
    public SerializableInt() { }

    public SerializableInt(int value) : base(value) { }

    public override void Serialize(
        IWriter writer,
        IChunkData chunk,
        IffFile iffFile
    ) {
        writer.Write(Value);
    }

    public override void Deserialize(
        IReader reader,
        IChunkData chunk,
        IffFile iffFile
    ) {
        Value = reader.ReadInt32();
    }
}

public class SerializableUInt : Serializable<uint> {
    public SerializableUInt() { }

    public SerializableUInt(uint value) : base(value) { }

    public override void Serialize(
        IWriter writer,
        IChunkData chunk,
        IffFile iffFile
    ) {
        writer.Write(Value);
    }

    public override void Deserialize(
        IReader reader,
        IChunkData chunk,
        IffFile iffFile
    ) {
        Value = reader.ReadUInt32();
    }
}

public class SerializableLong : Serializable<long> {
    public SerializableLong() { }

    public SerializableLong(long value) : base(value) { }

    public override void Serialize(
        IWriter writer,
        IChunkData chunk,
        IffFile iffFile
    ) {
        writer.Write(Value);
    }

    public override void Deserialize(
        IReader reader,
        IChunkData chunk,
        IffFile iffFile
    ) {
        Value = reader.ReadInt64();
    }
}

public class SerializableULong : Serializable<ulong> {
    public SerializableULong() { }

    public SerializableULong(ulong value) : base(value) { }

    public override void Serialize(
        IWriter writer,
        IChunkData chunk,
        IffFile iffFile
    ) {
        writer.Write(Value);
    }

    public override void Deserialize(
        IReader reader,
        IChunkData chunk,
        IffFile iffFile
    ) {
        Value = reader.ReadUInt64();
    }
}

public class SerializableFloat : Serializable<float> {
    public SerializableFloat() { }

    public SerializableFloat(float value) : base(value) { }

    public override void Serialize(
        IWriter writer,
        IChunkData chunk,
        IffFile iffFile
    ) {
        writer.Write(Value);
    }

    public override void Deserialize(
        IReader reader,
        IChunkData chunk,
        IffFile iffFile
    ) {
        Value = reader.ReadSingle();
    }
}

public class SerializableDouble : Serializable<double> {
    public SerializableDouble() { }

    public SerializableDouble(double value) : base(value) { }

    public override void Serialize(
        IWriter writer,
        IChunkData chunk,
        IffFile iffFile
    ) {
        writer.Write(Value);
    }

    public override void Deserialize(
        IReader reader,
        IChunkData chunk,
        IffFile iffFile
    ) {
        Value = reader.ReadDouble();
    }
}

public class SerializableGuid : Serializable<Guid> {
    public override void Serialize(
        IWriter writer,
        IChunkData chunk,
        IffFile iffFile
    ) {
        writer.Write(Value.ToByteArray());
    }

    public override void Deserialize(
        IReader reader,
        IChunkData chunk,
        IffFile iffFile
    ) {
        Value = new Guid(reader.ReadBytes(16));
    }
}

public class SerializableGmString : PointerSerializable<string> {
    public override int PointerOffset => 4;

    public override void Serialize(IWriter writer) {
        Debug.Assert(Value is not null);
        
        var length = writer.Encoding.GetByteCount(Value);
        writer.Write(length);
        writer.Write(writer.Encoding.GetBytes(Value, 0, Value.Length));
        writer.Write((byte)0); // Null terminator.
    }

    public override void Deserialize(IReader reader) {
        var expectedLength = reader.ReadInt32();
        var startPos = reader.Position;

        while (reader.ReadByte() != 0) { }

        reader.Position -= 1; // Go back to before the null terminator.
        var realLength = reader.Position - startPos;
        reader.Position = startPos;
        Value = reader.Encoding.GetString(reader.ReadBytes(realLength));
        reader.Position++; // Skip the null terminator.
        
        // TODO: warn if expectedLength != realLength
        Debug.Assert(expectedLength == realLength);
    }
}
