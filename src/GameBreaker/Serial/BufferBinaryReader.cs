﻿/*
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
namespace GameBreaker.Serial {
    public class BufferBinaryReader : IBinaryReader {
#region IBinaryReader Impl (Properties)
        private int offset;

        public int Offset {
            get => offset;
            set => offset = value;
        }

        public int Length => Buffer.Length;

        public byte[] Buffer { get; }

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
        public virtual byte ReadByte() {
            Debug.Assert(Offset >= 0 && Offset + 1 <= Length);
            return Buffer[Offset++];
        }

        public virtual bool ReadBoolean(bool wide) {
            return (wide ? ReadInt32() : ReadByte()) != 0;
        }

        public virtual string ReadChars(int count) {
            Debug.Assert(Offset >= 0 && Offset + count <= Length);
            var sb = new StringBuilder();
            for (int i = 0; i < count; i++)
                sb.Append(Convert.ToChar(Buffer[Offset++]));
            return sb.ToString();
        }

        public virtual BufferRegion ReadBytes(int count) {
            Debug.Assert(Offset >= 0 && Offset + count <= Length);
            var val = new BufferRegion(Buffer, Offset, count);
            Offset += count;
            return val;
        }

        public virtual short ReadInt16() {
            Debug.Assert(Offset >= 0 && Offset + 2 <= Length);
            return GmBitConverter.ToInt16(Buffer, ref offset);
        }

        public virtual ushort ReadUInt16() {
            Debug.Assert(Offset >= 0 && Offset + 2 <= Length);
            return GmBitConverter.ToUInt16(Buffer, ref offset);
        }

        public virtual Int24 ReadInt24() {
            Debug.Assert(Offset >= 0 && Offset + 3 <= Length);
            return GmBitConverter.ToInt24(Buffer, ref offset);
        }

        public virtual UInt24 ReadUInt24() {
            Debug.Assert(Offset >= 0 && Offset + 3 <= Length);
            return GmBitConverter.ToUInt24(Buffer, ref offset);
        }

        public virtual int ReadInt32() {
            Debug.Assert(Offset >= 0 && Offset + 4 <= Length);
            return GmBitConverter.ToInt32(Buffer, ref offset);
        }

        public virtual uint ReadUInt32() {
            Debug.Assert(Offset >= 0 && Offset + 4 <= Length);
            return GmBitConverter.ToUInt32(Buffer, ref offset);
        }

        public virtual long ReadInt64() {
            Debug.Assert(Offset >= 0 && Offset + 8 <= Length);
            return GmBitConverter.ToInt64(Buffer, ref offset);
        }

        public virtual ulong ReadUInt64() {
            Debug.Assert(Offset >= 0 && Offset + 8 <= Length);
            return GmBitConverter.ToUInt64(Buffer, ref offset);
        }

        public virtual float ReadSingle() {
            Debug.Assert(Offset >= 0 && Offset + 4 <= Length);
            return GmBitConverter.ToSingle(Buffer, ref offset);
        }

        public virtual double ReadDouble() {
            Debug.Assert(Offset >= 0 && Offset + 8 <= Length);
            return GmBitConverter.ToDouble(Buffer, ref offset);
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
}
