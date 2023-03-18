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
using System.Diagnostics;
using System.IO;
using System.Text;

// TODO: Better exceptions.
namespace GameBreaker.Util
{
    public class BufferBinaryReader : IBinaryReader {
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

            Encoding = encoding ?? IEncodable.DEFAULT_ENCODING;
        }

        public virtual byte ReadByte()
        {
            Debug.Assert(Offset >= 0 && Offset + 1 <= Length);
            return Buffer[Offset++];
        }

        public virtual bool ReadBoolean(bool wide)
        {
            return (wide ? ReadInt32() : ReadByte()) != 0;
        }

        public virtual string ReadChars(int count)
        {
            Debug.Assert(Offset >= 0 && Offset + count <= Length);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < count; i++)
                sb.Append(Convert.ToChar(Buffer[Offset++]));
            return sb.ToString();
        }

        public virtual BufferRegion ReadBytes(int count)
        {
            Debug.Assert(Offset >= 0 && Offset + count <= Length);
            BufferRegion val = new BufferRegion(Buffer, Offset, count);
            Offset += count;
            return val;
        }

        public virtual short ReadInt16()
        {
            Debug.Assert(Offset >= 0 && Offset + 2 <= Length);
            return (short)(Buffer[Offset++] | Buffer[Offset++] << 8);
        }

        public virtual ushort ReadUInt16()
        {
            Debug.Assert(Offset >= 0 && Offset + 2 <= Length);
            return (ushort)(Buffer[Offset++] | Buffer[Offset++] << 8);
        }

        public virtual int ReadInt24()
        {
            Debug.Assert(Offset >= 0 && Offset + 3 <= Length);
            return (int)(Buffer[Offset++] | Buffer[Offset++] << 8 | (sbyte)Buffer[Offset++] << 16);
        }

        public virtual uint ReadUInt24()
        {
            Debug.Assert(Offset >= 0 && Offset + 3 <= Length);
            return (uint)(Buffer[Offset++] | Buffer[Offset++] << 8 | Buffer[Offset++] << 16);
        }

        public virtual int ReadInt32()
        {
            Debug.Assert(Offset >= 0 && Offset + 4 <= Length);
            return (int)(Buffer[Offset++] | Buffer[Offset++] << 8 | 
                         Buffer[Offset++] << 16 | (sbyte)Buffer[Offset++] << 24);
        }

        public virtual uint ReadUInt32()
        {
            Debug.Assert(Offset >= 0 && Offset + 4 <= Length);
            return (uint)(Buffer[Offset++] | Buffer[Offset++] << 8 | 
                          Buffer[Offset++] << 16 | Buffer[Offset++] << 24);
        }

        public virtual long ReadInt64()
        {
            Debug.Assert(Offset >= 0 && Offset + 8 <= Length);
            long val = BitConverter.ToInt64(Buffer, Offset);
            Offset += 8;
            return val;
        }

        public virtual ulong ReadUInt64()
        {
            Debug.Assert(Offset >= 0 && Offset + 8 <= Length);
            ulong val = BitConverter.ToUInt64(Buffer, Offset);
            Offset += 8;
            return val;
        }

        public virtual float ReadSingle()
        {
            Debug.Assert(Offset >= 0 && Offset + 4 <= Length);
            float val = BitConverter.ToSingle(Buffer, Offset);
            Offset += 4;
            return val;
        }

        public virtual double ReadDouble()
        {
            Debug.Assert(Offset >= 0 && Offset + 8 <= Length);
            double val = BitConverter.ToDouble(Buffer, Offset);
            Offset += 8;
            return val;
        }
    }
}