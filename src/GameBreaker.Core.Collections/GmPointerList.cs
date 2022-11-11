// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using GameBreaker.Core.Abstractions.Serialization;
using GameBreaker.Core.Collections.Abstractions;

// ReSharper disable MethodOverloadWithOptionalParameter

namespace GameBreaker.Core.Collections;

public class GmPointerList<T> : GmList<T>, IGmPointerList<T>
    where T : IGmSerializable, new()
{
    public virtual bool UsePointerMap => true;

    public virtual void Serialize(
        IGmDataSerializer serializer,
        SerializeCollectionDelegate? before = null,
        SerializeCollectionDelegate? after = null,
        SerializeCollectionElementDelegate? elementWriter = null,
        SerializeCollectionElementDelegate? elementPointerWriter = null
    ) {
        elementWriter ??= (s, e) => e.Serialize(s);
        elementPointerWriter ??= (s, e) => s.WritePointer(e);
        serializer.Write(Count);

        // Write pointers.
        for (int i = 0; i < Count; i++) elementPointerWriter(serializer, this[i]);

        // Write elements.
        for (int i = 0; i < Count; i++) {
            before?.Invoke(serializer, i, Count);
            elementWriter(serializer, this[i]);
            after?.Invoke(serializer, i, Count);
        }
    }

    public virtual void Deserialize(
        IGmDataDeserializer deserializer,
        DeserializeCollectionDelegate? before = null,
        DeserializeCollectionDelegate? after = null,
        DeserializeCollectionElementDelegate? elementReader = null,
        DeserializeCollectionElementDelegate? elementPointerReader = null
    ) {
        elementReader ??= UsePointerMap
            ? (d, n) => d.ReadPointerObject<T>(d.ReadInt32(), n)
            : (d, n) => d.ReadPointerObjectUnique<T>(d.ReadInt32(), n);

        int count = deserializer.ReadInt32();
        Capacity = count;
        for (int i = 0; i < count; i++) {
            before?.Invoke(deserializer, i, count);
            Add((T) elementReader(deserializer, i + 1 != count));
            after?.Invoke(deserializer, i, count);
        }
    }

    public override void Serialize(
        IGmDataSerializer serializer,
        SerializeCollectionDelegate? before = null,
        SerializeCollectionDelegate? after = null,
        SerializeCollectionElementDelegate? elementWriter = null
    ) {
        Serialize(serializer, before, after, elementWriter);
    }

    public override void Deserialize(
        IGmDataDeserializer deserializer,
        DeserializeCollectionDelegate? before = null,
        DeserializeCollectionDelegate? after = null,
        DeserializeCollectionElementDelegate? elementReader = null
    ) {
        Deserialize(deserializer, before, after, elementReader);
    }
}