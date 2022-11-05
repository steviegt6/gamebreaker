// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.Collections.Generic;
using GameBreaker.Abstractions.IFF;
using GameBreaker.Abstractions.Serialization;
using GameBreaker.Util.Extensions;

namespace GameBreaker.IFF;

public class ChunkedFile : IChunkedFile
{
    public virtual IChunkedFileMetadata Metadata { get; }
    
    public virtual IDictionary<string, IChunk> Chunks { get; protected set; } = new Dictionary<string, IChunk>();

    public ChunkedFile(IChunkedFileMetadata metadata) {
        Metadata = metadata;
    }

    public virtual void Serialize(IGmDataSerializer serializer) {
        // Serialize chunks in metadata-denoted order.
        foreach (string chunkName in Metadata.ChunkNames) {
            if (!Chunks.TryGetValue(chunkName, out IChunk? chunk)) continue;
            
            // Chunks should handle serialization of their names and lengths.
            chunk.Serialize(serializer);
        }
    }

    public virtual void Deserialize(IGmDataDeserializer deserializer) {
        while (true) {
            // Ensure enough room to read the header.
            if (deserializer.Position + 4 > deserializer.Length) break;
            
            byte[] header = deserializer.ReadBytes(4);
            
            // Ensure enough room to read the chunk length.
            if (deserializer.Position + 4 > deserializer.Length) {
                deserializer.Position -= 4;
                break;
            }

            uint length = deserializer.ReadUInt32();
            
            // Ensure enough room to read the chunk,
            if (deserializer.Position + length > deserializer.Length) {
                deserializer.Position -= 8;
                break;
            }

            deserializer.Position -= 8;
            length += 8;

            var id = new ChunkIdentity(header);
            var chunk = Metadata.DeserializeChunk(id, length);
            chunk.Deserialize(deserializer.CreateChildDeserializer(length));
            Chunks[id.Value] = chunk;
        }
    }
}