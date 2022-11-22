// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using GameBreaker.DataTypes;

namespace GameBreaker.Serialization.Abstractions
{
    /// <summary>
    ///     Describes an object capable of deserializing data.
    /// </summary>
    public interface IDataDeserializer : IPositionable
    {
        /// <summary>
        ///     Reads a byte.
        /// </summary>
        byte ReadByte();

        /// <summary>
        ///     Reads a given amount of bytes (<paramref name="count"/>).
        /// </summary>
        /// <returns>The read bytes, as an array.</returns>
        byte[] ReadBytes(int count);

        /// <summary>
        ///     Reads a boolean.
        /// </summary>
        /// <param name="wide">Whether the boolean is wide (four bytes) or not (one byte).</param>
        bool ReadBoolean(bool wide = true);

        /// <summary>
        ///     Reads a character.
        /// </summary>
        char ReadChar();

        /// <summary>
        ///     Reads a given amount of characters (<paramref name="count"/>).
        /// </summary>
        /// <returns>The read characters, as an array.</returns>
        char[] ReadChars(int count);

        /// <summary>
        ///     Reads a 16-bit signed integer.
        /// </summary>
        short ReadInt16();

        /// <summary>
        ///     Reads a 16-bit unsigned integer.
        /// </summary>
        ushort ReadUInt16();

        /// <summary>
        ///     Reads a 24-bit signed integer.
        /// </summary>
        Int24 ReadInt24();

        /// <summary>
        ///     Reads a 24-bit unsigned integer.
        /// </summary>
        UInt24 ReadUInt24();

        /// <summary>
        ///     Reads a 32-bit signed integer.
        /// </summary>
        int ReadInt32();

        /// <summary>
        ///     Reads a 32-bit unsigned integer.
        /// </summary>
        uint ReadUInt32();

        /// <summary>
        ///     Reads a 64-bit signed integer.
        /// </summary>
        long ReadInt64();

        /// <summary>
        ///     Reads a 64-bit unsigned integer.
        /// </summary>
        ulong ReadUInt64();

        /// <summary>
        ///     Reads a 32-bit floating point number.
        /// </summary>
        float ReadSingle();

        /// <summary>
        ///     Reads a 64-bit floating point number.
        /// </summary>
        double ReadDouble();
    }
}