// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using GameBreaker.Abstractions;
using GameBreaker.Abstractions.Exceptions;
using GameBreaker.Abstractions.IFF;
using GameBreaker.Abstractions.IFF.GM;

namespace GameBreaker.IFF;

public class GameMakerFile : RootedFile, IGameMakerFile
{
    public virtual GmVersionInfo VersionInfo { get; } = new();

    public virtual ActionBlock<KeyValuePair<string, byte[]>> FileWrites { get; }

    public virtual string Directory { get; }

    public virtual string Filename { get; }

    public virtual byte[] Hash { get; }

    public GameMakerFile(IChunkedFileMetadata metadata) : base(metadata) { }

    public virtual GmString DefineString(string value, out int index) {
        if (value is null) throw new UninitializedGmStringException("Attempted to define GmString with a null value!");
        throw new System.NotImplementedException();
    }
}