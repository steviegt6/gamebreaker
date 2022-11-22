// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.
// Copyright (c) kryz-h. Licensed under the GPL License, version 3.
// See the LICENSE-UndertaleModTool file in the repository root for full terms and conditions.

using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using GameBreaker.DataTypes;
using GameBreaker.Serialization.Abstractions;

namespace GameBreaker.Serialization;

/// <summary>
///     Default implementation of <see cref="IDataSerializer"/>.
/// </summary>
public class DataDeserializer : IDataDeserializer
{
    /// <inheritdoc />
    public virtual Encoding Encoding { get; }

    /// <inheritdoc />
    public virtual long Length {
        get => Stream.Length;
        set => Stream.SetLength(value);
    }

    /// <inheritdoc />
    public virtual long Position {
        get => Stream.Position;
        set => Stream.Position = value;
    }

    /// <summary>
    ///     The underlying <see cref="System.IO.Stream"/> this <see cref="DataDeserializer"/> interfaces with.
    /// </summary>
    protected virtual Stream Stream { get; }

    /// <summary>
    ///     Instantiates a new <see cref="DataDeserializer"/>.
    /// </summary>
    /// <param name="stream">The underlying <see cref="System.IO.Stream"/> to interface with.</param>
    /// <param name="encoding">The encoding to handle strings with; <see cref="System.Text.Encoding.UTF8"/> by default.</param>
    public DataDeserializer(Stream stream, Encoding? encoding = null) {
        Stream = stream;
        Encoding = encoding ?? Encoding.UTF8;
    }

    /// <inheritdoc />
    public virtual byte ReadByte() {
        Debug.Assert(Position + 1 <= Length, "ReadByte: Read out of bounds.");
        return (byte) Stream.ReadByte();
    }

    // TODO: lol this is terrible
    /// <inheritdoc />
    public virtual byte[] ReadBytes(int count) {
        Debug.Assert(Position + count <= Length, "ReadBytes: Read out of bounds.");
        List<byte> bytes = new();
        for (int i = 0; i < count; i++) bytes.Add(ReadByte());
        return bytes.ToArray();
    }

    /// <inheritdoc />
    public virtual bool ReadBoolean(bool wide = true) {
        Debug.Assert(Position + 1 <= Length, "ReadBoolean: Read out of bounds.");
        int val = wide ? ReadInt32() : ReadByte();
        Debug.Assert(val is 0 or 1, $"ReadBoolean: Value was not 0 or 1 ({val}).");
        return val != 0;
    }

    /// <inheritdoc />
    public virtual char ReadChar() {
        Debug.Assert(Position + 1 <= Length, "ReadBoolean: Read out of bounds.");
        return Convert.ToChar(ReadByte());
    }

    /// <inheritdoc />
    public virtual char[] ReadChars(int count) {
        Debug.Assert(Position + count <= Length, "ReadChars: Read out of bounds.");
        StringBuilder sb = new();
        for (int i = 0; i < count; i++) sb.Append(ReadChar());
        // TODO: Just convert from a list? Just return a string? What to do?
        return sb.ToString().ToCharArray();
    }

    /// <inheritdoc />
    public virtual short ReadInt16() {
        Debug.Assert(Position + 2 <= Length, "ReadInt16: Read out of bounds.");
        return BinaryPrimitives.ReadInt16LittleEndian(ReadBytes(2));
    }

    /// <inheritdoc />
    public virtual ushort ReadUInt16() {
        Debug.Assert(Position + 2 <= Length, "ReadUInt16: Read out of bounds.");
        return BinaryPrimitives.ReadUInt16LittleEndian(ReadBytes(2));
    }

    // TODO: Moving away from direct byte reading and more into BinaryPrimitives/MemoryMarshal would be preferable...
    /// <inheritdoc />
    public virtual Int24 ReadInt24() {
        Debug.Assert(Position + 3 <= Length, "ReadInt24: Read out of bounds.");
        return new Int24(ReadByte() | ReadByte() << 8 | (sbyte) ReadByte() << 16);
    }

    /// <inheritdoc />
    public virtual UInt24 ReadUInt24() {
        Debug.Assert(Position + 3 <= Length, "ReadUInt24: Read out of bounds.");
        return new UInt24((uint) (ReadByte() | ReadByte() << 8 | ReadByte() << 16));
    }

    /// <inheritdoc />
    public virtual int ReadInt32() {
        Debug.Assert(Position + 4 <= Length, "ReadInt32: Read out of bounds.");
        return BinaryPrimitives.ReadInt32LittleEndian(ReadBytes(4));
    }

    /// <inheritdoc />
    public virtual uint ReadUInt32() {
        Debug.Assert(Position + 4 <= Length, "ReadUInt32: Read out of bounds.");
        return BinaryPrimitives.ReadUInt32LittleEndian(ReadBytes(4));
    }

    /// <inheritdoc />
    public virtual long ReadInt64() {
        Debug.Assert(Position + 8 <= Length, "ReadInt64: Read out of bounds.");
        return BitConverter.ToInt64(ReadBytes(8), 0);
    }

    /// <inheritdoc />
    public virtual ulong ReadUInt64() {
        Debug.Assert(Position + 8 <= Length, "ReadUInt64: Read out of bounds.");
        return BitConverter.ToUInt64(ReadBytes(8), 0);
    }

    /// <inheritdoc />
    public virtual float ReadSingle() {
        Debug.Assert(Position + 4 <= Length, "ReadSingle: Read out of bounds.");
        return BitConverter.ToSingle(ReadBytes(4), 0);
    }

    /// <inheritdoc />
    public virtual double ReadDouble() {
        Debug.Assert(Position + 8 <= Length, "ReadDouble: Read out of bounds.");
        return BitConverter.ToDouble(ReadBytes(8), 0);
    }

    protected virtual void Dispose(bool disposing) {
        if (disposing) {
            Stream.Close();
            Stream.Dispose();
        }
    }

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}