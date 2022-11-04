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
        uint length = serializer.Writer.BeginLength();

        // Serialize chunks in metadata-denoted order.
        foreach (string chunkName in Metadata.ChunkNames) {
            if (!Chunks.TryGetValue(chunkName, out IChunk? chunk)) continue;
            
            // Chunks should handle serialization of their names and lengths.
            chunk.Serialize(serializer);
        }
        
        serializer.Writer.EndLength(length);
    }

    public virtual void Deserialize(IGmDataDeserializer deserializer) {
        // TODO: Deserialization.
    }
}