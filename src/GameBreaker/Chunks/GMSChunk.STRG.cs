// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using GameBreaker.Core.Abstractions;
using GameBreaker.Core.Abstractions.IFF;
using GameBreaker.Core.Abstractions.Serialization;
using GameBreaker.Core.Collections;
using GameBreaker.Core.Collections.Abstractions;

namespace GameBreaker.Chunks;

partial class GMSChunk
{
    public class STRG : GMSChunk
    {
        public IGmPointerList<GmString> Strings { get; protected set; }

        protected override ChunkIdentity ExpectedIdentity => new("STRG");

        protected override void SerializeChunk(IGmDataSerializer serializer) {
            throw new System.NotImplementedException();
        }

        protected override void DeserializeChunk(IGmDataDeserializer deserializer) {
            Strings = ReadPointerList<GmString>(
                deserializer,
                (reader, notLast) =>
                {
                    int ptr = reader.ReadInt32();
                    return reader.ReadPointerObject<GmString>(ptr, notLast);
                }
            );
        }
    }
}