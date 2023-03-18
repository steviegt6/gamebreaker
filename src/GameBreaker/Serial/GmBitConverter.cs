using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using GameBreaker.Serial.Numerics;

namespace GameBreaker.Serial;

/// <summary>
///     Converts base GameMaker data types to an array of bytes, and an array of
///     bytes to base data types.
/// </summary>
public static class GmBitConverter {
    public static byte[] GetBytes(bool value, bool wide) {
        return wide
            ? BitConverter.GetBytes(value ? 1 : 0)
            : BitConverter.GetBytes(value);
    }

    public static byte[] GetBytes(Int24 value) {
        var bytes = new byte[Marshal.SizeOf<Int24>()];
        Unsafe.As<byte, Int24>(ref bytes[0]) = value;
        return bytes;
    }

    public static byte[] GetBytes(UInt24 value) {
        var bytes = new byte[Marshal.SizeOf<Int24>()];
        Unsafe.As<byte, UInt24>(ref bytes[0]) = value;
        return bytes;
    }

    public static Int24 ToInt24(byte[] value, int startIndex) {
        return Unsafe.As<byte, Int24>(ref value[startIndex]);
    }

    public static UInt24 ToUInt24(byte[] value, int startIndex) {
        return Unsafe.As<byte, UInt24>(ref value[startIndex]);
    }
}
