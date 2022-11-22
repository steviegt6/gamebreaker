// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using GameBreaker.DataTypes;

namespace GameBreaker.Serialization.Abstractions;

/// <summary>
///     Describes an object capable of serializing data.
/// </summary>
public interface IDataSerializer : IPositionable
{
    /// <summary>
    ///     Writes a byte.
    /// </summary>
    void Write(byte value);

    /// <summary>
    ///     Writes an array of bytes.
    /// </summary>
    void Write(byte[] value);

    /// <summary>
    ///     Writes a boolean.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="wide">Whether the boolean is wide (four bytes) or not (one byte).</param>
    void Write(bool value, bool wide = true);

    /// <summary>
    ///     Writes a character.
    /// </summary>
    void Write(char value);

    /// <summary>
    ///     Writes an array of characters.
    /// </summary>
    void Write(char[] value);

    /// <summary>
    ///     Writes a 16-bit signed integer.
    /// </summary>
    void Write(short value);

    
    /// <summary>
    ///     Writes a 16-bit unsigned integer.
    /// </summary>
    void Write(ushort value);

    /// <summary>
    ///     Writes a 23-bit signed integer.
    /// </summary>
    void Write(Int24 value);

    /// <summary>
    ///     Writes a 24-bit unsigned integer.
    /// </summary>
    void Write(UInt24 value);

    /// <summary>
    ///     Writes a 32-bit signed integer.
    /// </summary>
    void Write(int value);

    /// <summary>
    ///     Writes a 32-bit unsigned integer.
    /// </summary>
    void Write(uint value);

    /// <summary>
    ///     Writes a 64-bit signed integer.
    /// </summary>
    void Write(long value);

    /// <summary>
    ///     Writes a 64-bit unsigned integer.
    /// </summary>
    void Write(ulong value);

    /// <summary>
    ///     Writes a 32-bit floating point number.
    /// </summary>
    /// <param name="value"></param>
    void Write(float value);

    /// <summary>
    ///     Writes a 64-bit floating point number.
    /// </summary>
    void Write(double value);
}