// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System;
using GameBreaker.Abstractions.IFF;
using GameBreaker.Abstractions.Serialization;
using GameBreaker.Exceptions;
using GameBreaker.Util.Extensions;

namespace GameBreaker.IFF;

public class RootedFile : ChunkedFile, IRootedFile
{
    public virtual IChunkedFile? Root { get; protected set; }

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
        base.Deserialize(deserializer);

        // TODO: ReadBytes instead.
        if (deserializer.ReadChars(4) != FORM_C)
            throw new FormGameMakerDeserializationException("Expected 'FORM' chunk header! - is this a GameMaker IFF file?");
    }
}