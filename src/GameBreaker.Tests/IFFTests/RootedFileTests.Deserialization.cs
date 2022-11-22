// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.IO;
using GameBreaker.IFF;
using GameBreaker.IFF.Abstractions;
using GameBreaker.Serialization;
using GameBreaker.Tests.Utilities;

namespace GameBreaker.Tests.IFFTests;

partial class RootedFileTests
{
    private class RawByteRootChunk : RootChunk
    {
        public override IChunk CreateChunk(ChunkIdentity identity) {
            return new RawByteChunk();
        }
    }
    
    [TestCase("data.win")]
    public static void Deserialize_RawData_GetExpectedChunksTest(string filePath) {
        using var deserializationStream = TestFiles.GetEmbeddedBytes(filePath);
        using var serializationStream = new MemoryStream();
        using var mem = new MemoryStream();

        deserializationStream.CopyTo(mem);
        deserializationStream.Seek(0, SeekOrigin.Begin);

        var file = new ChunkedFile(new RawByteRootChunk());
        var deserializer = new DataDeserializer(deserializationStream);
        var serializer = new DataSerializer(serializationStream);

        file.Deserialize(deserializer);
        file.Serialize(serializer);

        Assert.That(mem.ToArray(), Is.EqualTo(serializationStream.ToArray()));
    }
    
    [TestCase("data.win")]
    public static void Deserialize_GMS_GetExpectedChunksTest(string filePath) {
        using var deserializationStream = TestFiles.GetEmbeddedBytes(filePath);
        using var serializationStream = new MemoryStream();
        using var mem = new MemoryStream();

        deserializationStream.CopyTo(mem);
        deserializationStream.Seek(0, SeekOrigin.Begin);

        var file = new ChunkedFile(new RawByteRootChunk());
        var deserializer = new DataDeserializer(deserializationStream);
        var serializer = new DataSerializer(serializationStream);

        file.Deserialize(deserializer);
        file.Serialize(serializer);

        Assert.That(mem.ToArray(), Is.EqualTo(serializationStream.ToArray()));
    }
}