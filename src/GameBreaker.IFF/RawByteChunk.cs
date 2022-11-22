// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System;
using GameBreaker.IFF.Abstractions;
using GameBreaker.Serialization.Abstractions;

namespace GameBreaker.IFF;

public class RawByteChunk : Chunk
{
    protected override ChunkIdentity? ExpectedIdentity => null;

    public byte[] Data { get; protected set; } = Array.Empty<byte>();

    protected override void SerializeChunk(IDataSerializer serializer, IChunkedFile file) {
        serializer.Write(Data);
    }

    protected override void DeserializeChunk(IDataDeserializer deserializer, IChunkedFile file, uint length) {
        Data = deserializer.ReadBytes((int) length); // TODO: cast ok?
    }
}