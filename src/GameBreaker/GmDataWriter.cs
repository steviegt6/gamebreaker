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
using System.Threading.Tasks;
using GameBreaker.Models;
using GameBreaker.Serial;
using GameBreaker.Serial.Numerics;

namespace GameBreaker
{
    public class GmDataWriter : IDataWriter
    {
        protected IBinaryWriter Writer { get; }
        
#region IBinaryWriter Impl (Properties)
        public virtual int Offset {
            get => Writer.Offset;

            set => Writer.Offset = value;
        }

        public virtual int Length => Writer.Length;

        public virtual byte[] Buffer => Writer.Buffer;

        public virtual Encoding Encoding => Writer.Encoding;

#endregion
        
#region IDataWriter Impl (Properties)
        public GMData Data { get; }

        public GMData.GMVersionInfo VersionInfo => Data.VersionInfo;

        public List<GMWarning> Warnings { get; }

        /// Maps used for tracking locations of pointer-referenced objects and the locations to patch
        public Dictionary<IGMSerializable, int> PointerOffsets { get; } = new();

        public virtual Dictionary<GMVariable, List<(int, GMCode.Bytecode.Instruction.VariableType)>> VariableReferences { get; } = new();

        public virtual Dictionary<GMFunctionEntry, List<(int, GMCode.Bytecode.Instruction.VariableType)>> FunctionReferences { get; } = new();
#endregion

        public Dictionary<IGMSerializable, List<int>> PendingPointerWrites = new ();
        public Dictionary<GMString, List<int>> PendingStringPointerWrites = new ();

        public GmDataWriter(IBinaryWriter writer, GMData data, string path)
        {
            Writer = writer;
            Data = data;
            Warnings = new List<GMWarning>();

            // Get directory of the data file for later usage
            if (path != null)
                Data.Directory = Path.GetDirectoryName(path);
        }

        public virtual void Flush(Stream stream) {
            Writer.Flush(stream);

            // Finalize all other file write operations if any exist
            Data.FileWrites.Complete();
            Data.FileWrites.Completion.GetAwaiter().GetResult();
        }

#region IBinaryWriter Impl (Methods)
        public virtual void Write(byte value) {
            Writer.Write(value);
        }

        public virtual void Write(bool value, bool wide) {
            Writer.Write(value, wide);
        }

        public virtual void Write(BufferRegion value) {
            Writer.Write(value);
        }

        public virtual void Write(byte[] value) {
            Writer.Write(value);
        }

        public virtual void Write(char[] value) {
            Writer.Write(value);
        }

        public virtual void Write(short value) {
            Writer.Write(value);
        }

        public virtual void Write(ushort value) {
            Writer.Write(value);
        }

        public virtual void Write(Int24 value) {
            Writer.Write(value);
        }

        public virtual void Write(UInt24 value) {
            Writer.Write(value);
        }

        public virtual void Write(int value) {
            Writer.Write(value);
        }

        public virtual void Write(uint value) {
            Writer.Write(value);
        }

        public virtual void Write(long value) {
            Writer.Write(value);
        }

        public virtual void Write(ulong value) {
            Writer.Write(value);
        }

        public virtual void Write(float value) {
            Writer.Write(value);
        }

        public virtual void Write(double value) {
            Writer.Write(value);
        }
#endregion

#region IDataWriter Impl (Methods)
        public void Write()
        {
            // Write the root chunk, FORM
            Write("FORM".ToCharArray());
            Data.FORM.Serialize(this);

            // Handle serialization of pointer offsets
            Parallel.ForEach(PendingPointerWrites, kvp =>
            {
                if (PointerOffsets.TryGetValue(kvp.Key, out int ptr))
                {
                    // Iterate through each reference and write the pointer
                    foreach (int addr in kvp.Value)
                        this.WriteAt(addr, ptr);
                }
                else
                {
                    // Iterate through each reference and write null
                    foreach (int addr in kvp.Value)
                        this.WriteAt(addr, 0);
                }
            });
            Parallel.ForEach(PendingStringPointerWrites, kvp =>
            {
                if (PointerOffsets.TryGetValue(kvp.Key, out int ptr))
                {
                    // Adjust offset to string contents beginning
                    ptr += 4;

                    // Iterate through each reference and write the pointer
                    foreach (int addr in kvp.Value)
                        this.WriteAt(addr, ptr);
                }
                else
                {
                    // Iterate through each reference and write null
                    foreach (int addr in kvp.Value)
                        this.WriteAt(addr, 0);
                }
            });
        }

        public virtual void WriteGMString(string value) {
            var len = Encoding.GetByteCount(value);
            Write(len);
            Encoding.GetBytes(value, 0, value.Length, Buffer, Offset);
            Offset += len;
            Buffer[Offset++] = 0;
        }

        /// <summary>
        /// Write a 32-bit pointer value in this position, for an object
        /// </summary>
        public virtual void WritePointer(IGMSerializable obj)
        {
            if (obj == null)
            {
                // This object doesn't exist, so it will never have a pointer value...
                Write(0);
                return;
            }

            // Add this location to a list for this object
            List<int> pending;
            if (PendingPointerWrites.TryGetValue(obj, out pending))
                pending.Add(Offset);
            else
                PendingPointerWrites.Add(obj, new List<int> { Offset });

            // Placeholder pointer value, will be overwritten in the future
            Write(0xBADD0660);
        }

        /// <summary>
        /// Write a 32-bit *string-only* pointer value in this position, for an object
        /// </summary>
        public virtual void WritePointerString(GMString obj)
        {
            if (obj == null)
            {
                // This string object doesn't exist, so it will never have a pointer value...
                Write(0);
                return;
            }

            // Add this location to a list for this string object
            List<int> pending;
            if (PendingStringPointerWrites.TryGetValue(obj, out pending))
                pending.Add(Offset);
            else
                PendingStringPointerWrites.Add(obj, new List<int> { Offset });

            // Placeholder pointer value, will be overwritten in the future
            Write(0xBADD0660);
        }

        /// <summary>
        /// Sets the current offset to be the pointer location for the specified object
        /// </summary>
        public virtual void WriteObjectPointer(IGMSerializable obj)
        {
            PointerOffsets.Add(obj, Offset);
        }
#endregion

#region IDispoable Impl
        protected virtual void Dispose(bool disposing) {
            if (disposing)
                Writer.Dispose();
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
#endregion
    }
}
