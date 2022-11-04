// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.Collections.Generic;
using GameBreaker.Abstractions.Serialization;

namespace GameBreaker.IFF.Abstractions;

/// <summary>
///     Describes a file with data separated into chunks containing four-byte headers as identities and four-byte integers representing their length.
/// </summary>
public interface IChunkedFile : IGmSerializable
{
    /// <summary>
    ///     Describes metadata for (de)serialization.
    /// </summary>
    IChunkedFileMetadata Metadata { get; }

    /// <summary>
    ///     The chunks stored within this chunked file.
    /// </summary>
    IDictionary<string, IChunk> Chunks { get; }
}