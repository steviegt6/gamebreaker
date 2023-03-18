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
using System.Text;

namespace GameBreaker.Util; 

/// <summary>
///     Represents a positionable object dealing with a buffer that performs
///     binary reading operations.
/// </summary>
public interface IBinaryReader : IPositionable, IEncodable {
    /// <summary>
    ///     Reads a byte from the buffer.
    /// </summary>
    /// <returns>The read byte.</returns>
    byte ReadByte();

    /// <summary>
    ///     Reads a boolean from the buffer.
    /// </summary>
    /// <param name="wide">
    ///     Whether this boolean is wide (8 bits) as opposed to narrow (1 bit).
    /// </param>
    /// <returns>The read boolean.</returns>
    bool ReadBoolean(bool wide);

    /// <summary>
    ///     Reads characters into a string from the buffer. Encoded using the
    ///     <see cref="Encoding"/> provided by
    ///     <see cref="IEncodable.Encoding"/>.
    /// </summary>
    /// <param name="count">The amount of bytes to read into characters.</param>
    /// <returns>A string made up of the decoded character array.</returns>
    string ReadChars(int count);
    
    /// <summary>
    ///     Reads bytes into a <see cref="BufferRegion"/>, which is a wrapper
    ///     around <see cref="Memory{T}"/> that holds a slice of the buffer in
    ///     memory.
    /// </summary>
    /// <param name="count">The amount of bytes to read.</param>
    /// <returns>A slice of the buffer of the given length.</returns>
    BufferRegion ReadBytes(int count);

    /// <summary>
    ///     Reads a 16-bit signed integer from the buffer.
    /// </summary>
    /// <returns>The read integer.</returns>
    short ReadInt16();
    
    /// <summary>
    ///     Reads a 16-bit unsigned integer from the buffer.
    /// </summary>
    /// <returns>The read integer.</returns>
    ushort ReadUInt16();
    
    /// <summary>
    ///     Reads a 24-bit signed integer from the buffer, represented as a
    ///     32-bit integer.
    /// </summary>
    /// <returns>The read integer.</returns>
    int ReadInt24();
    
    /// <summary>
    ///     Reads a 24-bit unsigned integer from the buffer, represented as a
    ///     32-bit integer.
    /// </summary>
    /// <returns>The read integer.</returns>
    uint ReadUInt24();
    
    /// <summary>
    ///     Reads a 32-bit signed integer from the buffer.
    /// </summary>
    /// <returns>The read integer.</returns>
    int ReadInt32();
    
    /// <summary>
    ///     Reads a 32-bit unsigned integer from the buffer.
    /// </summary>
    /// <returns>The read integer.</returns>
    uint ReadUInt32();
    
    /// <summary>
    ///     Reads a 64-bit signed integer from the buffer.
    /// </summary>
    /// <returns>The read integer.</returns>
    long ReadInt64();
    
    /// <summary>
    ///     Reads a 64-bit unsigned integer from the buffer.
    /// </summary>
    /// <returns>The read integer.</returns>
    ulong ReadUInt64();
    
    /// <summary>
    ///     Reads a 32-bit floating point number from the buffer.
    /// </summary>
    /// <returns>The read integer.</returns>
    float ReadSingle();
    
    /// <summary>
    ///     Reads a 64-bit floating point number from the buffer.
    /// </summary>
    /// <returns>The read integer.</returns>
    double ReadDouble();
}
