using System;
using System.IO;
using GameBreaker.Serial.Numerics;

namespace GameBreaker.Serial;

/// <summary>
///     Represents a positionable object dealing with a buffer that performs
///     binary reading operations.
/// </summary>
public interface IBinaryWriter : IPositionable,
                                 IEncodable,
                                 IDisposable {
    /// <summary>
    ///     Writes a byte to the buffer.
    /// </summary>
    /// <param name="value">The byte to write.</param>
    void Write(byte value);

    /// <summary>
    ///     Writes a boolean to the buffer.
    /// </summary>
    /// <param name="value">The boolean to write.</param>
    /// <param name="wide">Whether this boolean is wide or narrow.</param>
    void Write(bool value, bool wide);

    /// <summary>
    ///     Writes a byte array in memory to the buffer.
    /// </summary>
    /// <param name="value">The byte array in memory to write.</param>
    void Write(BufferRegion value);

    /// <summary>
    ///     Writes a byte array to the buffer.
    /// </summary>
    /// <param name="value">The byte array to write.</param>
    void Write(byte[] value);

    /// <summary>
    ///     Writes a char array to the buffer.
    /// </summary>
    /// <param name="value">The char array to write.</param>
    void Write(char[] value);

    /// <summary>
    ///     Writes a 16-bit signed integer to the buffer.
    /// </summary>
    /// <param name="value">The 16-bit signed integer to write.</param>
    void Write(short value);

    /// <summary>
    ///     Writes a 16-bit unsigned integer to the buffer.
    /// </summary>
    /// <param name="value">The 16-bit unsigned integer to write.</param>
    void Write(ushort value);

    /// <summary>
    ///     Writes a 24-bit signed integer to the buffer.
    /// </summary>
    /// <param name="value">The 24-bit signed integer to write.</param>
    void Write(Int24 value);

    /// <summary>
    ///     Writes a 24-bit unsigned integer to the buffer.
    /// </summary>
    /// <param name="value">The 24-bit unsigned integer to write.</param>
    void Write(UInt24 value);

    /// <summary>
    ///     Writes a 32-bit signed integer to the buffer.
    /// </summary>
    /// <param name="value">The 32-bit signed integer to write.</param>
    void Write(int value);

    /// <summary>
    ///     Writes a 32-bit unsigned integer to the buffer.
    /// </summary>
    /// <param name="value">The 32-bit unsigned integer to write.</param>
    void Write(uint value);

    /// <summary>
    ///     Writes a 64-bit signed integer to the buffer.
    /// </summary>
    /// <param name="value">The 64-bit signed integer to write.</param>
    void Write(long value);

    /// <summary>
    ///     Writes a 64-bit unsigned integer to the buffer.
    /// </summary>
    /// <param name="value">The 64-bit unsigned integer to write.</param>
    void Write(ulong value);

    /// <summary>
    ///     Writes a 32-bit floating point number to the buffer.
    /// </summary>
    /// <param name="value">The 32-bit floating point number to write.</param>
    void Write(float value);

    /// <summary>
    ///     Writes a 64-bit floating point number to the buffer.
    /// </summary>
    /// <param name="value">The 64-bit floating point number to write.</param>
    void Write(double value);

    /// <summary>
    ///     Flushes the buffer to the given <paramref name="stream"/>.
    /// </summary>
    /// <param name="stream">The stream to flush to.</param>
    void Flush(Stream stream);
}

/// <summary>
///     Extension methods for <see cref="IBinaryWriter"/>.
/// </summary>
public static class BinaryWriterExtensions {
    public static int BeginLength(this IBinaryWriter writer) {
        writer.Write(0xBADD0660);
        return writer.Offset;
    }

    public static void EndLength(this IBinaryWriter writer, int start) {
        var offset = writer.Offset;
        writer.Offset = start - sizeof(int);
        writer.Write(writer.Offset - start);
        writer.Offset = offset;
    }

    public static void WriteAt(
        this IBinaryWriter writer,
        int value,
        int offset
    ) {
        var oldOffset = writer.Offset;
        writer.Offset = offset;
        writer.Write(value);
        writer.Offset = oldOffset;
    }
}
