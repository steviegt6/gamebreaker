﻿using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using GameBreaker.Serial.IO;
using GameBreaker.Serial.IO.IFF;

namespace GameBreaker.Serial.Tests.IO.IFF;

internal class RawByteChunkData : ChunkData {
    private byte[]? data;

    public override void Serialize(IWriter writer) {
        Debug.Assert(data is not null);
        writer.Write(data);
    }

    public override void Deserialize(IReader reader, ChunkPosInfo posInfo) {
        data = reader.ReadBytes(posInfo.Length);
    }
}

internal class TestFormChunkData : FormChunkData {
    protected override List<int> ResolveChunks(
        IReader reader,
        ChunkPosInfo posInfo
    ) {
        ChunkNames.Clear();
        var offsets = new List<int>();

        while (reader.Position < posInfo.End) {
            offsets.Add(reader.Position);
            var name = new string(reader.ReadChars(4));
            ChunkNames.Add(name);
            RegisterFactory(name, () => new RawByteChunkData());
            var len = reader.ReadInt32();
            reader.Position += len;
        }

        return offsets;
    }
}

[TestFixture]
public static class SerializationTests {
    [Test]
    public static void TestSerialization() {
        var stream = Util.FromAssembly(Path.Combine("assets", "data.win"));
        if (stream is null)
            throw new FileNotFoundException("Could not find data.win");

        var reader = new BufferedReader(stream.ToBytes());
        var iff = new IffFile(new TestFormChunkData());
        iff.Deserialize(reader);
    }

    [Test]
    public static void TestEquality() {
        var stream = Util.FromAssembly(Path.Combine("assets", "data.win"));
        if (stream is null)
            throw new FileNotFoundException("Could not find data.win");

        var bytes = stream.ToBytes();
        var serializedBytes = new byte[bytes.Length];

        var reader = new BufferedReader(bytes);
        var writer = new BufferedWriter(serializedBytes);
        var iff = new IffFile(new TestFormChunkData());
        iff.Deserialize(reader);
        iff.Serialize(writer);

        CollectionAssert.AreEqual(bytes, serializedBytes);
    }
}
