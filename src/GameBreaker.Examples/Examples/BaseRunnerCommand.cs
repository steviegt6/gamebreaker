// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using GameBreaker.Abstractions.IFF;
using GameBreaker.Abstractions.Serialization;
using GameBreaker.IFF;
using GameBreaker.Serialization;

namespace GameBreaker.Examples;

[Command]
public abstract class BaseRunnerCommand : ICommand
{
    [CommandOption("path", 'p', Description = "Path to the GameMaker IIF file.")]
    public string? Path { get; set; } = null;

    public string? OutputPath { get; set; } = null;

    public abstract ValueTask ExecuteAsync(IConsole console);

    protected virtual async Task<IRootedFile> GetGameMakerFile(IChunkedFileMetadata metadata) {
        if (Path is null) throw new FileNotFoundException("Path to the GameMaker IIF file was not specified!");
        if (!File.Exists(Path)) throw new FileNotFoundException($"File does not exist at path: {Path}");

        await using var stream = File.OpenRead(Path);
        var file = new GameMakerFile(metadata);
        var deserializer = new GmDataDeserializer(new GmReader(stream));
        file.Deserialize(deserializer);
        return await Task.FromResult(file);
    }

    protected virtual async Task WriteGameMakerFile(IConsole console, IRootedFile file) {
        if (OutputPath is null) {
            await console.Output.WriteLineAsync("Option 'output-path' was not specified, defaulting to './data.win' (in cwd)!");
            OutputPath = "data.win";
        }

        await console.Output.WriteLineAsync($"Writing output GameMaker IFF file to: {OutputPath}");
        await using var stream = File.OpenWrite(OutputPath);
        var serializer = new GmDataSerializer(new GmWriter(stream));
        file.Serialize(serializer); // TODO: async?
    }
}