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

namespace GameBreaker.Serial; 

/// <summary>
///     Represents a positionable object dealing with a buffer, exposed through
///     direct access to a byte array, as well as <see cref="Offset"/> and
///     <see cref="Length"/> properties.
/// </summary>
public interface IPositionable {
    /// <summary>
    ///     The offset (position) in the buffer.
    /// </summary>
    int Offset { get; set; }

    /// <summary>
    ///     The length of the buffer. This may not necessarily fall in line with
    ///     the length of the allocated <see cref="Buffer"/> array, as the
    ///     length is not constant when the <see cref="Buffer"/> is being
    ///     written to, for example.
    /// </summary>
    int Length { get; }

    /// <summary>
    ///     The byte array that is being read from and/or written to.
    /// </summary>
    byte[] Buffer { get; }
}

/// <summary>
///     Extension methods for <see cref="IPositionable"/>.
/// </summary>
public static class PositionableExtensions {
    /// <summary>
    ///     Pads the <see cref="IPositionable.Offset"/> to the next multiple of
    ///     <paramref name="alignment"/>.
    /// </summary>
    /// <param name="pos">The positionable to pad.</param>
    /// <param name="alignment">The alignment to pad to.</param>
    public static void Pad(this IPositionable pos, int alignment)
    {
        if (pos.Offset % alignment != 0)
            pos.Offset += alignment - (pos.Offset % alignment);
    }
}
