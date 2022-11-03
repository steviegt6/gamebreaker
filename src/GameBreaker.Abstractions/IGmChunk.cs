// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using GameBreaker.Abstractions.Serialization;

namespace GameBreaker.Abstractions;

/// <summary>
///     Represents a serializable chunk of data.
/// </summary>
public interface IGmChunk : IGmSerializable
{
    /// <summary>
    ///     The length of this chunk.
    /// </summary>
    long Length { get; set; }

    /// <summary>
    ///     The start position of this chunk.
    /// </summary>
    long StartPosition { get; set; }

    /// <summary>
    ///     The end position of this chunk.
    /// </summary>
    long EndPosition { get; set; }
}