// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.Collections.Generic;
using GameBreaker.IFF.Abstractions;
using GameBreaker.IFF.Abstractions.Serialization;
using GameBreaker.Serialization.Abstractions;

namespace GameBreaker.IFF;

public abstract class SerializableDataChunk : Chunk
{
    protected abstract List<ISerializableData> Data { get; }

    protected override void SerializeChunk(IDataSerializer serializer, IChunkedFile file) {
        Data.ForEach(x => x.Serializer(serializer, file, this, x.Value));
    }

    protected override void DeserializeChunk(IDataDeserializer deserializer, IChunkedFile file, uint length) {
        Data.ForEach(x => x.Value = x.Deserializer(deserializer, file, this));
    }
}