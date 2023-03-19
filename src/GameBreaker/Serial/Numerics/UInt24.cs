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
///     Represents a 24-bit unsigned integer.
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = SIZE)]
public struct UInt24 {
    public const int SIZE = 3;
    
    public static readonly UInt24 Zero = new(0);
    
    [FieldOffset(0)]
    private readonly byte b1;

    [FieldOffset(1)]
    private readonly byte b2;

    [FieldOffset(2)]
    private readonly byte b3;

    // ReSharper disable once InconsistentNaming
    /// <summary>
    ///    Represents the smallest possible value of an UInt24. This field is
    ///    constant.
    /// </summary>
    public const uint MinValue = 0;

    // ReSharper disable once InconsistentNaming
    /// <summary>
    ///    Represents the largest possible value of an UInt24. This field is
    ///    constant.
    /// </summary>
    public const uint MaxValue = 16777215;

    public UInt24(uint value) {
        if (value is < MinValue or > MaxValue)
            throw new ArgumentOutOfRangeException(nameof(value));

        b1 = (byte) (value & 0xFF);
        b2 = (byte) ((value >> 8) & 0xFF);
        b3 = (byte) ((value >> 16) & 0xFF);
    }

    public static implicit operator uint(UInt24 value) {
        return value.b1 | (uint) (value.b2 << 8) | (uint) (value.b3 << 16);
    }

    public static implicit operator UInt24(uint value) {
        return new UInt24(value);
    }
}
