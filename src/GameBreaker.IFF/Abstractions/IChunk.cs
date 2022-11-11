// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using GameBreaker.Serialization.Abstractions;

namespace GameBreaker.IFF.Abstractions;

/// <summary>
///     Represents a chunk of data with a four-byte header identity and four-byte integer length.
/// </summary>
public interface IChunk
{
    /// <summary>
    ///     The chunk's four-byte identity.
    /// </summary>
    ChunkIdentity Identity { get; }

    void Serialize(IDataSerializer serialize, IChunkedFile file);

    void Deserialize(IDataDeserializer deserializer, IChunkedFile file, uint endPosition);
}