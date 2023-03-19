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
using System.IO;
using System.Text;
using GameBreaker.Serial.Numerics;

namespace GameBreaker.Serial
{
    public class BufferBinaryWriter : IBinaryWriter {
        public const int DEFAULT_SIZE = 1024 * 1024 * 32;
        
        private byte[] buffer;
        private int offset;

        public int Offset
        {
            get => offset;
            set {
                ResizeToFit(value);
                offset = value;
            }
        }

        public int Length { get; set; }

        public byte[] Buffer => buffer;

        public Encoding Encoding { get; }

        public BufferBinaryWriter(int baseSize = DEFAULT_SIZE)
        {
            buffer = new byte[baseSize];
            Offset = 0;

            Encoding = new UTF8Encoding(false);
        }

        private void ResizeToFit(int size)
        {
            while (size > Buffer.Length)
                Array.Resize(ref buffer, Buffer.Length * 2);
            if (Length < size)
                Length = size;
        }

        public virtual void Write(byte value)
        {
            ResizeToFit(Offset + 1);
            Buffer[Offset++] = value;
        }

        public virtual void Write(bool value, bool wide)
        {
            if (wide) {
                Write(value ? 1 : 0);
            }
            else {
                ResizeToFit(Offset + 1);
                Buffer[Offset++] = (byte)(value ? 1 : 0);
            }
        }

        public virtual void Write(BufferRegion value)
        {
            ResizeToFit(Offset + value.Length);
            value.CopyTo(Buffer.AsMemory().Slice(Offset, value.Length));
            Offset += value.Length;
        }

        public virtual void Write(byte[] value)
        {
            ResizeToFit(Offset + value.Length);
            System.Buffer.BlockCopy(value, 0, Buffer, Offset, value.Length);
            Offset += value.Length;
        }

        public virtual void Write(char[] value)
        {
            ResizeToFit(Offset + value.Length);
            foreach (char c in value)
                Buffer[Offset++] = Convert.ToByte(c);
        }

        public virtual void Write(ushort value)
        {
            ResizeToFit(Offset + 2);
            Buffer[Offset++] = (byte)(value & 0xFF);
            Buffer[Offset++] = (byte)((value >> 8) & 0xFF);
        }

        public virtual void Write(short value)
        {
            ResizeToFit(Offset + 2);
            Buffer[Offset++] = (byte)(value & 0xFF);
            Buffer[Offset++] = (byte)((value >> 8) & 0xFF);
        }

        public virtual void Write(float value)
        {
            ResizeToFit(Offset + 4);
            byte[] bytes = BitConverter.GetBytes(value);
            Buffer[Offset++] = bytes[0];
            Buffer[Offset++] = bytes[1];
            Buffer[Offset++] = bytes[2];
            Buffer[Offset++] = bytes[3];
        }
        public virtual void Write(Int24 value)
        {
            ResizeToFit(Offset + 3);
            Buffer[Offset++] = (byte)(value & 0xFF);
            Buffer[Offset++] = (byte)((value >> 8) & 0xFF);
            Buffer[Offset++] = (byte)((value >> 16) & 0xFF);
        }

        public virtual void Write(UInt24 value)
        {
            ResizeToFit(Offset + 3);
            Buffer[Offset++] = (byte)(value & 0xFF);
            Buffer[Offset++] = (byte)((value >> 8) & 0xFF);
            Buffer[Offset++] = (byte)((value >> 16) & 0xFF);
        }

        public virtual void Write(int value)
        {
            ResizeToFit(Offset + 4);
            Buffer[Offset++] = (byte)(value & 0xFF);
            Buffer[Offset++] = (byte)((value >> 8) & 0xFF);
            Buffer[Offset++] = (byte)((value >> 16) & 0xFF);
            Buffer[Offset++] = (byte)((value >> 24) & 0xFF);
        }

        public virtual void WriteAt(int pos, int value)
        {
            Buffer[pos] = (byte)(value & 0xFF);
            Buffer[pos + 1] = (byte)((value >> 8) & 0xFF);
            Buffer[pos + 2] = (byte)((value >> 16) & 0xFF);
            Buffer[pos + 3] = (byte)((value >> 24) & 0xFF);
        }

        public virtual void Write(uint value)
        {
            ResizeToFit(Offset + 4);
            Buffer[Offset++] = (byte)(value & 0xFF);
            Buffer[Offset++] = (byte)((value >> 8) & 0xFF);
            Buffer[Offset++] = (byte)((value >> 16) & 0xFF);
            Buffer[Offset++] = (byte)((value >> 24) & 0xFF);
        }

        public virtual void Write(double value)
        {
            ResizeToFit(Offset + 8);
            byte[] bytes = BitConverter.GetBytes(value);
            Buffer[Offset++] = bytes[0];
            Buffer[Offset++] = bytes[1];
            Buffer[Offset++] = bytes[2];
            Buffer[Offset++] = bytes[3];
            Buffer[Offset++] = bytes[4];
            Buffer[Offset++] = bytes[5];
            Buffer[Offset++] = bytes[6];
            Buffer[Offset++] = bytes[7];
        }

        public virtual void Write(ulong value)
        {
            ResizeToFit(Offset + 8);
            Buffer[Offset++] = (byte)(value & 0xFF);
            Buffer[Offset++] = (byte)((value >> 8) & 0xFF);
            Buffer[Offset++] = (byte)((value >> 16) & 0xFF);
            Buffer[Offset++] = (byte)((value >> 24) & 0xFF);
            Buffer[Offset++] = (byte)((value >> 32) & 0xFF);
            Buffer[Offset++] = (byte)((value >> 40) & 0xFF);
            Buffer[Offset++] = (byte)((value >> 48) & 0xFF);
            Buffer[Offset++] = (byte)((value >> 56) & 0xFF);
        }

        public virtual void Write(long value)
        {
            ResizeToFit(Offset + 8);
            Buffer[Offset++] = (byte)(value & 0xFF);
            Buffer[Offset++] = (byte)((value >> 8) & 0xFF);
            Buffer[Offset++] = (byte)((value >> 16) & 0xFF);
            Buffer[Offset++] = (byte)((value >> 24) & 0xFF);
            Buffer[Offset++] = (byte)((value >> 32) & 0xFF);
            Buffer[Offset++] = (byte)((value >> 40) & 0xFF);
            Buffer[Offset++] = (byte)((value >> 48) & 0xFF);
            Buffer[Offset++] = (byte)((value >> 56) & 0xFF);
        }

        public virtual void Flush(Stream stream) {
            stream.Write(Buffer, 0, Length);
        }

#region IDisposable Impl
        protected virtual void Dispose(bool disposing) {
            if (disposing) { }
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
#endregion
    }
}
