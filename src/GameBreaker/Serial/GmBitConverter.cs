/*
 * Copyright (c) 2023 Tomat & GameBreaker Contributors
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

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
        var bytes = new byte[Int24.SIZE];
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
        var bytes = new byte[UInt24.SIZE];
        Unsafe.As<byte, UInt24>(ref bytes[0]) = value;
        return bytes;
    }
#endregion

#region ToX (byte[])
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

#region ToX (ReadOnlySpan<byte>)
    /// <summary>
    ///     Converts a read-only byte span into a 24-bit signed integer.
    /// </summary>
    /// <param name="value">
    ///     A read-only span containing the bytes to convert.
    /// </param>
    /// <returns>
    ///     A 24-bit signed integer representing the converted bytes.
    /// </returns>
    public static Int24 ToInt24(ReadOnlySpan<byte> value) {
        return Unsafe.ReadUnaligned<Int24>(
            ref MemoryMarshal.GetReference(value)
        );
    }

    /// <summary>
    ///     Converts a read-only byte span into a 24-bit unsigned integer.
    /// </summary>
    /// <param name="value">
    ///     A read-only span containing the bytes to convert.
    /// </param>
    /// <returns>
    ///     A 24-bit unsigned integer representing the converted bytes.
    /// </returns>
    public static UInt24 ToUInt24(ReadOnlySpan<byte> value) {
        return Unsafe.ReadUnaligned<UInt24>(
            ref MemoryMarshal.GetReference(value)
        );
    }
#endregion

#region ToX (byte[]) (ref startIndex)
    // Can't <inheritdoc /> because we added a parameter. XML summaries are hot
    // garbage...
    /// <summary>
    ///     Returns a Boolean value converted from two bytes at a specified
    ///     position in a byte array.
    /// </summary>
    /// <param name="value">A byte array.</param>
    /// <param name="startIndex">
    ///     The index of the byte within <paramref name="value"/>.
    /// </param>
    /// <param name="wide">Whether this Boolean is wide or narrow.</param>
    /// <returns>
    ///     <see langword="true"/> if the byte at<paramref name="startIndex"/>
    ///     is nonzero; otherwise <see langword="false"/>.
    /// </returns>
    public static bool ToBoolean(byte[] value, ref int startIndex, bool wide) {
        var result = wide
            ? BitConverter.ToInt32(value, startIndex) != 0
            : BitConverter.ToBoolean(value, startIndex);
        startIndex += wide ? sizeof(int) : sizeof(bool);
        return result;
    }

    /// <inheritdoc cref="BitConverter.ToInt16(byte[],int)"/>
    public static short ToInt16(byte[] value, ref int startIndex) {
        var result = BitConverter.ToInt16(value, startIndex);
        startIndex += sizeof(short);
        return result;
    }

    /// <inheritdoc cref="BitConverter.ToInt16(byte[],int)"/>
    public static ushort ToUInt16(byte[] value, ref int startIndex) {
        var result = BitConverter.ToUInt16(value, startIndex);
        startIndex += sizeof(ushort);
        return result;
    }

    /// <inheritdoc cref="BitConverter.ToInt16(byte[],int)"/>
    public static Int24 ToInt24(byte[] value, ref int startIndex) {
        var result = Unsafe.As<byte, Int24>(ref value[startIndex]);
        startIndex += Int24.SIZE;
        return result;
    }

    /// <inheritdoc cref="BitConverter.ToInt16(byte[],int)"/>
    public static UInt24 ToUInt24(byte[] value, ref int startIndex) {
        var result = Unsafe.As<byte, UInt24>(ref value[startIndex]);
        startIndex += UInt24.SIZE;
        return result;
    }

    /// <inheritdoc cref="BitConverter.ToInt16(byte[],int)"/>
    public static int ToInt32(byte[] value, ref int startIndex) {
        var result = BitConverter.ToInt32(value, startIndex);
        startIndex += sizeof(int);
        return result;
    }

    /// <inheritdoc cref="BitConverter.ToInt16(byte[],int)"/>
    public static uint ToUInt32(byte[] value, ref int startIndex) {
        var result = BitConverter.ToUInt32(value, startIndex);
        startIndex += sizeof(uint);
        return result;
    }

    /// <inheritdoc cref="BitConverter.ToInt16(byte[],int)"/>
    public static long ToInt64(byte[] value, ref int startIndex) {
        var result = BitConverter.ToInt64(value, startIndex);
        startIndex += sizeof(long);
        return result;
    }

    /// <inheritdoc cref="BitConverter.ToInt16(byte[],int)"/>
    public static ulong ToUInt64(byte[] value, ref int startIndex) {
        var result = BitConverter.ToUInt64(value, startIndex);
        startIndex += sizeof(ulong);
        return result;
    }

    /// <inheritdoc cref="BitConverter.ToInt16(byte[],int)"/>
    public static float ToSingle(byte[] value, ref int startIndex) {
        var result = BitConverter.ToSingle(value, startIndex);
        startIndex += sizeof(float);
        return result;
    }

    /// <inheritdoc cref="BitConverter.ToInt16(byte[],int)"/>
    public static double ToDouble(byte[] value, ref int startIndex) {
        var result = BitConverter.ToDouble(value, startIndex);
        startIndex += sizeof(double);
        return result;
    }
#endregion
}
