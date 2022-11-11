// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.Collections.Generic;

namespace GameBreaker.IFF.Abstractions;

public interface IRootChunk
{
    IDictionary<ChunkIdentity, IChunk> Chunks { get; }

    IChunk CreateChunk(ChunkIdentity identity);
}