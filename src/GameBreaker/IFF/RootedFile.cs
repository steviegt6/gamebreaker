// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System;
using System.Collections.Generic;
using GameBreaker.Abstractions.IFF;
using GameBreaker.Abstractions.Serialization;
using GameBreaker.Exceptions;
using GameBreaker.Util.Extensions;

namespace GameBreaker.IFF;

public class RootedFile : ChunkedFile, IRootedFile
{
    public virtual IChunkedFile? Root { get; protected set; }

    public override IDictionary<string, IChunk>? Chunks => Root?.Chunks;

    public RootedFile(IChunkedFileMetadata metadata) : base(metadata) { }

    public override void Serialize(IGmDataSerializer serializer) {
        // base.Serialize(serializer);
        if (Root is null) throw new Exception(); // TODO: Error handling.

        serializer.Write(FORM_C);

        uint length = serializer.BeginLength();
        Root.Serialize(serializer);
        serializer.EndLength(length);
    }

    public override void Deserialize(IGmDataDeserializer deserializer) {
        var id = new ChunkIdentity(deserializer.ReadBytes(4));
        // TODO: Constant value for FORM later.
        if (id.Value != "FORM")
            throw new FormGameMakerDeserializationException(
                $"Expected \"FORM\" chunk header but got \"{id.Value}\"! - is this a GameMaker IFF file?"
            );

        uint length = deserializer.ReadUInt32();
        Root = new ChunkedFile(Metadata);
        Root.Deserialize(deserializer.CreateChildDeserializer(length));
    }
}