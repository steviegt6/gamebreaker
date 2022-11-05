// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using CliFx.Attributes;
using CliFx.Infrastructure;
using GameBreaker.Abstractions.IFF;
using GameBreaker.Abstractions.Serialization;
using GameBreaker.IFF;
using GameBreaker.Serialization;
using GameBreaker.Utilities;

namespace GameBreaker.Examples.ChunkManipulation;

[Command("chunk-shuffler", Description = "Shuffles the chunks within the root FORM chunk.")]
public sealed class ChunkShufflerExample : BaseRunnerCommand
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

    [CommandOption("preserve-gen8", /*'g',*/ Description = "Whether to preserve GEN8's position and not shuffle it.")]
    public bool PreserveGEN8 { get; set; } = false;

    public override async ValueTask ExecuteAsync(IConsole console) {
        var iff = await GetGameMakerFile(new SimpleMetadata());
        if (iff.Chunks is null) throw new Exception("IFF deserialization failed!");

        Dictionary<string, IChunk> chunks = iff.Chunks.Shuffle().ToDictionary();

        if (PreserveGEN8) {
            List<KeyValuePair<string, IChunk>> list = chunks.ToList();
            int index = list.FindIndex(x => x.Value.Identity == "GEN8");
            if (index != -1) {
                KeyValuePair<string, IChunk> gen8 = list[index];
                list.RemoveAt(index);
                list.Insert(0, gen8);
                chunks = list.ToDictionary();
            }
        }

        iff.Chunks.SetTo(chunks);
        await WriteGameMakerFile(console, iff);
    }
}