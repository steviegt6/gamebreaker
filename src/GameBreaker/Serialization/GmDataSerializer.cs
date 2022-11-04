// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.
// Copyright (c) colinator27. Licensed under the MIT License.
// See the LICENSE-DogScepter file in the repository root for full terms and conditions.

using System.Collections.Generic;
using System.Threading.Tasks;
using GameBreaker.Abstractions;
using GameBreaker.Abstractions.IFF;
using GameBreaker.Abstractions.Serialization;
using GameBreaker.Util.Extensions;

namespace GameBreaker.Serialization
{
    public class GmDataSerializer : IGmDataSerializer
    {
        public virtual IGmIFF Iff { get; } = null!;

        public virtual IPositionableWriter Writer { get; }

        protected virtual Dictionary<IGmSerializable, int> PointerOffsets { get; } = new();

        protected virtual Dictionary<IGmSerializable, List<long>> PendingPointerWrites { get; } = new();

        protected virtual Dictionary<GmString, List<long>> PendingStringPointerWrites { get; } = new();

        // protected virtual Dictionary<GmVariable, List<int>> VariableReferences { get; } = new();

        // protected virtual Dictionary<GmFunctionEntry, List<int>> FunctionReferences { get; } = new();

        public GmDataSerializer(IPositionableWriter writer) {
            Writer = writer;

            writer.OnFlush += _ =>
            {
                // Finalize all other file write operations if any exist.
                Iff.FileWrites.Complete();
                Iff.FileWrites.Completion.GetAwaiter().GetResult();
            };
        }

        public virtual void SerializeData() {
            Iff.Root.Serialize(this);

            // Handle serialization of pointer offsets
            Parallel.ForEach(PendingPointerWrites, PerformPointerWrite);
            Parallel.ForEach(PendingStringPointerWrites, PerformStringPointerWrite);
        }

        public virtual void WritePointer(IGmSerializable? ptr) {
            // If the object does not exist, write a null pointer.
            if (ptr is null) {
                Writer.Write(0);
                return;
            }

            // Add this location to a list for this object.
            if (PendingPointerWrites.TryGetValue(ptr, out List<long>? pending))
                pending.Add(Writer.Position);
            else
                PendingPointerWrites.Add(ptr, new List<long> {Writer.Position});

            // Placeholder pointer value, will be overwritten in the future.
            Writer.Write(DEADGAME);
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

        protected virtual void PerformPointerWrite(KeyValuePair<IGmSerializable, List<long>> kvp) {
            if (PointerOffsets.TryGetValue(kvp.Key, out int ptr))
                foreach (long addr in kvp.Value)
                    Writer.WriteAt(addr, ptr);
            else
                foreach (long addr in kvp.Value)
                    Writer.WriteAt(addr, 0);
        }

        protected virtual void PerformStringPointerWrite(KeyValuePair<GmString, List<long>> kvp) {
            if (PointerOffsets.TryGetValue(kvp.Key, out int ptr)) {
                // Skip length
                ptr += 4;

                foreach (long addr in kvp.Value) Writer.WriteAt(addr, ptr);
            }
            else
                // If the string doesn't exist, write null
                foreach (long addr in kvp.Value)
                    Writer.WriteAt(addr, 0);
        }
    }
}