// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using CliFx.Attributes;
using CliFx.Infrastructure;
using GameBreaker.Common;
using GameBreaker.Core.Abstractions.IFF;
using GameBreaker.Utilities;

namespace GameBreaker.Examples.ChunkManipulation;

[Command("chunk-shuffler", Description = "Shuffles the chunks within the root FORM chunk.")]
public sealed class ChunkShufflerExample : BaseRunnerCommand
{
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