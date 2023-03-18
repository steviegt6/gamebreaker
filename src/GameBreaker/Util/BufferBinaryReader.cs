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
using System.Collections.Generic;
using System.IO;
using System.Text;

// TODO: Better exceptions.
namespace GameBreaker.Util
{
    public class BufferBinaryReader : IPositionable, IEncodable {
        /// <summary>
        ///     Default encoding for <see cref="BufferBinaryReader"/> and its
        ///     inheritors. UTF-8 with no BOM.
        /// </summary>
        public static readonly Encoding DEFAULT_ENCODING =
            new UTF8Encoding(false);
        
        public int Offset { get; set; }

        public int Length => Buffer.Length;

        public byte[] Buffer { get; }
        
        public Encoding Encoding { get; }

        // TODO: Provide ctor capable of using slices? Meh... not useful...?
        protected BufferBinaryReader(Stream stream, Encoding? encoding = null) {
            // TODO: Figure out an acceptable way to handle large files.
            if (stream.Length > int.MaxValue)
                throw new IOException("Stream is too large");

            Buffer = new byte[stream.Length];
            
            stream.Seek(0, SeekOrigin.Begin);
            var bytes = stream.Read(Buffer, 0, Length);
            stream.Close();
            
            if (bytes != Length)
                throw new IOException("Stream read failed");

            Encoding = encoding ?? DEFAULT_ENCODING;
        }

        public byte ReadByte()
        {
#if DEBUG
            if (Offset < 0 || Offset + 1 > Length)
                throw new IOException("Reading out of bounds");
#endif
            return Buffer[Offset++];
        }

        public virtual bool ReadBoolean()
        {
            return ReadByte() != 0;
        }

        public string ReadChars(int count)
        {
#if DEBUG
            if (Offset < 0 || Offset + count > Length)
                throw new IOException("Reading out of bounds");
#endif
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < count; i++)
                sb.Append(Convert.ToChar(Buffer[Offset++]));
            return sb.ToString();
        }

        public BufferRegion ReadBytes(int count)
        {
#if DEBUG
            if (Offset < 0 || Offset + count > Length)
                throw new IOException("Reading out of bounds");
#endif
            BufferRegion val = new BufferRegion(Buffer, Offset, count);
            Offset += count;
            return val;
        }

        public short ReadInt16()
        {
#if DEBUG
            if (Offset < 0 || Offset + 2 > Length)
                throw new IOException("Reading out of bounds");
#endif
            return (short)(Buffer[Offset++] | Buffer[Offset++] << 8);
        }

        public ushort ReadUInt16()
        {
#if DEBUG
            if (Offset < 0 || Offset + 2 > Length)
                throw new IOException("Reading out of bounds");
#endif
            return (ushort)(Buffer[Offset++] | Buffer[Offset++] << 8);
        }

        public int ReadInt24()
        {
#if DEBUG
            if (Offset < 0 || Offset + 3 > Length)
                throw new IOException("Reading out of bounds");
#endif
            return (int)(Buffer[Offset++] | Buffer[Offset++] << 8 | (sbyte)Buffer[Offset++] << 16);
        }

        public uint ReadUInt24()
        {
#if DEBUG
            if (Offset < 0 || Offset + 3 > Length)
                throw new IOException("Reading out of bounds");
#endif
            return (uint)(Buffer[Offset++] | Buffer[Offset++] << 8 | Buffer[Offset++] << 16);
        }

        public int ReadInt32()
        {
#if DEBUG
            if (Offset < 0 || Offset + 4 > Length)
                throw new IOException("Reading out of bounds");
#endif
            return (int)(Buffer[Offset++] | Buffer[Offset++] << 8 | 
                         Buffer[Offset++] << 16 | (sbyte)Buffer[Offset++] << 24);
        }

        public uint ReadUInt32()
        {
#if DEBUG
            if (Offset < 0 || Offset + 4 > Length)
                throw new IOException("Reading out of bounds");
#endif
            return (uint)(Buffer[Offset++] | Buffer[Offset++] << 8 | 
                          Buffer[Offset++] << 16 | Buffer[Offset++] << 24);
        }

        public float ReadSingle()
        {
#if DEBUG
            if (Offset < 0 || Offset + 4 > Length)
                throw new IOException("Reading out of bounds");
#endif
            float val = BitConverter.ToSingle(Buffer, Offset);
            Offset += 4;
            return val;
        }

        public double ReadDouble()
        {
#if DEBUG
            if (Offset < 0 || Offset + 8 > Length)
                throw new IOException("Reading out of bounds");
#endif
            double val = BitConverter.ToDouble(Buffer, Offset);
            Offset += 8;
            return val;
        }

        public long ReadInt64()
        {
#if DEBUG
            if (Offset < 0 || Offset + 8 > Length)
                throw new IOException("Reading out of bounds");
#endif
            long val = BitConverter.ToInt64(Buffer, Offset);
            Offset += 8;
            return val;
        }

        public ulong ReadUInt64()
        {
#if DEBUG
            if (Offset < 0 || Offset + 8 > Length)
                throw new IOException("Reading out of bounds");
#endif
            ulong val = BitConverter.ToUInt64(Buffer, Offset);
            Offset += 8;
            return val;
        }
    }
}