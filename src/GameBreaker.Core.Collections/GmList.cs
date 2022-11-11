// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.Collections.Generic;
using GameBreaker.Core.Abstractions.Serialization;
using GameBreaker.Core.Collections.Abstractions;

namespace GameBreaker.Core.Collections;

/// <inheritdoc cref="IGmList{T}"/>
public class GmList<T> : List<T>, IGmList<T>
    where T : IGmSerializable, new() // TODO
{
    public GmList() { }

    public GmList(int capacity) : base(capacity) { }

    public virtual void Serialize(
        IGmDataSerializer serializer,
        SerializeCollectionDelegate? before = null,
        SerializeCollectionDelegate? after = null,
        SerializeCollectionElementDelegate? elementWriter = null
    ) {
        elementWriter ??= (s, e) => e.Serialize(s);

        serializer.Write(Count);

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
        DeserializeCollectionElementDelegate? elementReader = null
    ) {
        elementReader ??= (d, _) =>
        {
            T e = new();
            e.Deserialize(d);
            return e;
        };
        int count = deserializer.ReadInt32();
        Capacity = count;

        for (int i = 0; i < Count; i++) {
            before?.Invoke(deserializer, i, count);
            Add((T) elementReader(deserializer, i + 1 != count));
            after?.Invoke(deserializer, i, count);
        }
    }
}