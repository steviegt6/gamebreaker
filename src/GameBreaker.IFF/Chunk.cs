// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System;
using GameBreaker.IFF.Abstractions;
using GameBreaker.Serialization.Abstractions;
using GameBreaker.Serialization.Utilities;

namespace GameBreaker.IFF;

public abstract class Chunk : IChunk
{
    protected abstract ChunkIdentity? ExpectedIdentity { get; }
    
    public virtual ChunkIdentity Identity { get; protected set; }

    public void Serialize(IDataSerializer serialize, IChunkedFile file) {
        serialize.Write(Identity.ToBytes()); // Write chunk name.
        serialize.WriteWithLength(s => SerializeChunk(s, file));
    }

    protected abstract void SerializeChunk(IDataSerializer serializer, IChunkedFile file);

    public void Deserialize(IDataDeserializer deserializer, IChunkedFile file, uint endPosition) {
        Identity = ReadIdentity(deserializer, file, endPosition);
        uint length = ReadLength(deserializer, file, endPosition);

        DeserializeChunk(deserializer, file, length);
    }

    protected virtual ChunkIdentity ReadIdentity(IDataDeserializer deserializer, IChunkedFile file, uint endPosition) {
        if (deserializer.Position + 4 > endPosition) throw new Exception(); // TODO
        var id = new ChunkIdentity(deserializer.ReadBytes(4));
        if (ExpectedIdentity is not null && id.Value != ExpectedIdentity.Value) throw new Exception(); // TODO
        return id;
    }

    protected virtual uint ReadLength(IDataDeserializer deserializer, IChunkedFile file, uint endPosition) {
        if (deserializer.Position + 4 > endPosition) throw new Exception(); // TODO
        return deserializer.ReadUInt32();
    }

    protected abstract void DeserializeChunk(IDataDeserializer deserializer, IChunkedFile file, uint length);
}