// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using GameBreaker.Abstractions.IFF;

namespace GameBreaker.Common;

public class SimpleMetadata : IChunkedFileMetadata
{
    public IChunk DeserializeChunk(ChunkIdentity identity, uint length) {
        return new SimpleChunk();
    }
}