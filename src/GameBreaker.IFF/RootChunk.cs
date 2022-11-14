// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System;
using System.Collections.Generic;
using System.Linq;
using GameBreaker.IFF.Abstractions;
using GameBreaker.Serialization.Abstractions;

namespace GameBreaker.IFF;

public abstract class RootChunk : IRootChunk
{
    public IChunkedFile? File { get; set; }

    public virtual IDictionary<ChunkIdentity, IChunk> Chunks { get; } = new Dictionary<ChunkIdentity, IChunk>();

    public abstract IChunk CreateChunk(ChunkIdentity identity);

    public void Serialize(IDataSerializer serializer) {
        if (File is null) throw new Exception(); // TODO

        IDictionary<ChunkIdentity, IChunk> chunks = SortChunks(Chunks);
        for (int i = 0; i < chunks.Count; i++) SerializeChunk(serializer, chunks.Values.ElementAt(i), i, i == chunks.Count - 1);
    }

    protected virtual IDictionary<ChunkIdentity, IChunk> SortChunks(IDictionary<ChunkIdentity, IChunk> chunks) {
        return chunks;
    }

    protected virtual void SerializeChunk(IDataSerializer serializer, IChunk chunk, int index, bool last) {
        chunk.Serialize(serializer, File);
    }

    public void Deserialize(IDataDeserializer deserializer, uint endPosition) {
        if (File is null) throw new Exception(); // TODO

        IEnumerable<(ChunkIdentity id, uint pos)> scannedIdentities = ScanChunks(deserializer, endPosition);

        foreach ((var id, uint pos) in scannedIdentities) {
            deserializer.Position = pos;
            var chunk = CreateChunk(id);
            chunk.Deserialize(deserializer, File, pos);
            Chunks[id] = chunk;
        }
    }

    protected virtual IEnumerable<(ChunkIdentity id, uint pos)> ScanChunks(IDataDeserializer deserializer, uint endPosition) {
        List<(ChunkIdentity id, uint pos)> identities = new();
        while (deserializer.Position < endPosition) identities.Add(ScanChunk(deserializer, endPosition));
        return identities;
    }

    protected virtual (ChunkIdentity id, uint pos) ScanChunk(IDataDeserializer deserializer, uint endPosition) {
        uint pos = (uint) deserializer.Position; // TODO: uint case ok?
        var id = new ChunkIdentity(deserializer.ReadBytes(4));
        int length = deserializer.ReadInt32();
        deserializer.Position += length;
        return (id, pos);
    }
}