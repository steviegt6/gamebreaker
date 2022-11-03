// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using GameBreaker.Abstractions;
using GameBreaker.Abstractions.Serialization;

namespace GameBreaker.Chunks;

public abstract partial class GmChunk : IGmChunk
{
    public long Length { get; set; }

    public long StartPosition { get; set; }

    public long EndPosition { get; set; }

    public abstract void Serialize(IGmDataSerializer serializer);

    public virtual void Deserialize(IGmDataDeserializer deserializer) {
        Length = deserializer.Reader.ReadInt32();
        StartPosition = deserializer.Reader.Position;
        EndPosition = StartPosition + Length;
    }
}