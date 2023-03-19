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
#region GetBytes
    /// <summary>
    ///     Returns the specified Boolean value as a byte array, treated as
    ///     either wide or narrow depending on the value of
    ///     <paramref name="wide"/>.
    /// </summary>
    /// <param name="value">A Boolean value.</param>
    /// <param name="wide">Whether this Boolean is wide or narrow.</param>
    /// <returns>
    ///     A byte array with length 1 or 4, depending on
    ///     <paramref name="wide"/>.
    /// </returns>
    public static byte[] GetBytes(bool value, bool wide) {
        return wide
            ? BitConverter.GetBytes(value ? 1 : 0)
            : BitConverter.GetBytes(value);
    }

    /// <summary>
    ///     Returns the specified 24-bit signed integer value as an array of
    ///     bytes.
    /// </summary>
    /// <param name="value">The number to convert.</param>
    /// <returns>An array of bytes with length 3.</returns>
    public static byte[] GetBytes(Int24 value) {
        var bytes = new byte[Marshal.SizeOf<Int24>()];
        Unsafe.As<byte, Int24>(ref bytes[0]) = value;
        return bytes;
    }
    
    /// <summary>
    ///     Returns the specified 24-bit unsigned integer value as an array of
    ///     bytes.
    /// </summary>
    /// <param name="value">The number to convert.</param>
    /// <returns>An array of bytes with length 3.</returns>
    public static byte[] GetBytes(UInt24 value) {
        var bytes = new byte[Marshal.SizeOf<Int24>()];
        Unsafe.As<byte, UInt24>(ref bytes[0]) = value;
        return bytes;
    }
#endregion

#region ToX
    /// <summary>
    ///     Returns a 24-bit signed integer converted from three bytes at a
    ///     specified position in a byte array.
    /// </summary>
    /// <param name="value">An array of bytes.</param>
    /// <param name="startIndex">
    ///     The starting position within <paramref name="value"/>.
    /// </param>
    /// <returns>
    ///     A 24-bit signed integer formed by four bytes beginning at
    ///     <paramref name="startIndex"/>.
    /// </returns>
    public static Int24 ToInt24(byte[] value, int startIndex) {
        return Unsafe.As<byte, Int24>(ref value[startIndex]);
    }

    /// <summary>
    ///     Returns a 24-bit unsigned integer converted from three bytes at a
    ///     specified position in a byte array.
    /// </summary>
    /// <param name="value">An array of bytes.</param>
    /// <param name="startIndex">
    ///     The starting position within <paramref name="value"/>.
    /// </param>
    /// <returns>
    ///     A 24-bit unsigned integer formed by four bytes beginning at
    ///     <paramref name="startIndex"/>.
    /// </returns>
    public static UInt24 ToUInt24(byte[] value, int startIndex) {
        return Unsafe.As<byte, UInt24>(ref value[startIndex]);
    }
#endregion
}
