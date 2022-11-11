// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.Collections.Generic;
using GameBreaker.IFF.Abstractions;

namespace GameBreaker.IFF;

public abstract class RootChunk : IRootChunk
{
    public virtual IChunkedFile File { get; }

    public virtual IDictionary<ChunkIdentity, IChunk> Chunks { get; } = new Dictionary<ChunkIdentity, IChunk>();

    protected RootChunk(IChunkedFile file) {
        File = file;
    }

    public abstract IChunk CreateChunk(ChunkIdentity identity);
}