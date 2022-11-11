// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.Collections.Generic;
using System.Linq;
using GameBreaker.IFF.Abstractions;
using GameBreaker.Serialization.Abstractions;

namespace GameBreaker.IFF;

public abstract class ChunkedFile : IChunkedFile
{
    public IRootChunk Root { get; }

    protected ChunkedFile(IRootChunk root) {
        Root = root;
    }

    public void Serialize(IDataSerializer serializer) {
        IDictionary<ChunkIdentity, IChunk> chunks = SortChunks(Root.Chunks);
        for (int i = 0; i < chunks.Count; i++) SerializeChunk(serializer, chunks.Values.ElementAt(i), i, i == chunks.Count - 1);
    }

    protected virtual IDictionary<ChunkIdentity, IChunk> SortChunks(IDictionary<ChunkIdentity, IChunk> chunks) {
        return chunks;
    }

    protected virtual void SerializeChunk(IDataSerializer serializer, IChunk chunk, int index, bool last) {
        chunk.Serialize(serializer, this);
    }

    public void Deserialize(IDataDeserializer deserializer, uint endPosition) {
        IEnumerable<(ChunkIdentity id, uint pos)> scannedIdentities = ScanChunks(deserializer, endPosition);
        
        foreach ((var id, uint pos) in scannedIdentities) {
            deserializer.Position = pos;
            var chunk = Root.CreateChunk(id);
            chunk.Deserialize(deserializer, this, pos);
            Root.Chunks[id] = chunk;
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