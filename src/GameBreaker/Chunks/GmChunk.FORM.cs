// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.
// Copyright (c) colinator27. Licensed under the MIT License.
// See the LICENSE-DogScepter file in the repository root for full terms and conditions.

using System.Collections.Generic;
using System.Diagnostics;
using GameBreaker.Abstractions;
using GameBreaker.Abstractions.Serialization;
using GameBreaker.Exceptions;
using GameBreaker.Util.Extensions;

namespace GameBreaker.Chunks;

partial class GmChunk
{
    public class FORM : GmChunk, IRootGmChunk
    {
        protected List<string> ChunkNames = new();

        public Dictionary<string, IGmChunk> Chunks { get; }

        public override void Serialize(IGmDataSerializer serializer) {
            serializer.Writer.Write(FORM_C);

            // Save four bytes so we can write the FORM chunk's length later.
            long formStart = serializer.Writer.BeginLength();

            for (int i = 0; i < ChunkNames.Count; i++) {
                if (!Chunks.TryGetValue(ChunkNames[i], out IGmChunk? chunk)) continue;
                
                Debug.WriteLine($"Writing chunk '{ChunkNames[i]}' at position '{serializer.Writer.Position:X}'.");
                
                // Write chunk name.
                serializer.Writer.Write(ChunkNames[i].ToCharArray());
                
                // Save four bytes so we can write the chunk's length later.
                long chunkStart = serializer.Writer.BeginLength();
                
                // Serialize the chunk.
                chunk.Serialize(serializer);
                
                // Write the chunk's length.
                serializer.Writer.EndLength(chunkStart);
            }
            
            // Write the FORM chunk's length.
            serializer.Writer.EndLength(formStart);
        }

        public override void Deserialize(IGmDataDeserializer deserializer) {
            if (deserializer.Reader.ReadChars(4) != FORM_C)
                throw new FormGmDeserializationException("'FORM' chunk/file header not found; this is not a valid GameMaker data file!");

            base.Deserialize(deserializer);
        }
    }
}