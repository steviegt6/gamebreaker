using System;
using System.Diagnostics;

namespace GameBreaker.Serial.IO;

public abstract class Serializable<T> : ISerializable<T> {
    public T? Value { get; set; }

    protected Serializable() { }

    protected Serializable(T? value) {
        Value = value;
    }

    public abstract void Serialize(IWriter writer);

    public abstract void Deserialize(IReader reader);
}

public class SerializableByte : Serializable<byte> {
    public SerializableByte() { }

    public SerializableByte(byte value) : base(value) { }

    public override void Serialize(IWriter writer) {
        writer.Write(Value);
    }

    public override void Deserialize(IReader reader) {
        Value = reader.ReadByte();
    }
}

public class SerializableByteArray : Serializable<byte[]> {
    public SerializableByteArray() { }

    public SerializableByteArray(byte[] value) : base(value) { }
    
    public SerializableByteArray(int length) : base(new byte[length]) { }

    public override void Serialize(IWriter writer) {
        Debug.Assert(Value is not null);
        writer.Write(Value);
    }

    public override void Deserialize(IReader reader) {
        Debug.Assert(Value is not null);
        Value = reader.ReadBytes(Value.Length);
    }
}

public class SerializableChar : Serializable<char> {
    public SerializableChar() { }

    public SerializableChar(char value) : base(value) { }

    public override void Serialize(IWriter writer) {
        writer.Write(Value);
    }

    public override void Deserialize(IReader reader) {
        Value = reader.ReadChar();
    }
}

public class SerializableCharArray : Serializable<char[]> {
    public SerializableCharArray() { }

    public SerializableCharArray(char[] value) : base(value) { }

    public SerializableCharArray(int length) : base(new char[length]) { }

    public override void Serialize(IWriter writer) {
        Debug.Assert(Value is not null);
        writer.Write(Value);
    }

    public override void Deserialize(IReader reader) {
        Debug.Assert(Value is not null);
        Value = reader.ReadChars(Value.Length);
    }
}

public class SerializableBool : Serializable<bool> {
    public SerializableBool() { }

    public SerializableBool(bool value) : base(value) { }

    public override void Serialize(IWriter writer) {
        writer.Write(Value);
    }

    public override void Deserialize(IReader reader) {
        Value = reader.ReadBool();
    }
}

public class SerializableWideBool : Serializable<bool> {
    public SerializableWideBool() { }

    public SerializableWideBool(bool value) : base(value) { }

    public override void Serialize(IWriter writer) {
        writer.Write(Value, true);
    }

    public override void Deserialize(IReader reader) {
        Value = reader.ReadBool(true);
    }
}

public class SerializableShort : Serializable<short> {
    public SerializableShort() { }

    public SerializableShort(short value) : base(value) { }

    public override void Serialize(IWriter writer) {
        writer.Write(Value);
    }

    public override void Deserialize(IReader reader) {
        Value = reader.ReadInt16();
    }
}

public class SerializableUShort : Serializable<ushort> {
    public SerializableUShort() { }

    public SerializableUShort(ushort value) : base(value) { }

    public override void Serialize(IWriter writer) {
        writer.Write(Value);
    }

    public override void Deserialize(IReader reader) {
        Value = reader.ReadUInt16();
    }
}

public class SerializableInt24 : Serializable<int> {
    public SerializableInt24() { }

    public SerializableInt24(int value) : base(value) { }

    public override void Serialize(IWriter writer) {
        writer.WriteInt24(Value);
    }

    public override void Deserialize(IReader reader) {
        Value = reader.ReadInt24();
    }
}

public class SerializableUInt24 : Serializable<uint> {
    public SerializableUInt24() { }

    public SerializableUInt24(uint value) : base(value) { }

    public override void Serialize(IWriter writer) {
        writer.WriteUInt24(Value);
    }

    public override void Deserialize(IReader reader) {
        Value = reader.ReadUInt24();
    }
}

public class SerializableInt : Serializable<int> {
    public SerializableInt() { }

    public SerializableInt(int value) : base(value) { }

    public override void Serialize(IWriter writer) {
        writer.Write(Value);
    }

    public override void Deserialize(IReader reader) {
        Value = reader.ReadInt32();
    }
}

public class SerializableUInt : Serializable<uint> {
    public SerializableUInt() { }

    public SerializableUInt(uint value) : base(value) { }

    public override void Serialize(IWriter writer) {
        writer.Write(Value);
    }

    public override void Deserialize(IReader reader) {
        Value = reader.ReadUInt32();
    }
}

public class SerializableLong : Serializable<long> {
    public SerializableLong() { }

    public SerializableLong(long value) : base(value) { }

    public override void Serialize(IWriter writer) {
        writer.Write(Value);
    }

    public override void Deserialize(IReader reader) {
        Value = reader.ReadInt64();
    }
}

public class SerializableULong : Serializable<ulong> {
    public SerializableULong() { }

    public SerializableULong(ulong value) : base(value) { }

    public override void Serialize(IWriter writer) {
        writer.Write(Value);
    }

    public override void Deserialize(IReader reader) {
        Value = reader.ReadUInt64();
    }
}

public class SerializableFloat : Serializable<float> {
    public SerializableFloat() { }

    public SerializableFloat(float value) : base(value) { }

    public override void Serialize(IWriter writer) {
        writer.Write(Value);
    }

    public override void Deserialize(IReader reader) {
        Value = reader.ReadSingle();
    }
}

public class SerializableDouble : Serializable<double> {
    public SerializableDouble() { }

    public SerializableDouble(double value) : base(value) { }

    public override void Serialize(IWriter writer) {
        writer.Write(Value);
    }

    public override void Deserialize(IReader reader) {
        Value = reader.ReadDouble();
    }
}

public class SerializableGmString : Serializable<string> {
    private int ugh;
    
    public override void Serialize(IWriter writer) {
        writer.Write(ugh);
    }

    public override void Deserialize(IReader reader) {
        ugh = reader.ReadInt32();
        Value = "TODO";
    }
}

public class SerializableGuid : Serializable<Guid> {
    public override void Serialize(IWriter writer) {
        writer.Write(Value.ToByteArray());
    }

    public override void Deserialize(IReader reader) {
        Value = new Guid(reader.ReadBytes(16));
    }
}
