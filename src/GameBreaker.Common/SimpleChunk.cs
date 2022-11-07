// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using GameBreaker.Abstractions.IFF;
using GameBreaker.Abstractions.Serialization;
using GameBreaker.IFF;

namespace GameBreaker.Common;

public class SimpleChunk : Chunk
{
    protected override ChunkIdentity ExpectedIdentity => throw new System.NotImplementedException();

    private byte[]? Data { get; set; }

    protected override ChunkIdentity ReadHeader(IGmDataDeserializer deserializer) {
        return new ChunkIdentity(deserializer.ReadBytes(4));
    }

    protected override void SerializeChunk(IGmDataSerializer serializer) {
        if (Data is null) throw new System.InvalidOperationException("Attempted to serialize non-deserialized chunk.");

        serializer.Write(Data);
    }

    protected override void DeserializeChunk(IGmDataDeserializer deserializer) {
        Data = deserializer.ReadBytes((int) Length);
    }
}