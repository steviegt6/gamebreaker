// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.Collections.Generic;
using GameBreaker.Serialization.Abstractions;

namespace GameBreaker.IFF.Abstractions;

public interface IRootChunk
{
    IChunkedFile? File { get; set; }

    IDictionary<ChunkIdentity, IChunk> Chunks { get; }

    IChunk CreateChunk(ChunkIdentity identity);

    void Serialize(IDataSerializer serializer);

    void Deserialize(IDataDeserializer deserializer, uint endPosition);
}