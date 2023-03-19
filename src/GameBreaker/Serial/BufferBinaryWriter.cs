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
    public class BufferBinaryWriter : IBinaryWriter, IDisposable {
        public const int DEFAULT_SIZE = 1024 * 1024 * 32;
        
        private readonly Stream stream;
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

        public BufferBinaryWriter(Stream stream, int baseSize = DEFAULT_SIZE)
        {
            this.stream = stream;
            buffer = new byte[baseSize];
            offset = 0;

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
            ResizeToFit(offset + 1);
            Buffer[offset++] = value;
        }

        public virtual void Write(bool value, bool wide)
        {
            if (wide) {
                Write(value ? 1 : 0);
            }
            else {
                ResizeToFit(offset + 1);
                Buffer[offset++] = (byte)(value ? 1 : 0);
            }
        }

        public virtual void Write(BufferRegion value)
        {
            ResizeToFit(offset + value.Length);
            value.CopyTo(Buffer.AsMemory().Slice(Offset, value.Length));
            offset += value.Length;
        }

        public virtual void Write(byte[] value)
        {
            ResizeToFit(offset + value.Length);
            System.Buffer.BlockCopy(value, 0, Buffer, offset, value.Length);
            offset += value.Length;
        }

        public virtual void Write(char[] value)
        {
            ResizeToFit(offset + value.Length);
            foreach (char c in value)
                Buffer[offset++] = Convert.ToByte(c);
        }

        public virtual void Write(ushort value)
        {
            ResizeToFit(offset + 2);
            Buffer[offset++] = (byte)(value & 0xFF);
            Buffer[offset++] = (byte)((value >> 8) & 0xFF);
        }

        public virtual void Write(short value)
        {
            ResizeToFit(offset + 2);
            Buffer[offset++] = (byte)(value & 0xFF);
            Buffer[offset++] = (byte)((value >> 8) & 0xFF);
        }

        public virtual void Write(float value)
        {
            ResizeToFit(offset + 4);
            byte[] bytes = BitConverter.GetBytes(value);
            Buffer[offset++] = bytes[0];
            Buffer[offset++] = bytes[1];
            Buffer[offset++] = bytes[2];
            Buffer[offset++] = bytes[3];
        }
        public virtual void Write(Int24 value)
        {
            ResizeToFit(offset + 3);
            Buffer[offset++] = (byte)(value & 0xFF);
            Buffer[offset++] = (byte)((value >> 8) & 0xFF);
            Buffer[offset++] = (byte)((value >> 16) & 0xFF);
        }

        public virtual void Write(UInt24 value)
        {
            ResizeToFit(offset + 3);
            Buffer[offset++] = (byte)(value & 0xFF);
            Buffer[offset++] = (byte)((value >> 8) & 0xFF);
            Buffer[offset++] = (byte)((value >> 16) & 0xFF);
        }

        public virtual void Write(int value)
        {
            ResizeToFit(offset + 4);
            Buffer[offset++] = (byte)(value & 0xFF);
            Buffer[offset++] = (byte)((value >> 8) & 0xFF);
            Buffer[offset++] = (byte)((value >> 16) & 0xFF);
            Buffer[offset++] = (byte)((value >> 24) & 0xFF);
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
            ResizeToFit(offset + 4);
            Buffer[offset++] = (byte)(value & 0xFF);
            Buffer[offset++] = (byte)((value >> 8) & 0xFF);
            Buffer[offset++] = (byte)((value >> 16) & 0xFF);
            Buffer[offset++] = (byte)((value >> 24) & 0xFF);
        }

        public virtual void Write(double value)
        {
            ResizeToFit(offset + 8);
            byte[] bytes = BitConverter.GetBytes(value);
            Buffer[offset++] = bytes[0];
            Buffer[offset++] = bytes[1];
            Buffer[offset++] = bytes[2];
            Buffer[offset++] = bytes[3];
            Buffer[offset++] = bytes[4];
            Buffer[offset++] = bytes[5];
            Buffer[offset++] = bytes[6];
            Buffer[offset++] = bytes[7];
        }

        public virtual void Write(ulong value)
        {
            ResizeToFit(offset + 8);
            Buffer[offset++] = (byte)(value & 0xFF);
            Buffer[offset++] = (byte)((value >> 8) & 0xFF);
            Buffer[offset++] = (byte)((value >> 16) & 0xFF);
            Buffer[offset++] = (byte)((value >> 24) & 0xFF);
            Buffer[offset++] = (byte)((value >> 32) & 0xFF);
            Buffer[offset++] = (byte)((value >> 40) & 0xFF);
            Buffer[offset++] = (byte)((value >> 48) & 0xFF);
            Buffer[offset++] = (byte)((value >> 56) & 0xFF);
        }

        public virtual void Write(long value)
        {
            ResizeToFit(offset + 8);
            Buffer[offset++] = (byte)(value & 0xFF);
            Buffer[offset++] = (byte)((value >> 8) & 0xFF);
            Buffer[offset++] = (byte)((value >> 16) & 0xFF);
            Buffer[offset++] = (byte)((value >> 24) & 0xFF);
            Buffer[offset++] = (byte)((value >> 32) & 0xFF);
            Buffer[offset++] = (byte)((value >> 40) & 0xFF);
            Buffer[offset++] = (byte)((value >> 48) & 0xFF);
            Buffer[offset++] = (byte)((value >> 56) & 0xFF);
        }

        public virtual void WriteGMString(string value)
        {
            int len = Encoding.GetByteCount(value);
            ResizeToFit(offset + len + 5);
            Buffer[offset++] = (byte)(len & 0xFF);
            Buffer[offset++] = (byte)((len >> 8) & 0xFF);
            Buffer[offset++] = (byte)((len >> 16) & 0xFF);
            Buffer[offset++] = (byte)((len >> 24) & 0xFF);
            Encoding.GetBytes(value, 0, value.Length, Buffer, offset);
            offset += len;
            Buffer[offset++] = 0;
        }

        public virtual void Flush()
        {
            stream.Write(Buffer, 0, Length);
        }

        public void Dispose()
        {
            stream.Close();
        }
    }
}
