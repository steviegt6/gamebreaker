// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.Collections.Generic;

namespace GameBreaker.Core.Abstractions.IFF;

/// <summary>
///     Describes data revolving around the expected chunks of a <see cref="IChunkedFile"/>.
/// </summary>
public interface IChunkedFileMetadata
{
    /*// TODO: Does order truly matter here? One advantage of IFF is that chunk orders are not important. Needs testing...
    /// <summary>
    ///     A collection of chunk names that describe the expected order of chunks in the IFF file.
    /// </summary>
    IEnumerable<string> ChunkNames { get; }*/

    IChunk CreateChunk(ChunkIdentity identity, uint length);

    IDictionary<string, IChunk> OrderChunks(IDictionary<string, IChunk> chunks);
}