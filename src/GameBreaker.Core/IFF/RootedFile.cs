// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System;
using System.Collections.Generic;
using GameBreaker.Core.Abstractions.IFF;
using GameBreaker.Core.Abstractions.Serialization;
using GameBreaker.Core.Exceptions;
using GameBreaker.Core.Util.Extensions;

namespace GameBreaker.Core.IFF;

public class RootedFile : ChunkedFile, IRootedFile
{
    public virtual IChunkedFile? Root { get; protected set; }

    public override IDictionary<string, IChunk>? Chunks => Root?.Chunks;

    public RootedFile(IChunkedFileMetadata metadata) : base(metadata) { }

    public override void Serialize(IGmDataSerializer serializer) {
        serializer.GameMakerFile = this;
        
        // base.Serialize(serializer);
        if (Root is null) throw new Exception(); // TODO: Error handling.

        serializer.Write(new ChunkIdentity("FORM").ToBytes());

        uint length = serializer.BeginLength();
        Root.Serialize(serializer);
        serializer.EndLength(length);
    }

    public override void Deserialize(IGmDataDeserializer deserializer) {
        deserializer.GameMakerFile = this;
        
        var id = new ChunkIdentity(deserializer.ReadBytes(4));
        // TODO: Constant value for FORM later.
        if (id.Value != "FORM")
            throw new FormGameMakerDeserializationException(
                $"Expected \"FORM\" chunk header but got \"{id.Value}\"! - is this a GameMaker IFF file?"
            );

        uint length = deserializer.ReadUInt32();
        Root = CreateRoot(Metadata);
        Root.Deserialize(deserializer);
    }

    protected virtual IChunkedFile CreateRoot(IChunkedFileMetadata metadata) {
        return new ChunkedFile(metadata);
    }
}