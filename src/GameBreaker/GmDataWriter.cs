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
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GameBreaker.Models;
using GameBreaker.Serial;
using GameBreaker.Serial.Numerics;

namespace GameBreaker {
    public sealed class GmDataWriter : IDataWriter {
        private int offset;
        private int length;
        private byte[] buffer;
        private Encoding encoding;

        public int Offset {
            get => offset;
            set => offset = value;
        }

        public int Length => length;

        public byte[] Buffer => buffer;

        public Encoding Encoding => encoding;

#region IDataWriter Impl (Properties)
        public GMData Data { get; }

        public GMData.GMVersionInfo VersionInfo => Data.VersionInfo;

        public List<GMWarning> Warnings { get; }

        /// Maps used for tracking locations of pointer-referenced objects and the locations to patch
        public Dictionary<IGMSerializable, int> PointerOffsets { get; } = new();

        public Dictionary<GMVariable,
                List<(int, GMCode.Bytecode.Instruction.VariableType)>>
            VariableReferences { get; } = new();

        public Dictionary<GMFunctionEntry,
                List<(int, GMCode.Bytecode.Instruction.VariableType)>>
            FunctionReferences { get; } = new();
#endregion

        public Dictionary<IGMSerializable, List<int>> PendingPointerWrites =
            new ();

        public Dictionary<GMString, List<int>> PendingStringPointerWrites =
            new ();

        public GmDataWriter(
            int size,
            GMData data,
            string path,
            Encoding? encoding = null
        ) {
            buffer = new byte[size];
            this.encoding = encoding ?? IEncodable.DEFAULT_ENCODING;

            Data = data;
            Warnings = new List<GMWarning>();

            // Get directory of the data file for later usage
            if (path != null)
                Data.Directory = Path.GetDirectoryName(path);
        }

        public void Flush(Stream stream) {
            stream.Write(buffer, 0, length);

            // Finalize all other file write operations if any exist
            Data.FileWrites.Complete();
            Data.FileWrites.Completion.GetAwaiter().GetResult();
        }

#region IBinaryWriter Impl (Methods)
        // TODO: Provide option to resize to `size` instead of `Length` * 2?
        // Mostly for systems with lower memory that may want to minimize
        // allocations. Detrimental to speed, however. It's a tradeoff.
        private void EnsureCapacity(int size) {
            if (length >= size)
                return;

            var newSize = Math.Max(length * 2, size);
            Array.Resize(ref buffer, newSize);
            length = size;
        }

        /// <inheritdoc cref="IBinaryWriter.Write(byte)"/>
        public void Write(byte value) {
            EnsureCapacity(offset + sizeof(byte));
            ref var b = ref buffer[offset];
            Unsafe.As<byte, byte>(ref b) = value;
            offset += sizeof(byte);
        }

        /// <inheritdoc cref="IBinaryWriter.Write(bool,bool)"/>
        public void Write(bool value, bool wide) {
            if (wide)
                Write(value ? 1 : 0);
            else
                Write((byte) (value ? 1 : 0));
        }

        /// <inheritdoc cref="IBinaryWriter.Write(BufferRegion)"/>
        public void Write(BufferRegion value) {
            EnsureCapacity(offset + value.Length);
            value.CopyTo(buffer.AsMemory().Slice(offset, value.Length));
            offset += value.Length;
        }

        /// <inheritdoc cref="IBinaryWriter.Write(byte[])"/>
        public void Write(byte[] value) {
            EnsureCapacity(offset + value.Length);
            System.Buffer.BlockCopy(value, 0, buffer, offset, value.Length);
            offset += value.Length;
        }

        /// <inheritdoc cref="IBinaryWriter.Write(char[])"/>
        public void Write(char[] value) {
            EnsureCapacity(offset + value.Length);

            // TODO: Specific encoding would be nice. What to do?
            foreach (var c in value)
                buffer[offset++] = Convert.ToByte(c);
        }

        /// <inheritdoc cref="IBinaryWriter.Write(short)"/>
        public void Write(short value) {
            EnsureCapacity(offset + sizeof(short));
            ref var b = ref buffer[offset];
            Unsafe.As<byte, short>(ref b) = value;
            offset += sizeof(short);
        }

        /// <inheritdoc cref="IBinaryWriter.Write(ushort)"/>
        public void Write(ushort value) {
            EnsureCapacity(offset + sizeof(ushort));
            ref var b = ref buffer[offset];
            Unsafe.As<byte, ushort>(ref b) = value;
            offset += sizeof(ushort);
        }

        /// <inheritdoc cref="IBinaryWriter.Write(Int24)"/>
        public void Write(Int24 value) {
            EnsureCapacity(offset + Int24.SIZE);
            ref var b = ref buffer[offset];
            Unsafe.As<byte, Int24>(ref b) = value;
            offset += Int24.SIZE;
        }

        /// <inheritdoc cref="IBinaryWriter.Write(UInt24)"/>
        public void Write(UInt24 value) {
            EnsureCapacity(offset + UInt24.SIZE);
            ref var b = ref buffer[offset];
            Unsafe.As<byte, UInt24>(ref b) = value;
            offset += UInt24.SIZE;
        }

        /// <inheritdoc cref="IBinaryWriter.Write(int)"/>
        public void Write(int value) {
            EnsureCapacity(offset + sizeof(int));
            ref var b = ref buffer[offset];
            Unsafe.As<byte, int>(ref b) = value;
            offset += sizeof(int);
        }

        /// <inheritdoc cref="IBinaryWriter.Write(uint)"/>
        public void Write(uint value) {
            EnsureCapacity(offset + sizeof(uint));
            ref var b = ref buffer[offset];
            Unsafe.As<byte, uint>(ref b) = value;
            offset += sizeof(uint);
        }

        /// <inheritdoc cref="IBinaryWriter.Write(long)"/>
        public void Write(long value) {
            EnsureCapacity(offset + sizeof(long));
            ref var b = ref buffer[offset];
            Unsafe.As<byte, long>(ref b) = value;
            offset += sizeof(long);
        }

        /// <inheritdoc cref="IBinaryWriter.Write(ulong)"/>
        public void Write(ulong value) {
            EnsureCapacity(offset + sizeof(ulong));
            ref var b = ref buffer[offset];
            Unsafe.As<byte, ulong>(ref b) = value;
            offset += sizeof(ulong);
        }

        /// <inheritdoc cref="IBinaryWriter.Write(float)"/>
        public void Write(float value) {
            EnsureCapacity(offset + sizeof(float));
            ref var b = ref buffer[offset];
            Unsafe.As<byte, float>(ref b) = value;
            offset += sizeof(float);
        }

        /// <inheritdoc cref="IBinaryWriter.Write(double)"/>
        public void Write(double value) {
            EnsureCapacity(offset + sizeof(double));
            ref var b = ref buffer[offset];
            Unsafe.As<byte, double>(ref b) = value;
            offset += sizeof(double);
        }
#endregion

#region IDataWriter Impl (Methods)
        public void Write() {
            // Write the root chunk, FORM
            Write("FORM".ToCharArray());
            Data.FORM.Serialize(this);

            // Handle serialization of pointer offsets
            Parallel.ForEach(PendingPointerWrites,
                             kvp => {
                                 if (PointerOffsets.TryGetValue(
                                         kvp.Key,
                                         out int ptr)) {
                                     // Iterate through each reference and write the pointer
                                     foreach (int addr in kvp.Value)
                                         this.WriteAt(addr, ptr);
                                 }
                                 else {
                                     // Iterate through each reference and write null
                                     foreach (int addr in kvp.Value)
                                         this.WriteAt(addr, 0);
                                 }
                             });
            Parallel.ForEach(PendingStringPointerWrites,
                             kvp => {
                                 if (PointerOffsets.TryGetValue(
                                         kvp.Key,
                                         out int ptr)) {
                                     // Adjust offset to string contents beginning
                                     ptr += 4;

                                     // Iterate through each reference and write the pointer
                                     foreach (int addr in kvp.Value)
                                         this.WriteAt(addr, ptr);
                                 }
                                 else {
                                     // Iterate through each reference and write null
                                     foreach (int addr in kvp.Value)
                                         this.WriteAt(addr, 0);
                                 }
                             });
        }

        public void WriteGMString(string value) {
            var len = encoding.GetByteCount(value);
            Write(len);
            encoding.GetBytes(value, 0, value.Length, buffer, offset);
            offset += len;
            buffer[offset++] = 0;
        }

        /// <summary>
        /// Write a 32-bit pointer value in this position, for an object
        /// </summary>
        public void WritePointer(IGMSerializable obj) {
            if (obj == null) {
                // This object doesn't exist, so it will never have a pointer value...
                Write(0);
                return;
            }

            // Add this location to a list for this object
            List<int> pending;
            if (PendingPointerWrites.TryGetValue(obj, out pending))
                pending.Add(offset);
            else
                PendingPointerWrites.Add(obj,
                                         new List<int> {
                                             offset
                                         });

            // Placeholder pointer value, will be overwritten in the future
            Write(0xBADD0660);
        }

        /// <summary>
        /// Write a 32-bit *string-only* pointer value in this position, for an object
        /// </summary>
        public void WritePointerString(GMString obj) {
            if (obj == null) {
                // This string object doesn't exist, so it will never have a pointer value...
                Write(0);
                return;
            }

            // Add this location to a list for this string object
            List<int> pending;
            if (PendingStringPointerWrites.TryGetValue(obj, out pending))
                pending.Add(offset);
            else
                PendingStringPointerWrites.Add(obj,
                                               new List<int> {
                                                   offset
                                               });

            // Placeholder pointer value, will be overwritten in the future
            Write(0xBADD0660);
        }

        /// <summary>
        /// Sets the current offset to be the pointer location for the specified object
        /// </summary>
        public void WriteObjectPointer(IGMSerializable obj) {
            PointerOffsets.Add(obj, offset);
        }
#endregion

#region IDispoable Impl
        void IDisposable.Dispose() { }
#endregion
    }
}
