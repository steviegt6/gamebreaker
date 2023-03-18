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
using GameBreaker.Serial.Numerics;

[assembly: InternalsVisibleTo("GameBreaker.Tests")]

namespace GameBreaker.Serial;

/// <summary>
///     Essentially functions like <see cref="BitConverter"/>, but deals
///     exclusively as little endian regardless of the system's endianness.
/// </summary>
public static class LittleEndianBitConverter {
    internal static bool IsLittleEndian = BitConverter.IsLittleEndian;

    /// <summary>
    ///     Reads a value of type <typeparamref name="T"/> from a byte array
    ///     starting at the specified index.
    /// </summary>
    /// <param name="value">The byte array to read.</param>
    /// <param name="startIndex">The start index in the array.</param>
    /// <typeparam name="T">The type to read to.</typeparam>
    /// <returns>The read value of the given type.</returns>
    private static T ReadUnaligned<T>(byte[] value, int startIndex)
        where T : struct {
        // If we're on a little endian system, we can just read the value
        // directly the same way that BitConverter does.
        if (IsLittleEndian)
            return Unsafe.ReadUnaligned<T>(ref value[startIndex]);

        // If we aren't so lucky...
        Unsafe.SkipInit(out T res);
        Unsafe.CopyBlockUnaligned(
            ref Unsafe.As<T, byte>(ref res),
            ref value[startIndex],
            (uint) Unsafe.SizeOf<T>()
        );
        return res;
    }

#region GetXBytes
    public static byte[] GetBooleanBytes(bool value, bool wide) {
        if (wide)
            return GetInt32Bytes(value ? 1 : 0);

        return new[] {
            (byte) (value ? 1 : 0),
        };
    }

    public static byte[] GetInt16Bytes(short value) {
        return new[] {
            (byte) (value & 0xFF),
            (byte) ((value >> 8) & 0xFF),
        };
    }

    public static byte[] GetUInt16Bytes(ushort value) {
        return new[] {
            (byte) (value & 0xFF),
            (byte) ((value >> 8) & 0xFF),
        };
    }

    public static byte[] GetInt24Bytes(int value) {
        return new[] {
            (byte) (value & 0xFF),
            (byte) ((value >> 8) & 0xFF),
            (byte) ((value >> 16) & 0xFF),
        };
    }

    public static byte[] GetUInt24Bytes(uint value) {
        return new[] {
            (byte) (value & 0xFF),
            (byte) ((value >> 8) & 0xFF),
            (byte) ((value >> 16) & 0xFF),
        };
    }

    public static byte[] GetInt32Bytes(int value) {
        return new[] {
            (byte) (value & 0xFF),
            (byte) ((value >> 8) & 0xFF),
            (byte) ((value >> 16) & 0xFF),
            (byte) ((value >> 24) & 0xFF),
        };
    }

    public static byte[] GetUInt32Bytes(uint value) {
        return new[] {
            (byte) (value & 0xFF),
            (byte) ((value >> 8) & 0xFF),
            (byte) ((value >> 16) & 0xFF),
            (byte) ((value >> 24) & 0xFF),
        };
    }

    public static byte[] GetInt64Bytes(long value) {
        return new[] {
            (byte) (value & 0xFF),
            (byte) ((value >> 8) & 0xFF),
            (byte) ((value >> 16) & 0xFF),
            (byte) ((value >> 24) & 0xFF),
            (byte) ((value >> 32) & 0xFF),
            (byte) ((value >> 40) & 0xFF),
            (byte) ((value >> 48) & 0xFF),
            (byte) ((value >> 56) & 0xFF),
        };
    }

    public static byte[] GetUInt64Bytes(ulong value) {
        return new[] {
            (byte) (value & 0xFF),
            (byte) ((value >> 8) & 0xFF),
            (byte) ((value >> 16) & 0xFF),
            (byte) ((value >> 24) & 0xFF),
            (byte) ((value >> 32) & 0xFF),
            (byte) ((value >> 40) & 0xFF),
            (byte) ((value >> 48) & 0xFF),
            (byte) ((value >> 56) & 0xFF),
        };
    }

    public static byte[] GetSingleBytes(float value) {
        return GetInt32Bytes(
            BitConverter.ToInt32(BitConverter.GetBytes(value), 0)
        );
    }

    public static byte[] GetDoubleBytes(double value) {
        return GetInt64Bytes(
            BitConverter.ToInt64(BitConverter.GetBytes(value), 0)
        );
    }
#endregion

#region ToX
    public static short ToInt16(byte[] value, int startIndex) {
        return ReadUnaligned<short>(value, startIndex);
    }

    public static ushort ToUInt16(byte[] value, int startIndex) {
        return ReadUnaligned<ushort>(value, startIndex);
    }

    // TODO: Create Int24 and UInt24 types to benefit from optimizations.
    public static Int24 ToInt24(byte[] value, int startIndex) {
        return ReadUnaligned<Int24>(value, startIndex);
    }

    public static UInt24 ToUInt24(byte[] value, int startIndex) {
        return ReadUnaligned<UInt24>(value, startIndex);
    }

    public static int ToInt32(byte[] value, int startIndex) {
        return ReadUnaligned<int>(value, startIndex);
    }

    public static uint ToUInt32(byte[] value, int startIndex) {
        return ReadUnaligned<uint>(value, startIndex);
    }

    public static long ToInt64(byte[] value, int startIndex) {
        return ReadUnaligned<long>(value, startIndex);
    }

    public static ulong ToUInt64(byte[] value, int startIndex) {
        return ReadUnaligned<ulong>(value, startIndex);
    }

    public static float ToSingle(byte[] value, int startIndex) {
        return ReadUnaligned<float>(value, startIndex);
    }

    public static double ToDouble(byte[] value, int startIndex) {
        return ReadUnaligned<double>(value, startIndex);
    }
#endregion

#region ToX (ref startIndex)
    public static short ToInt16(byte[] value, ref int startIndex) {
        var result = ToInt16(value, startIndex);
        startIndex += 2;
        return result;
    }

    public static ushort ToUInt16(byte[] value, ref int startIndex) {
        var result = ToUInt16(value, startIndex);
        startIndex += 2;
        return result;
    }

    public static int ToInt24(byte[] value, ref int startIndex) {
        var result = ToInt24(value, startIndex);
        startIndex += 3;
        return result;
    }

    public static uint ToUInt24(byte[] value, ref int startIndex) {
        var result = ToUInt24(value, startIndex);
        startIndex += 3;
        return result;
    }

    public static int ToInt32(byte[] value, ref int startIndex) {
        var result = ToInt32(value, startIndex);
        startIndex += 4;
        return result;
    }

    public static uint ToUInt32(byte[] value, ref int startIndex) {
        var result = ToUInt32(value, startIndex);
        startIndex += 4;
        return result;
    }

    public static long ToInt64(byte[] value, ref int startIndex) {
        var result = ToInt64(value, startIndex);
        startIndex += 8;
        return result;
    }

    public static ulong ToUInt64(byte[] value, ref int startIndex) {
        var result = ToUInt64(value, startIndex);
        startIndex += 8;
        return result;
    }

    public static float ToSingle(byte[] value, ref int startIndex) {
        var result = ToSingle(value, startIndex);
        startIndex += 4;
        return result;
    }

    public static double ToDouble(byte[] value, ref int startIndex) {
        var result = ToDouble(value, startIndex);
        startIndex += 8;
        return result;
    }
#endregion
}
