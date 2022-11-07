// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.IO;
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

            serializer.Write(Data);
        }

        protected override void DeserializeChunk(IGmDataDeserializer deserializer) {
            Data = deserializer.ReadBytes((int) Length);
        }
    }

    private sealed class SimpleMetadata : IChunkedFileMetadata
    {
        public IChunk DeserializeChunk(ChunkIdentity identity, uint length) {
            return new SimpleChunk();
        }
    }

    [TestCase("data.win")]
    public static void Deserialize_GetExpectedChunksTest(string filePath) {
        using var deserializationStream = TestFiles.GetEmbeddedBytes(filePath);
        using var serializationStream = new MemoryStream();
        using var mem = new MemoryStream();

        deserializationStream.CopyTo(mem);
        deserializationStream.Seek(0, SeekOrigin.Begin);

        var file = new GameMakerFile(new SimpleMetadata());
        var deserializer = new GmDataDeserializer(new GmReader(deserializationStream));
        var serializer = new GmDataSerializer(new GmWriter(serializationStream));

        file.Deserialize(deserializer);
        file.Serialize(serializer);

        Assert.That(mem.ToArray(), Is.EqualTo(serializationStream.ToArray()));
    }
}