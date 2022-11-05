// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.Collections.Generic;
using System.IO;
using GameBreaker.Abstractions.IFF;
using GameBreaker.Abstractions.Serialization;
using GameBreaker.IFF;
using GameBreaker.Serialization;

namespace GameBreaker.Tests.Utilities;

public static class SerializationUtilities
{
    private sealed class EmptyMetadata : IChunkedFileMetadata
    {
        public IEnumerable<string> ChunkNames { get; } = new List<string>();
        
        public IChunk DeserializeChunk(ChunkIdentity identity, uint length) {
            throw new System.NotImplementedException();
        }
    }

    public static (MemoryStream ms, IGmDataSerializer serializer, IGmDataDeserializer deserializer) PrepareSerializationTest() {
        MemoryStream ms = new();
        IGmDataSerializer serializer = new GmDataSerializer(new GmWriter(ms));
        IGmDataDeserializer deserializer = new GmDataDeserializer(new GmReader(ms));
        return (ms, serializer, deserializer);
    }
}