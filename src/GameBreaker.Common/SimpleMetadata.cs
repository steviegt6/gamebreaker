// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.Collections.Generic;
using GameBreaker.Core.Abstractions.IFF;

namespace GameBreaker.Common;

public class SimpleMetadata : IChunkedFileMetadata
{
    public IChunk CreateChunk(ChunkIdentity identity, uint length) {
        return new SimpleChunk();
    }

    public IDictionary<string, IChunk> OrderChunks(IDictionary<string, IChunk> chunks) {
        return chunks;
    }
}