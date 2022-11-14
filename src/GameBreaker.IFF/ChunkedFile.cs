// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System;
using GameBreaker.IFF.Abstractions;
using GameBreaker.Serialization.Abstractions;
using GameBreaker.Serialization.Utilities;

namespace GameBreaker.IFF;

public class ChunkedFile : IChunkedFile
{
    public IRootChunk Root { get; }

    public ChunkedFile(IRootChunk root) {
        root.File = this;
        Root = root;
    }

    public virtual void Serialize(IDataSerializer serializer) {
        serializer.Write(new ChunkIdentity("FORM").ToBytes());
        serializer.WriteWithLength((d) => { Root.Serialize(d); });
    }

    public virtual void Deserialize(IDataDeserializer deserializer) {
        if (new ChunkIdentity(deserializer.ReadBytes(4)).Value != "FORM") throw new Exception(); // TODO
        uint length = deserializer.ReadUInt32();
        Root.Deserialize(deserializer, length);
    }
}