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

namespace GameBreaker.Serial
{
    public class BufferBinaryWriter : IPositionable, IEncodable, IDisposable
    {
        private readonly Stream stream;
        private byte[] buffer;
        private int offset;
        private int currentSize;
        private Encoding encoding;

        public int Offset
        {
            get => offset; set
            {
                ResizeToFit(value);
                offset = value;
            }
        }

        public int Length => currentSize;

        public byte[] Buffer { get => buffer; }
        public Encoding Encoding { get => encoding; }

        public BufferBinaryWriter(Stream stream, int baseSize = 1024 * 1024 * 32)
        {
            this.stream = stream;
            buffer = new byte[baseSize];
            currentSize = 0;
            offset = 0;

            encoding = new UTF8Encoding(false);
        }

        private void ResizeToFit(int size)
        {
            while (size > buffer.Length)
                Array.Resize(ref buffer, buffer.Length * 2);
            if (currentSize < size)
                currentSize = size;
        }

        public void Write(byte value)
        {
            ResizeToFit(offset + 1);
            buffer[offset++] = value;
        }

        public void Write(sbyte value)
        {
            ResizeToFit(offset + 1);
            buffer[offset++] = (byte)value;
        }

        public virtual void Write(bool value)
        {
            ResizeToFit(offset + 1);
            buffer[offset++] = (byte)(value ? 1 : 0);
        }

        public void Write(BufferRegion value)
        {
            ResizeToFit(offset + value.Length);
            value.Memory.CopyTo(buffer.AsMemory().Slice(Offset, value.Length));
            offset += value.Length;
        }

        public void Write(byte[] value)
        {
            ResizeToFit(offset + value.Length);
            System.Buffer.BlockCopy(value, 0, buffer, offset, value.Length);
            offset += value.Length;
        }

        public void Write(char[] value)
        {
            ResizeToFit(offset + value.Length);
            foreach (char c in value)
                buffer[offset++] = Convert.ToByte(c);
        }

        public void Write(ushort value)
        {
            ResizeToFit(offset + 2);
            buffer[offset++] = (byte)(value & 0xFF);
            buffer[offset++] = (byte)((value >> 8) & 0xFF);
        }

        public void Write(short value)
        {
            ResizeToFit(offset + 2);
            buffer[offset++] = (byte)(value & 0xFF);
            buffer[offset++] = (byte)((value >> 8) & 0xFF);
        }

        public void Write(float value)
        {
            ResizeToFit(offset + 4);
            byte[] bytes = BitConverter.GetBytes(value);
            buffer[offset++] = bytes[0];
            buffer[offset++] = bytes[1];
            buffer[offset++] = bytes[2];
            buffer[offset++] = bytes[3];
        }
        public void WriteInt24(int value)
        {
            ResizeToFit(offset + 3);
            buffer[offset++] = (byte)(value & 0xFF);
            buffer[offset++] = (byte)((value >> 8) & 0xFF);
            buffer[offset++] = (byte)((value >> 16) & 0xFF);
        }

        public void WriteUInt24(uint value)
        {
            ResizeToFit(offset + 3);
            buffer[offset++] = (byte)(value & 0xFF);
            buffer[offset++] = (byte)((value >> 8) & 0xFF);
            buffer[offset++] = (byte)((value >> 16) & 0xFF);
        }

        public void Write(int value)
        {
            ResizeToFit(offset + 4);
            buffer[offset++] = (byte)(value & 0xFF);
            buffer[offset++] = (byte)((value >> 8) & 0xFF);
            buffer[offset++] = (byte)((value >> 16) & 0xFF);
            buffer[offset++] = (byte)((value >> 24) & 0xFF);
        }

        public void WriteAt(int pos, int value)
        {
            buffer[pos] = (byte)(value & 0xFF);
            buffer[pos + 1] = (byte)((value >> 8) & 0xFF);
            buffer[pos + 2] = (byte)((value >> 16) & 0xFF);
            buffer[pos + 3] = (byte)((value >> 24) & 0xFF);
        }

        public void Write(uint value)
        {
            ResizeToFit(offset + 4);
            buffer[offset++] = (byte)(value & 0xFF);
            buffer[offset++] = (byte)((value >> 8) & 0xFF);
            buffer[offset++] = (byte)((value >> 16) & 0xFF);
            buffer[offset++] = (byte)((value >> 24) & 0xFF);
        }

        public void Write(double value)
        {
            ResizeToFit(offset + 8);
            byte[] bytes = BitConverter.GetBytes(value);
            buffer[offset++] = bytes[0];
            buffer[offset++] = bytes[1];
            buffer[offset++] = bytes[2];
            buffer[offset++] = bytes[3];
            buffer[offset++] = bytes[4];
            buffer[offset++] = bytes[5];
            buffer[offset++] = bytes[6];
            buffer[offset++] = bytes[7];
        }

        public void Write(ulong value)
        {
            ResizeToFit(offset + 8);
            buffer[offset++] = (byte)(value & 0xFF);
            buffer[offset++] = (byte)((value >> 8) & 0xFF);
            buffer[offset++] = (byte)((value >> 16) & 0xFF);
            buffer[offset++] = (byte)((value >> 24) & 0xFF);
            buffer[offset++] = (byte)((value >> 32) & 0xFF);
            buffer[offset++] = (byte)((value >> 40) & 0xFF);
            buffer[offset++] = (byte)((value >> 48) & 0xFF);
            buffer[offset++] = (byte)((value >> 56) & 0xFF);
        }

        public void Write(long value)
        {
            ResizeToFit(offset + 8);
            buffer[offset++] = (byte)(value & 0xFF);
            buffer[offset++] = (byte)((value >> 8) & 0xFF);
            buffer[offset++] = (byte)((value >> 16) & 0xFF);
            buffer[offset++] = (byte)((value >> 24) & 0xFF);
            buffer[offset++] = (byte)((value >> 32) & 0xFF);
            buffer[offset++] = (byte)((value >> 40) & 0xFF);
            buffer[offset++] = (byte)((value >> 48) & 0xFF);
            buffer[offset++] = (byte)((value >> 56) & 0xFF);
        }

        public void WriteGMString(string value)
        {
            int len = encoding.GetByteCount(value);
            ResizeToFit(offset + len + 5);
            buffer[offset++] = (byte)(len & 0xFF);
            buffer[offset++] = (byte)((len >> 8) & 0xFF);
            buffer[offset++] = (byte)((len >> 16) & 0xFF);
            buffer[offset++] = (byte)((len >> 24) & 0xFF);
            encoding.GetBytes(value, 0, value.Length, buffer, offset);
            offset += len;
            buffer[offset++] = 0;
        }

        public virtual void Flush()
        {
            stream.Write(buffer, 0, currentSize);
        }

        public void Dispose()
        {
            stream.Close();
        }
    }
}
