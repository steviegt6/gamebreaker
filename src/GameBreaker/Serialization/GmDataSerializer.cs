// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.
// Copyright (c) colinator27. Licensed under the MIT License.
// See the LICENSE-DogScepter file in the repository root for full terms and conditions.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GameBreaker.Abstractions;
using GameBreaker.Abstractions.IFF.GM;
using GameBreaker.Abstractions.Serialization;
using GameBreaker.Util.Extensions;

namespace GameBreaker.Serialization
{
    public class GmDataSerializer : IGmDataSerializer
    {
        public virtual IGameMakerFile GameMakerFile { get; } = null!;

        protected virtual IPositionableWriter Writer { get; }

        protected virtual Dictionary<IGmSerializable, int> PointerOffsets { get; } = new();

        protected virtual Dictionary<IGmSerializable, List<long>> PendingPointerWrites { get; } = new();

        protected virtual Dictionary<GmString, List<long>> PendingStringPointerWrites { get; } = new();

        // protected virtual Dictionary<GmVariable, List<int>> VariableReferences { get; } = new();

        // protected virtual Dictionary<GmFunctionEntry, List<int>> FunctionReferences { get; } = new();

        public GmDataSerializer(IPositionableWriter writer) {
            Writer = writer;
        }

        #region IGmDataSerializer Impl

        public virtual void WritePointer(IGmSerializable? ptr) {
            // If the object does not exist, write a null pointer.
            if (ptr is null) {
                Write(0);
                return;
            }

            // Add this location to a list for this object.
            if (PendingPointerWrites.TryGetValue(ptr, out List<long>? pending))
                pending.Add(Position);
            else
                PendingPointerWrites.Add(ptr, new List<long> {Position});

            // Placeholder pointer value, will be overwritten in the future.
            Write(DEADGAME);
        }

        public virtual void WritePointerString(GmString? ptr) {
            // If this string does not exist, write a null pointer.
            if (ptr is null) {
                Writer.Write(0);
                return;
            }

            if (PendingStringPointerWrites.TryGetValue(ptr, out List<long>? pending))
                pending.Add(Writer.Position);
            else
                PendingStringPointerWrites.Add(ptr, new List<long> {Writer.Position});

            // Placeholder pointer value, will be overwritten in the future.
            Writer.Write(DEADGAME);
        }

        public virtual void WriteObjectPointer(IGmSerializable ptr) {
            // TODO: Pointers are 32-bit integers - what do we do if the position exceeds 2^32-1?
            PointerOffsets.Add(ptr, (int) Writer.Position);
        }

        #endregion

        #region IPositionableWriter Impl

        public virtual Encoding Encoding => Writer.Encoding;

        public virtual long Length {
            get => Writer.Length;
            set => Writer.Length = value;
        }

        public virtual long Position {
            get => Writer.Position;
            set => Writer.Position = value;
        }

        public virtual void Write(byte value) {
            Writer.Write(value);
        }

        public virtual void Write(byte[] value) {
            Writer.Write(value);
        }

        public virtual void Write(bool value) {
            Writer.Write(value);
        }

        public virtual void Write(char value) {
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

        public virtual void Write(GmString value) {
            Writer.Write(value);
        }

        #endregion

        #region IDisposable Impl

        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                Writer.Dispose();
            }
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        protected virtual void PerformPointerWrite(KeyValuePair<IGmSerializable, List<long>> kvp) {
            if (PointerOffsets.TryGetValue(kvp.Key, out int ptr))
                foreach (long addr in kvp.Value)
                    this.WriteAt(addr, ptr);
            else
                foreach (long addr in kvp.Value)
                    this.WriteAt(addr, 0);
        }

        protected virtual void PerformStringPointerWrite(KeyValuePair<GmString, List<long>> kvp) {
            if (PointerOffsets.TryGetValue(kvp.Key, out int ptr)) {
                // Skip length
                ptr += 4;

                foreach (long addr in kvp.Value) this.WriteAt(addr, ptr);
            }
            else
                // If the string doesn't exist, write null
                foreach (long addr in kvp.Value)
                    this.WriteAt(addr, 0);
        }
    }
}