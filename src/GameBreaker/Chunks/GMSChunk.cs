// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using GameBreaker.Core.Abstractions.Serialization;
using GameBreaker.Core.Collections;
using GameBreaker.Core.Collections.Abstractions;
using GameBreaker.Core.IFF;

namespace GameBreaker.Chunks;

public abstract partial class GMSChunk : Chunk
{
    /*protected virtual GmPointerList<T> ReadPointerList<T>(IGmDataDeserializer deserializer, DeserializeCollectionElementDelegate elementReader)
        where T : IGmSerializable, new() {
        GmPointerList<T> list = new();
        list.Deserialize(deserializer, null, null, elementReader);
        return list;
    }*/
}