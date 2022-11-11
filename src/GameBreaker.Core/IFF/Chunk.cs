// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System;
using GameBreaker.Core.Abstractions.IFF;
using GameBreaker.Core.Abstractions.Serialization;
using GameBreaker.Core.Util.Extensions;

namespace GameBreaker.Core.IFF;

public abstract class Chunk : IChunk
{
    /// <summary>
    ///     The expected chunk identity.
    /// </summary>
    protected abstract ChunkIdentity ExpectedIdentity { get; }

    /// <inheritdoc cref="IChunk.Identity"/>
    public virtual ChunkIdentity Identity { get; protected set; }

    /// <inheritdoc cref="IChunk.Length"/>
    public virtual uint Length { get; protected set; }

    public void Serialize(IGmDataSerializer serializer) {
        // Write chunk name.
        serializer.Write(Identity.ToBytes());

        // Write chunk and then the new length.
        uint length = serializer.BeginLength();
        SerializeChunk(serializer);
        serializer.EndLength(length);
    }

    public void Deserialize(IGmDataDeserializer deserializer) {
        // Read name and length.
        Identity = ReadHeader(deserializer);
        Length = deserializer.ReadUInt32();

        // Read chunk.
        DeserializeChunk(deserializer);
    }

    protected virtual ChunkIdentity ReadHeader(IGmDataDeserializer deserializer) {
        var id = new ChunkIdentity(deserializer.ReadBytes(4));

        if (id.Value != ExpectedIdentity.Value) {
            // TODO: Custom exception type and message.
            throw new Exception();
        }

        return id;
    }

    protected abstract void SerializeChunk(IGmDataSerializer serializer);

    protected abstract void DeserializeChunk(IGmDataDeserializer deserializer);
}