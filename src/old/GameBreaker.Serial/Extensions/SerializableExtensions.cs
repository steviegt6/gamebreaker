using GameBreaker.Serial.IO;

namespace GameBreaker.Serial.Extensions;

public static class SerializableExtensions {
    public static ISerializable<byte> AsSerializable(this byte v) {
        return new SerializableByte(v);
    }

    public static ISerializable<byte[]> AsSerializable(this byte[] v) {
        return new SerializableByteArray(v);
    }

    public static ISerializable<char> AsSerializable(this char v) {
        return new SerializableChar(v);
    }

    public static ISerializable<char[]> AsSerializable(this char[] v) {
        return new SerializableCharArray(v);
    }

    public static ISerializable<bool> AsSerializable(this bool v, bool wide) {
        return wide ? new SerializableWideBool(v) : new SerializableBool(v);
    }

    public static ISerializable<short> AsSerializable(this short v) {
        return new SerializableShort(v);
    }

    public static ISerializable<ushort> AsSerializable(this ushort v) {
        return new SerializableUShort(v);
    }

    public static ISerializable<int> AsSerializableInt24(this int v) {
        return new SerializableInt24(v);
    }

    public static ISerializable<uint> AsSerializableUInt24(this uint v) {
        return new SerializableUInt24(v);
    }

    public static ISerializable<int> AsSerializable(this int v) {
        return new SerializableInt(v);
    }

    public static ISerializable<uint> AsSerializable(this uint v) {
        return new SerializableUInt(v);
    }

    public static ISerializable<long> AsSerializable(this long v) {
        return new SerializableLong(v);
    }

    public static ISerializable<ulong> AsSerializable(this ulong v) {
        return new SerializableULong(v);
    }

    public static ISerializable<float> AsSerializable(this float v) {
        return new SerializableFloat(v);
    }

    public static ISerializable<double> AsSerializable(this double v) {
        return new SerializableDouble(v);
    }
}
