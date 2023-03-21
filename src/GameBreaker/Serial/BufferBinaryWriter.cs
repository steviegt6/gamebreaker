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
using System.IO;
using System.Text;
using GameBreaker.Serial.Numerics;

namespace GameBreaker.Serial {
    /// <summary>
    ///     An implementation of <see cref="IBinaryWriter"/> that uses a raw
    ///     byte array as its backing buffer, flushable to a
    ///     <see cref="Stream"/>.
    /// </summary>
    public class BufferBinaryWriter : IBinaryWriter {
        /// <summary>
        ///     The default size to initialize the buffer.
        /// </summary>
        public const int DEFAULT_SIZE = 1024 * 1024 * 32;

        private byte[] buffer;
        private int offset;

        /// <inheritdoc cref="IBinaryWriter.Offset"/>
        public int Offset {
            get => offset;
            set => EnsureCapacity(offset = value);
        }

        /// <inheritdoc cref="IBinaryWriter.Length"/>
        public int Length { get; set; }

        /// <inheritdoc cref="IBinaryWriter.Buffer"/>
        public byte[] Buffer => buffer;

        /// <inheritdoc cref="IBinaryWriter.Encoding"/>
        public Encoding Encoding { get; }

        /// <summary>
        ///     Initializes a new instance of <see cref="BufferBinaryWriter"/>.
        /// </summary>
        /// <param name="baseSize">
        ///     The base size to initialize the buffer.
        /// </param>
        public BufferBinaryWriter(int baseSize = DEFAULT_SIZE) {
            buffer = new byte[baseSize];
            Offset = 0;

            Encoding = new UTF8Encoding(false);
        }

        // TODO: Provide option to resize to `size` instead of `Length` * 2?
        // Mostly for systems with lower memory that may want to minimize
        // allocations. Detrimental to speed, however. It's a tradeoff.
        private void EnsureCapacity(int size) {
            if (Buffer.Length >= size)
                return;

            var newSize = Math.Max(Buffer.Length * 2, size);
            Array.Resize(ref buffer, newSize);
            Length = size;
        }

        /// <inheritdoc cref="IBinaryWriter.Write(byte)"/>
        public virtual void Write(byte value) {
            EnsureCapacity(Offset + 1);
            Buffer[Offset++] = value;
        }

        /// <inheritdoc cref="IBinaryWriter.Write(bool,bool)"/>
        public virtual void Write(bool value, bool wide) {
            if (wide) {
                Write(value ? 1 : 0);
            }
            else {
                EnsureCapacity(Offset + 1);
                Buffer[Offset++] = (byte)(value ? 1 : 0);
            }
        }

        /// <inheritdoc cref="IBinaryWriter.Write(BufferRegion)"/>
        public virtual void Write(BufferRegion value) {
            EnsureCapacity(Offset + value.Length);
            value.CopyTo(Buffer.AsMemory().Slice(Offset, value.Length));
            Offset += value.Length;
        }

        /// <inheritdoc cref="IBinaryWriter.Write(byte[])"/>
        public virtual void Write(byte[] value) {
            EnsureCapacity(Offset + value.Length);
            System.Buffer.BlockCopy(value, 0, Buffer, Offset, value.Length);
            Offset += value.Length;
        }

        /// <inheritdoc cref="IBinaryWriter.Write(char[])"/>
        public virtual void Write(char[] value) {
            EnsureCapacity(Offset + value.Length);
            foreach (char c in value)
                Buffer[Offset++] = Convert.ToByte(c);
        }
        
        /// <inheritdoc cref="IBinaryWriter.Write(short)"/>
        public virtual void Write(short value) {
            EnsureCapacity(Offset + 2);
            Buffer[Offset++] = (byte)(value & 0xFF);
            Buffer[Offset++] = (byte)((value >> 8) & 0xFF);
        }

        /// <inheritdoc cref="IBinaryWriter.Write(ushort)"/>
        public virtual void Write(ushort value) {
            EnsureCapacity(Offset + 2);
            Buffer[Offset++] = (byte)(value & 0xFF);
            Buffer[Offset++] = (byte)((value >> 8) & 0xFF);
        }

        /// <inheritdoc cref="IBinaryWriter.Write(Int24)"/>
        public virtual void Write(Int24 value) {
            EnsureCapacity(Offset + 3);
            Buffer[Offset++] = (byte)(value & 0xFF);
            Buffer[Offset++] = (byte)((value >> 8) & 0xFF);
            Buffer[Offset++] = (byte)((value >> 16) & 0xFF);
        }

        /// <inheritdoc cref="IBinaryWriter.Write(UInt24)"/>
        public virtual void Write(UInt24 value) {
            EnsureCapacity(Offset + 3);
            Buffer[Offset++] = (byte)(value & 0xFF);
            Buffer[Offset++] = (byte)((value >> 8) & 0xFF);
            Buffer[Offset++] = (byte)((value >> 16) & 0xFF);
        }

        /// <inheritdoc cref="IBinaryWriter.Write(int)"/>
        public virtual void Write(int value) {
            EnsureCapacity(Offset + 4);
            Buffer[Offset++] = (byte)(value & 0xFF);
            Buffer[Offset++] = (byte)((value >> 8) & 0xFF);
            Buffer[Offset++] = (byte)((value >> 16) & 0xFF);
            Buffer[Offset++] = (byte)((value >> 24) & 0xFF);
        }

        /// <inheritdoc cref="IBinaryWriter.Write(uint)"/>
        public virtual void Write(uint value) {
            EnsureCapacity(Offset + 4);
            Buffer[Offset++] = (byte)(value & 0xFF);
            Buffer[Offset++] = (byte)((value >> 8) & 0xFF);
            Buffer[Offset++] = (byte)((value >> 16) & 0xFF);
            Buffer[Offset++] = (byte)((value >> 24) & 0xFF);
        }
        
        /// <inheritdoc cref="IBinaryWriter.Write(long)"/>
        public virtual void Write(long value) {
            EnsureCapacity(Offset + 8);
            Buffer[Offset++] = (byte)(value & 0xFF);
            Buffer[Offset++] = (byte)((value >> 8) & 0xFF);
            Buffer[Offset++] = (byte)((value >> 16) & 0xFF);
            Buffer[Offset++] = (byte)((value >> 24) & 0xFF);
            Buffer[Offset++] = (byte)((value >> 32) & 0xFF);
            Buffer[Offset++] = (byte)((value >> 40) & 0xFF);
            Buffer[Offset++] = (byte)((value >> 48) & 0xFF);
            Buffer[Offset++] = (byte)((value >> 56) & 0xFF);
        }

        /// <inheritdoc cref="IBinaryWriter.Write(ulong)"/>
        public virtual void Write(ulong value) {
            EnsureCapacity(Offset + 8);
            Buffer[Offset++] = (byte)(value & 0xFF);
            Buffer[Offset++] = (byte)((value >> 8) & 0xFF);
            Buffer[Offset++] = (byte)((value >> 16) & 0xFF);
            Buffer[Offset++] = (byte)((value >> 24) & 0xFF);
            Buffer[Offset++] = (byte)((value >> 32) & 0xFF);
            Buffer[Offset++] = (byte)((value >> 40) & 0xFF);
            Buffer[Offset++] = (byte)((value >> 48) & 0xFF);
            Buffer[Offset++] = (byte)((value >> 56) & 0xFF);
        }

        /// <inheritdoc cref="IBinaryWriter.Write(float)"/>
        public virtual void Write(float value) {
            EnsureCapacity(Offset + 4);
            byte[] bytes = BitConverter.GetBytes(value);
            Buffer[Offset++] = bytes[0];
            Buffer[Offset++] = bytes[1];
            Buffer[Offset++] = bytes[2];
            Buffer[Offset++] = bytes[3];
        }

        /// <inheritdoc cref="IBinaryWriter.Write(double)"/>
        public virtual void Write(double value) {
            EnsureCapacity(Offset + 8);
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

        /// <inheritdoc cref="IBinaryWriter.Flush"/>
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
