/*
 * Copyright (c) 2023 Tomat & GameBreaker Contributors
 * Copyright (c) 2020 colinator27
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
using System.Diagnostics;
using System.IO;
using System.Text;
using GameBreaker.Serial.Numerics;

// TODO: Better exceptions.
namespace GameBreaker.Serial; 

/// <summary>
///     An implementation of <see cref="IBinaryReader"/> that takes a raw
///     byte array as its backing buffer.
/// </summary>
public class BufferBinaryReader : IBinaryReader {
#region IBinaryReader Impl (Properties)
    private int offset;

    /// <inheritdoc cref="IBinaryReader.Offset"/>
    public int Offset {
        get => offset;
        set => offset = value;
    }

    /// <inheritdoc cref="IBinaryReader.Length"/>
    public int Length => Buffer.Length;

    /// <inheritdoc cref="IBinaryReader.Buffer"/>
    public byte[] Buffer { get; }

    /// <inheritdoc cref="IBinaryReader.Encoding"/>
    public Encoding Encoding { get; }
#endregion

    /// <summary>
    ///     Initializes a new instance of <see cref="BufferBinaryReader"/>.
    /// </summary>
    /// <param name="buffer">The backing buffer to use.</param>
    /// <param name="encoding">The <see cref="Encoding"/> to use.</param>
    public BufferBinaryReader(byte[] buffer, Encoding? encoding = null) {
        Buffer = buffer;
        Encoding = encoding ?? IEncodable.DEFAULT_ENCODING;
    }

#region IBinaryReader Impl (Methods)
    /// <inheritdoc cref="IBinaryReader.ReadByte"/>
    public virtual unsafe byte ReadByte() {
        Debug.Assert(Offset >= 0 && Offset + 1 <= Length);
        fixed (byte* ptr = &Buffer[Offset]) {
            Offset += sizeof(byte);
            return *ptr;
        }
    }

    /// <inheritdoc cref="IBinaryReader.ReadBoolean"/>
    public virtual bool ReadBoolean(bool wide) {
        return wide ? ReadInt32() != 0 : ReadByte() != 0;
    }

    /// <inheritdoc cref="IBinaryReader.ReadChars"/>
    public virtual string ReadChars(int count) {
        Debug.Assert(Offset >= 0 && Offset + count <= Length);
        var sb = new StringBuilder();
        for (int i = 0; i < count; i++)
            sb.Append(Convert.ToChar(Buffer[Offset++]));
        return sb.ToString();
    }

    /// <inheritdoc cref="IBinaryReader.ReadBytes"/>
    public virtual BufferRegion ReadBytes(int count) {
        Debug.Assert(Offset >= 0 && Offset + count <= Length);
        var val = new BufferRegion(Buffer, Offset, count);
        Offset += count;
        return val;
    }

    /// <inheritdoc cref="IBinaryReader.ReadInt16"/>
    public virtual unsafe short ReadInt16() {
        Debug.Assert(Offset >= 0 && Offset + 2 <= Length);
        fixed (byte* ptr = &Buffer[Offset]) {
            Offset += sizeof(short);
            return *(short*)ptr;
        }
    }

    /// <inheritdoc cref="IBinaryReader.ReadUInt16"/>
    public virtual unsafe ushort ReadUInt16() {
        Debug.Assert(Offset >= 0 && Offset + 2 <= Length);
        fixed (byte* ptr = &Buffer[Offset]) {
            Offset += sizeof(ushort);
            return *(ushort*)ptr;
        }
    }

    /// <inheritdoc cref="IBinaryReader.ReadInt24"/>
    public virtual unsafe Int24 ReadInt24() {
        Debug.Assert(Offset >= 0 && Offset + 3 <= Length);
        fixed (byte* ptr = &Buffer[Offset]) {
            Offset += sizeof(Int24);
            return *(Int24*)ptr;
        }
    }

    /// <inheritdoc cref="IBinaryReader.ReadUInt24"/>
    public virtual unsafe UInt24 ReadUInt24() {
        Debug.Assert(Offset >= 0 && Offset + 3 <= Length);
        fixed (byte* ptr = &Buffer[Offset]) {
            Offset += sizeof(UInt24);
            return *(UInt24*)ptr;
        }
    }

    /// <inheritdoc cref="IBinaryReader.ReadInt32"/>
    public virtual unsafe int ReadInt32() {
        Debug.Assert(Offset >= 0 && Offset + 4 <= Length);
        fixed (byte* ptr = &Buffer[Offset]) {
            Offset += sizeof(int);
            return *(int*)ptr;
        }
    }

    /// <inheritdoc cref="IBinaryReader.ReadUInt32"/>
    public virtual unsafe uint ReadUInt32() {
        Debug.Assert(Offset >= 0 && Offset + 4 <= Length);
        fixed (byte* ptr = &Buffer[Offset]) {
            Offset += sizeof(uint);
            return *(uint*)ptr;
        }
    }

    /// <inheritdoc cref="IBinaryReader.ReadInt64"/>
    public virtual unsafe long ReadInt64() {
        Debug.Assert(Offset >= 0 && Offset + 8 <= Length);
        fixed (byte* ptr = &Buffer[Offset]) {
            Offset += sizeof(long);
            return *(long*)ptr;
        }
    }

    /// <inheritdoc cref="IBinaryReader.ReadUInt64"/>
    public virtual unsafe ulong ReadUInt64() {
        Debug.Assert(Offset >= 0 && Offset + 8 <= Length);
        fixed (byte* ptr = &Buffer[Offset]) {
            Offset += sizeof(ulong);
            return *(ulong*)ptr;
        }
    }

    /// <inheritdoc cref="IBinaryReader.ReadSingle"/>
    public virtual unsafe float ReadSingle() {
        Debug.Assert(Offset >= 0 && Offset + 4 <= Length);
        fixed (byte* ptr = &Buffer[Offset]) {
            Offset += sizeof(float);
            return *(float*)ptr;
        }
    }

    /// <inheritdoc cref="IBinaryReader.ReadDouble"/>
    public virtual unsafe double ReadDouble() {
        Debug.Assert(Offset >= 0 && Offset + 8 <= Length);
        fixed (byte* ptr = &Buffer[Offset]) {
            Offset += sizeof(double);
            return *(double*)ptr;
        }
    }
#endregion

#region IDisposable Impl
    protected virtual void Dispose(bool disposing) {
        if (disposing) { }
    }

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
#endregion

    /// <summary>
    ///     Initializes a <see cref="BufferBinaryReader"/> instance from the
    ///     given <paramref name="stream"/>. The stream will be closed after
    ///     reading.
    /// </summary>
    /// <param name="stream">The stream to read from.</param>
    /// <param name="encoding">The encoding to use.</param>
    /// <returns>The initialized <see cref="BufferBinaryReader"/>.</returns>
    public static BufferBinaryReader FromStream(
        Stream stream,
        Encoding? encoding = null
    ) {
        if (stream.Length > int.MaxValue)
            throw new IOException("Stream is too large");

        var buf = new byte[stream.Length];

        stream.Seek(0, SeekOrigin.Begin);
        var bytes = stream.Read(buf, 0, buf.Length);
        stream.Close();

        if (bytes != buf.Length)
            throw new IOException("Stream read failed");

        return new BufferBinaryReader(buf, encoding);
    }
}