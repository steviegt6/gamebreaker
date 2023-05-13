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
using System.Runtime.InteropServices;

namespace GameBreaker.Serial.Numerics;

/// <summary>
///     Represents a 24-bit signed integer.
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct Int24 {
    public static readonly int SIZE = Marshal.SizeOf<Int24>();

    /// <summary>
    ///     Represents the number zero.
    /// </summary>
    public static readonly Int24 Zero = new(0);

    private readonly byte b0;
    private readonly byte b1;
    private readonly byte b2;

    // ReSharper disable once InconsistentNaming
    /// <summary>
    ///     Represents the smallest possible value of <see cref="Int24"/>. This
    ///     field is constant.
    /// </summary>
    public const int MinValue = -8388608; // -2^23

    // ReSharper disable once InconsistentNaming
    /// <summary>
    ///     Represents the largest possible value of <see cref="Int24"/>. This
    ///     field is constant.
    /// </summary>
    public const int MaxValue = 8388607; // 2^23 - 1

    public Int24(int value) {
        b0 = (byte) (value & 0xFF);
        b1 = (byte) ((value >> 8) & 0xFF);
        b2 = (byte) ((value >> 16) & 0xFF);
    }

    public static implicit operator int(Int24 value) {
        var res = value.b0 | (value.b1 << 8) | (value.b2 << 16);

        // Sign extend
        if ((res & 0x800000) != 0)
            return res | unchecked((int) 0xFF000000);

        return res;
    }

    public static implicit operator Int24(int value) {
        return new Int24(value);
    }
}
