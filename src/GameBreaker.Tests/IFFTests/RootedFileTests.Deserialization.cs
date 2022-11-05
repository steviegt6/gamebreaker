// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.Collections.Generic;
using GameBreaker.Abstractions.IFF;
using GameBreaker.Abstractions.Serialization;
using GameBreaker.IFF;
using GameBreaker.Serialization;
using GameBreaker.Tests.Utilities;

namespace GameBreaker.Tests.IFFTests;

partial class RootedFileTests
{
    private sealed class SimpleChunk : Chunk
    {
        protected override ChunkIdentity ExpectedIdentity => throw new System.NotImplementedException();

        private byte[]? Data { get; set; }

        protected override ChunkIdentity ReadHeader(IGmDataDeserializer deserializer) {
            return new ChunkIdentity(deserializer.ReadBytes(4));
        }

        protected override void SerializeChunk(IGmDataSerializer serializer) {
            if (Data is null) throw new System.InvalidOperationException("Attempted to serialize non-deserialized chunk.");
            if (Data.Length != Length) throw new System.InvalidOperationException("Attempted to serialize chunk with invalid length.");

            serializer.Write(Data);
        }

        protected override void DeserializeChunk(IGmDataDeserializer deserializer) {
            Data = deserializer.ReadBytes((int) Length);
        }
    }

    private sealed class SimpleMetadata : IChunkedFileMetadata
    {
        public IEnumerable<string> ChunkNames => throw new System.NotImplementedException();

        public IChunk DeserializeChunk(ChunkIdentity identity, uint length) {
            return new SimpleChunk();
        }
    }

    [TestCase(new string[] { }, "data.win")]
    public static void Deserialize_GetExpectedChunksTest(string[] expectedChunks, string filePath) {
        var file = new GameMakerFile(new SimpleMetadata());
        var deserializer = new GmDataDeserializer(new GmReader(TestFiles.GetEmbeddedBytes(filePath)));
        file.Deserialize(deserializer);
    }
}