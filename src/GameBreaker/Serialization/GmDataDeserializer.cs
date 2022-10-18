// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.
// Copyright (c) colinator27. Licensed under the MIT License.
// See the LICENSE-DogScepter file in the repository root for full terms and conditions.

using System.Collections.Generic;
using GameBreaker.Abstractions;
using GameBreaker.Abstractions.Serialization;

namespace GameBreaker.Serialization
{
    public class GmDataDeserializer : IGmDataDeserializer
    {
        public virtual IGmData Data { get; } = null!;

        public virtual IPositionableReader Reader { get; }

        protected virtual Dictionary<int, IGmSerializable> PointerOffsets { get; }
        // protected virtual Dictionary<int, Instruction> Instructions { get; }

        public GmDataDeserializer(IPositionableReader reader) {
            Reader = reader;
        }

        public virtual void DeserializeData() {
            throw new System.NotImplementedException();
        }

        public virtual T ReadPointer<T>(int ptr)
            where T : IGmSerializable, new() {
            throw new System.NotImplementedException();
        }

        public virtual T ReadPointer<T>()
            where T : IGmSerializable, new() {
            throw new System.NotImplementedException();
        }

        public virtual T ReadPointerObject<T>(int ptr, bool returnAfter = true)
            where T : IGmSerializable, new() {
            throw new System.NotImplementedException();
        }

        public virtual T ReadPointerObject<T>()
            where T : IGmSerializable, new() {
            throw new System.NotImplementedException();
        }

        public virtual T ReadPointerObjectUnique<T>(int ptr, bool returnAfter = true)
            where T : IGmSerializable, new() {
            throw new System.NotImplementedException();
        }

        public virtual T ReadPointerObjectUnique<T>()
            where T : IGmSerializable, new() {
            throw new System.NotImplementedException();
        }

        public virtual GmString ReadStringPointer() {
            throw new System.NotImplementedException();
        }

        public virtual GmString ReadStringPointerObject() {
            throw new System.NotImplementedException();
        }
    }
}