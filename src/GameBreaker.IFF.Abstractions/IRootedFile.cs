// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

namespace GameBreaker.IFF.Abstractions;

/// <summary>
///     Describes a chunked file with a "root" chunk that contains the actual chunks. Encapsulates a <see cref="IChunkedFile"/> as its root.
/// </summary>
public interface IRootedFile : IChunkedFile
{
    /// <summary>
    ///     The root chunk, containing its own subset of chunks.
    /// </summary>
    IChunkedFile Root { get; }
}