// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using GameBreaker.Core.Abstractions.Serialization;
using GameBreaker.Core.Collections.Abstractions;

namespace GameBreaker.Core.Collections;

public class GmRemotePointerList<T> : GmList<T>, IGmRemotePointerList<T>
    where T : IGmSerializable, new() // TODO
{
    public virtual void Serialize(
        IGmDataSerializer serializer,
        SerializeCollectionDelegate? before = null,
        SerializeCollectionDelegate? after = null,
        SerializeCollectionElementDelegate? elementWriter = null,
        SerializeCollectionElementDelegate? elementPointerWriter = null
    ) {
    }

    public virtual void Deserialize(
        IGmDataDeserializer deserializer,
        DeserializeCollectionDelegate? before = null,
        DeserializeCollectionDelegate? after = null,
        DeserializeCollectionElementDelegate? elementReader = null,
        DeserializeCollectionElementDelegate? elementPointerReader = null
    ) {
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