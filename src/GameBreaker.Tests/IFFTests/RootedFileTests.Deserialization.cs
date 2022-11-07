// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.IO;
using GameBreaker.Common;
using GameBreaker.IFF;
using GameBreaker.Serialization;
using GameBreaker.Tests.Utilities;

namespace GameBreaker.Tests.IFFTests;

partial class RootedFileTests
{
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