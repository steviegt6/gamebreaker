// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System;
using System.Collections.Generic;
using System.Linq;
using GameBreaker.Common;
using GameBreaker.Core.Abstractions.IFF;

namespace GameBreaker;

public class GMSMetadata : IChunkedFileMetadata
{
    public virtual IChunk CreateChunk(ChunkIdentity identity, uint length) {
        return CHUNKS.TryGetValue(identity.Value, out Func<IChunk>? factory) ? factory() : new SimpleChunk();
    }

    public virtual IDictionary<string, IChunk> OrderChunks(IDictionary<string, IChunk> chunks) {
        List<string> chunkKeys = CHUNKS.Keys.ToList();

        int IndexOf(KeyValuePair<string, IChunk> x) {
            int index = chunkKeys.IndexOf(x.Key);
            if (index == -1) index = int.MaxValue;
            return index;
        }

        return chunks.OrderBy(IndexOf).ToDictionary(x => x.Key, x => x.Value);
    }
}