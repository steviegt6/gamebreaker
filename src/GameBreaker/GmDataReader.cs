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

using ICSharpCode.SharpZipLib.BZip2;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GameBreaker.Models;
using GameBreaker.Serial;
using GameBreaker.Serial.Numerics;

namespace GameBreaker;

public class GmDataReader : IDataReader {
    protected IBinaryReader Reader { get; }

#region IBinaryReader Impl (Properties)
    public virtual int Offset {
        get => Reader.Offset;
        set => Reader.Offset = value;
    }

    public virtual int Length => Reader.Length;

    public virtual byte[] Buffer => Reader.Buffer;

    public virtual Encoding Encoding => Reader.Encoding;
#endregion

#region IDataReader Impl (Properties)
    public GMData Data { get; }

    public GMData.GMVersionInfo VersionInfo => Data.VersionInfo;

    public List<GMWarning> Warnings { get; }

    public Dictionary<int, IGMSerializable> PointerOffsets { get; }

    public Dictionary<int, GMCode.Bytecode.Instruction> Instructions { get; }

    public List<(GMTextureData, int)> TexturesToDecompress { get; }
#endregion

    public GmDataReader(IBinaryReader reader, string path) {
        Reader = reader;

        Data = new GMData();
        Data.WorkingBuffer = Buffer;

        // Get hash for comparing later
        using (var sha1 = SHA1.Create())
            Data.Hash = sha1.ComputeHash(Buffer);
        Data.Length = Buffer.Length;

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
    public virtual byte ReadByte() {
        return Reader.ReadByte();
    }

    public virtual bool ReadBoolean(bool wide) {
        return Reader.ReadBoolean(wide);
    }

    public virtual string ReadChars(int count) {
        return Reader.ReadChars(count);
    }

    public virtual BufferRegion ReadBytes(int count) {
        return Reader.ReadBytes(count);
    }

    public virtual short ReadInt16() {
        return Reader.ReadInt16();
    }

    public virtual ushort ReadUInt16() {
        return Reader.ReadUInt16();
    }

    public virtual Int24 ReadInt24() {
        return Reader.ReadInt24();
    }

    public virtual UInt24 ReadUInt24() {
        return Reader.ReadUInt24();
    }

    public virtual int ReadInt32() {
        return Reader.ReadInt32();
    }

    public virtual uint ReadUInt32() {
        return Reader.ReadUInt32();
    }

    public virtual long ReadInt64() {
        return Reader.ReadInt64();
    }

    public virtual ulong ReadUInt64() {
        return Reader.ReadUInt64();
    }

    public virtual float ReadSingle() {
        return Reader.ReadSingle();
    }

    public virtual double ReadDouble() {
        return Reader.ReadDouble();
    }
#endregion

#region IDataReader Impl (Methods)
    public virtual void Deserialize(bool clearData = true) {
#if DEBUG
        Stopwatch s = new Stopwatch();
        s.Start();
#endif

        // Parse the root chunk of the file, FORM
        if (ReadChars(4) != "FORM")
            throw new GMException("Root chunk is not \"FORM\"; invalid file.");

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
                                 using MemoryStream bufferWrapper = new(Buffer);
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
    public virtual T ReadPointer<T>(int ptr) where T : IGMSerializable, new() {
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
    public virtual T ReadPointer<T>() where T : IGMSerializable, new() {
        return ReadPointer<T>(ReadInt32());
    }

    /// <summary>
    /// Follows the specified pointer for an object type, deserializes it and returns it.
    /// Also has helper callbacks for list reading.
    /// </summary>
    public virtual T ReadPointerObject<T>(
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

        int returnTo = Offset;
        Offset = ptr;

        res.Deserialize(this);

        if (returnAfter)
            Offset = returnTo;

        return res;
    }

    /// <summary>
    /// Follows a pointer (in the file) for an object type, deserializes it and returns it.
    /// </summary>
    public virtual T ReadPointerObject<T>(bool unique = false)
        where T : IGMSerializable, new() {
        return ReadPointerObject<T>(ReadInt32(), unique: unique);
    }

    /// <summary>
    /// Reads a string without parsing it
    /// </summary>
    public virtual GMString ReadStringPointer() {
        return ReadPointer<GMString>(ReadInt32() - 4);
    }

    /// <summary>
    /// Reads a string AND parses it
    /// </summary>
    public virtual GMString ReadStringPointerObject() {
        return ReadPointerObject<GMString>(ReadInt32() - 4);
    }

    /// <summary>
    /// Reads a GameMaker-style string
    /// </summary>
    public virtual string ReadGMString() {
        Offset += 4; // Skip length; unreliable
        int baseOffset = Offset;
        while (Buffer[Offset] != 0)
            Offset++;
        int length = Offset - baseOffset;
        string res = Encoding.GetString(Buffer, baseOffset, length);
        Offset++; // go past null terminator
        return res;
    }
#endregion
}
