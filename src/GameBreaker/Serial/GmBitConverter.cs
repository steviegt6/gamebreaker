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
