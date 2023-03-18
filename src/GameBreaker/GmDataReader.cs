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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GameBreaker.Models;
using GameBreaker.Util;

namespace GameBreaker
{
    public class GmDataReader : BufferBinaryReader, IDataReader
    {
        public GMData Data { get; }

        public GMData.GMVersionInfo VersionInfo => Data.VersionInfo;

        public List<GMWarning> Warnings { get; }

        public Dictionary<int, IGMSerializable> PointerOffsets { get; }

        public Dictionary<int, GMCode.Bytecode.Instruction> Instructions {
            get;
        }

        public List<(GMTextureData, int)> TexturesToDecompress { get; }

        public GMChunk CurrentlyParsingChunk = null;

        public GmDataReader(Stream stream, string path) : base(stream)
        {
            Data = new GMData();
            Data.WorkingBuffer = Buffer;

            // Get hash for comparing later
            using (var sha1 = SHA1.Create())
                Data.Hash = sha1.ComputeHash(Buffer);
            Data.Length = Buffer.Length;

            // Get directory of the data file for later usage
            if (path != null)
            {
                Data.Directory = Path.GetDirectoryName(path);
                Data.Filename = Path.GetFileName(path);
            }

            Warnings = new List<GMWarning>();
            PointerOffsets = new Dictionary<int, IGMSerializable>(65536);
            Instructions = new Dictionary<int, GMCode.Bytecode.Instruction>(1024 * 1024);
            TexturesToDecompress = new List<(GMTextureData, int)>(64);
        }

        public void Deserialize(bool clearData = true)
        {
#if DEBUG
            Stopwatch s = new Stopwatch();
            s.Start();
#endif

            // Parse the root chunk of the file, FORM
            if (ReadChars(4) != "FORM")
                throw new GMException("Root chunk is not \"FORM\"; invalid file.");
            Data.FORM = new GMChunkFORM();
            Data.FORM.Deserialize(this);

            if (clearData)
            {
                PointerOffsets.Clear();
                Instructions.Clear();
            }

            if (TexturesToDecompress.Count > 0)
            {
                Data.Logger?.Invoke("Decompressing BZ2 textures...");
                Parallel.ForEach(TexturesToDecompress, tex =>
                {
                    // Decompress BZip2 data, leaving just QOI data
                    using MemoryStream bufferWrapper = new(Buffer);
                    bufferWrapper.Seek(tex.Item2, SeekOrigin.Begin);
                    using MemoryStream result = new(1024);
                    BZip2.Decompress(bufferWrapper, result, false);
                    tex.Item1.Data = new BufferRegion(result.ToArray());
                });
            }

#if DEBUG
            s.Stop();
            Data.Logger?.Invoke($"Finished reading WAD in {s.ElapsedMilliseconds} ms");
#endif
        }

        /// <summary>
        /// Returns (a possibly empty) object of the object type, at the specified pointer address
        /// </summary>
        public T ReadPointer<T>(int ptr) where T : IGMSerializable, new()
        {
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
        public T ReadPointer<T>() where T : IGMSerializable, new()
        {
            return ReadPointer<T>(ReadInt32());
        }

        /// <summary>
        /// Follows the specified pointer for an object type, deserializes it and returns it.
        /// Also has helper callbacks for list reading.
        /// </summary>
        public T ReadPointerObject<T>(int ptr, bool returnAfter = true, bool unique = false) where T : IGMSerializable, new()
        {
            if (ptr == 0)
                return default;

            T res;
            if (!unique && PointerOffsets.TryGetValue(ptr, out IGMSerializable s))
                res = (T)s;
            else
            {
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
        public T ReadPointerObject<T>(bool unique = false) where T : IGMSerializable, new()
        {
            return ReadPointerObject<T>(ReadInt32(), unique: unique);
        }

        /// <summary>
        /// Reads a string without parsing it
        /// </summary>
        public GMString ReadStringPointer()
        {
            return ReadPointer<GMString>(ReadInt32() - 4);
        }

        /// <summary>
        /// Reads a string AND parses it
        /// </summary>
        public GMString ReadStringPointerObject()
        {
            return ReadPointerObject<GMString>(ReadInt32() - 4);
        }

        /// <summary>
        /// Reads a GameMaker-style string
        /// </summary>
        public string ReadGMString()
        {
            Offset += 4; // Skip length; unreliable
            int baseOffset = Offset;
            while (Buffer[Offset] != 0)
                Offset++;
            int length = Offset - baseOffset;
            string res = Encoding.GetString(Buffer, baseOffset, length);
            Offset++; // go past null terminator
            return res;
        }
    }

    /// <summary>
    /// Represents a part of a buffer. Keeps a reference to the source array for its lifetime.
    /// </summary>
    public class BufferRegion
    {
        private readonly byte[] _internalRef;
        public Memory<byte> Memory;
        public int Length => Memory.Length;

        public BufferRegion(byte[] data)
        {
            _internalRef = data;
            Memory = _internalRef.AsMemory();
        }

        public BufferRegion(byte[] source, int start, int count)
        {
            _internalRef = source;
            Memory = _internalRef.AsMemory().Slice(start, count);
        }
    }
}
