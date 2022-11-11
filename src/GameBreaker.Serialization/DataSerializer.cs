// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.
// Copyright (c) kryz-h. Licensed under the GPL License, version 3.
// See the LICENSE-UndertaleModTool file in the repository root for full terms and conditions.

using System;
using System.IO;
using System.Text;
using GameBreaker.DataTypes;
using GameBreaker.Serialization.Abstractions;

namespace GameBreaker.Serialization;

public class DataSerializer : BinaryWriter, IDataSerializer
{
    public virtual Encoding Encoding { get; }

    public virtual long Length {
        get => OutStream.Length;
        set => OutStream.SetLength(value);
    }

    public virtual long Position {
        get => OutStream.Position;
        set => OutStream.Position = value;
    }

    public DataSerializer(Stream stream, Encoding? encoding = null) : base(stream) {
        Encoding = encoding ?? Encoding.UTF8;
    }

    public virtual void Write(Int24 value) {
        Span<byte> buffer = stackalloc byte[3];
        buffer[0] = (byte) (value.Value & 0xFF);
        buffer[1] = (byte) ((value.Value >> 8) & 0xFF);
        buffer[2] = (byte) ((value.Value >> 16) & 0xFF);
        OutStream.Write(buffer);
    }

    public virtual void Write(UInt24 value) {
        Span<byte> buffer = stackalloc byte[3];
        buffer[0] = (byte) (value.Value & 0xFF);
        buffer[1] = (byte) ((value.Value >> 8) & 0xFF);
        buffer[2] = (byte) ((value.Value >> 16) & 0xFF);
        OutStream.Write(buffer);
    }
}