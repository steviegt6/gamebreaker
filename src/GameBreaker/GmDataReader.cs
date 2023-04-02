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
using ICSharpCode.SharpZipLib.BZip2;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GameBreaker.Exceptions;
using GameBreaker.Models;
using GameBreaker.Serial;
using GameBreaker.Serial.Numerics;

namespace GameBreaker;

public sealed class GmDataReader : IDataReader {
    private int offset;
    private byte[] buffer;
    private Encoding encoding;

    public int Offset {
        get => offset;
        set => offset = value;
    }

    public int Length => buffer.Length;

    public byte[] Buffer => buffer;

    public Encoding Encoding => encoding;

#region IDataReader Impl (Properties)
    public GMData Data { get; }

    public GMData.GMVersionInfo VersionInfo => Data.VersionInfo;

    public List<GMWarning> Warnings { get; }

    public Dictionary<int, IGMSerializable> PointerOffsets { get; }

    public Dictionary<int, GMCode.Bytecode.Instruction> Instructions { get; }

    public List<(GMTextureData, int)> TexturesToDecompress { get; }
#endregion

    public GmDataReader(byte[] buffer, string path, Encoding? encoding = null) {
        this.buffer = buffer;
        this.encoding = encoding ?? IEncodable.DEFAULT_ENCODING;

        Data = new GMData();
        Data.WorkingBuffer = this.buffer;

        // Get hash for comparing later
        using (var sha1 = SHA1.Create())
            Data.Hash = sha1.ComputeHash(this.buffer);
        Data.Length = this.buffer.Length;

        // Get directory of the data file for later usage
        if (path != null) {
            Data.Directory = Path.GetDirectoryName(path);
            Data.Filename = Path.GetFileName(path);
        }

        Warnings = new List<GMWarning>();
        PointerOffsets = new Dictionary<int, IGMSerializable>(65536);
        Instructions =
            new Dictionary<int, GMCode.Bytecode.Instruction>(1024 * 1024);
        TexturesToDecompress = new List<(GMTextureData, int)>(64);
    }

#region IBinaryReader Impl (Methods)
    // TODO: Benchmark whether pinning here is actually faster...
    /// <inheritdoc cref="IBinaryReader.ReadByte"/>
    public unsafe byte ReadByte() {
        Debug.Assert(offset >= 0 && offset + 1 <= Length);

        fixed (byte* ptr = &buffer[offset]) {
            offset += sizeof(byte);
            return *ptr;
        }
    }

    /// <inheritdoc cref="IBinaryReader.ReadBoolean"/>
    public bool ReadBoolean(bool wide) {
        return wide ? ReadInt32() != 0 : ReadByte() != 0;
    }

    // TODO: Investigate possible optimizations.
    /// <inheritdoc cref="IBinaryReader.ReadChars"/>
    public string ReadChars(int count) {
        Debug.Assert(offset >= 0 && offset + count <= Length);
        var sb = new StringBuilder(count);
        for (var i = 0; i < count; i++)
            sb.Append((char) ReadByte());
        return sb.ToString();
    }

    // TODO: Investigate possible optimizations.
    /// <inheritdoc cref="IBinaryReader.ReadBytes"/>
    public BufferRegion ReadBytes(int count) {
        Debug.Assert(offset >= 0 && offset + count <= Length);
        var val = new BufferRegion(buffer, offset, count);
        offset += count;
        return val;
    }

    /// <inheritdoc cref="IBinaryReader.ReadInt16"/>
    public unsafe short ReadInt16() {
        Debug.Assert(offset >= 0 && offset + 2 <= Length);

        fixed (byte* ptr = &buffer[offset]) {
            offset += sizeof(short);
            return *(short*)ptr;
        }
    }

    /// <inheritdoc cref="IBinaryReader.ReadUInt16"/>
    public unsafe ushort ReadUInt16() {
        Debug.Assert(offset >= 0 && offset + 2 <= Length);

        fixed (byte* ptr = &buffer[offset]) {
            offset += sizeof(ushort);
            return *(ushort*)ptr;
        }
    }

    /// <inheritdoc cref="IBinaryReader.ReadInt24"/>
    public unsafe Int24 ReadInt24() {
        Debug.Assert(offset >= 0 && offset + 3 <= Length);

        fixed (byte* ptr = &buffer[offset]) {
            offset += sizeof(Int24);
            return *(Int24*)ptr;
        }
    }

    /// <inheritdoc cref="IBinaryReader.ReadUInt24"/>
    public unsafe UInt24 ReadUInt24() {
        Debug.Assert(offset >= 0 && offset + 3 <= Length);

        fixed (byte* ptr = &buffer[offset]) {
            offset += sizeof(UInt24);
            return *(UInt24*)ptr;
        }
    }

    /// <inheritdoc cref="IBinaryReader.ReadInt32"/>
    public unsafe int ReadInt32() {
        Debug.Assert(offset >= 0 && offset + 4 <= Length);

        fixed (byte* ptr = &buffer[offset]) {
            offset += sizeof(int);
            return *(int*)ptr;
        }
    }

    /// <inheritdoc cref="IBinaryReader.ReadUInt32"/>
    public unsafe uint ReadUInt32() {
        Debug.Assert(offset >= 0 && offset + 4 <= Length);

        fixed (byte* ptr = &buffer[offset]) {
            offset += sizeof(uint);
            return *(uint*)ptr;
        }
    }

    /// <inheritdoc cref="IBinaryReader.ReadInt64"/>
    public unsafe long ReadInt64() {
        Debug.Assert(offset >= 0 && offset + 8 <= Length);

        fixed (byte* ptr = &buffer[offset]) {
            offset += sizeof(long);
            return *(long*)ptr;
        }
    }

    /// <inheritdoc cref="IBinaryReader.ReadUInt64"/>
    public unsafe ulong ReadUInt64() {
        Debug.Assert(offset >= 0 && offset + 8 <= Length);

        fixed (byte* ptr = &buffer[offset]) {
            offset += sizeof(ulong);
            return *(ulong*)ptr;
        }
    }

    /// <inheritdoc cref="IBinaryReader.ReadSingle"/>
    public unsafe float ReadSingle() {
        Debug.Assert(offset >= 0 && offset + 4 <= Length);

        fixed (byte* ptr = &buffer[offset]) {
            offset += sizeof(float);
            return *(float*)ptr;
        }
    }

    /// <inheritdoc cref="IBinaryReader.ReadDouble"/>
    public unsafe double ReadDouble() {
        Debug.Assert(offset >= 0 && offset + 8 <= Length);

        fixed (byte* ptr = &buffer[offset]) {
            offset += sizeof(double);
            return *(double*)ptr;
        }
    }
#endregion

#region IDataReader Impl (Methods)
    public void Deserialize(bool clearData = true) {
#if DEBUG
        Stopwatch s = new Stopwatch();
        s.Start();
#endif

        // Parse the root chunk of the file, FORM
        if (ReadChars(4) != "FORM")
            throw new InvalidFormHeaderException(
                "Root chunk is not \"FORM\"; invalid file.");

        Data.FORM = new GMChunkFORM();
        Data.FORM.Deserialize(this);

        if (clearData) {
            PointerOffsets.Clear();
            Instructions.Clear();
        }

        if (TexturesToDecompress.Count > 0) {
            Data.Logger?.Invoke("Decompressing BZ2 textures...");
            Parallel.ForEach(TexturesToDecompress,
                             tex => {
                                 // Decompress BZip2 data, leaving just QOI data
                                 using MemoryStream bufferWrapper = new(buffer);
                                 bufferWrapper.Seek(
                                     tex.Item2,
                                     SeekOrigin.Begin);
                                 using MemoryStream result = new(1024);
                                 BZip2.Decompress(bufferWrapper, result, false);
                                 tex.Item1.Data =
                                     new BufferRegion(result.ToArray());
                             });
        }

#if DEBUG
        s.Stop();
        Data.Logger?.Invoke(
            $"Finished reading WAD in {s.ElapsedMilliseconds} ms");
#endif
    }

    /// <summary>
    /// Returns (a possibly empty) object of the object type, at the specified pointer address
    /// </summary>
    public T ReadPointer<T>(int ptr) where T : IGMSerializable, new() {
        if (ptr == 0)
            return default;
        if (PointerOffsets.TryGetValue(ptr, out IGMSerializable s))
            return (T)s;

        T res = new T();
        PointerOffsets[ptr] = res;
        return res;
    }

    /// <summary>
    /// Returns (a possibly empty) object of the object type, at the pointer in the file
    /// </summary>
    public T ReadPointer<T>() where T : IGMSerializable, new() {
        return ReadPointer<T>(ReadInt32());
    }

    /// <summary>
    /// Follows the specified pointer for an object type, deserializes it and returns it.
    /// Also has helper callbacks for list reading.
    /// </summary>
    public T ReadPointerObject<T>(
        int ptr,
        bool returnAfter = true,
        bool unique = false
    )
        where T : IGMSerializable, new() {
        if (ptr == 0)
            return default;

        T res;

        if (!unique && PointerOffsets.TryGetValue(ptr, out IGMSerializable s))
            res = (T)s;
        else {
            res = new T();

            if (unique)
                PointerOffsets[ptr] = res;
        }

        int returnTo = offset;
        offset = ptr;

        res.Deserialize(this);

        if (returnAfter)
            offset = returnTo;

        return res;
    }

    /// <summary>
    /// Follows a pointer (in the file) for an object type, deserializes it and returns it.
    /// </summary>
    public T ReadPointerObject<T>(bool unique = false)
        where T : IGMSerializable, new() {
        return ReadPointerObject<T>(ReadInt32(), unique: unique);
    }

    /// <summary>
    /// Reads a string without parsing it
    /// </summary>
    public GMString ReadStringPointer() {
        return ReadPointer<GMString>(ReadInt32() - 4);
    }

    /// <summary>
    /// Reads a string AND parses it
    /// </summary>
    public GMString ReadStringPointerObject() {
        return ReadPointerObject<GMString>(ReadInt32() - 4);
    }

    /// <summary>
    /// Reads a GameMaker-style string
    /// </summary>
    public string ReadGMString() {
        offset += 4; // Skip length; unreliable
        int baseOffset = offset;
        while (buffer[offset] != 0)
            offset++;
        int length = offset - baseOffset;
        string res = encoding.GetString(buffer, baseOffset, length);
        offset++; // go past null terminator
        return res;
    }
#endregion

#region IDisposable Impl
    void IDisposable.Dispose() { }
#endregion

    /// <summary>
    ///     Initializes a <see cref="GmDataReader"/> instance from the given
    ///     <paramref name="stream"/>. The stream will be closed after reading.
    /// </summary>
    /// <param name="stream">The stream to read from.</param>
    /// <param name="path">TODO</param>
    /// <param name="encoding">The encoding to use.</param>
    /// <returns>The initialized <see cref="GmDataReader"/>.</returns>
    public static GmDataReader FromStream(
        Stream stream,
        string path,
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

        return new GmDataReader(buf, path, encoding);
    }
}
