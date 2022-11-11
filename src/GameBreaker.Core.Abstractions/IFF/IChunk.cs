// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using GameBreaker.Core.Abstractions.Serialization;

namespace GameBreaker.Core.Abstractions.IFF;

/// <summary>
///     Represents a chunk of data with a four-byte header identity and four-byte integer length.
/// </summary>
public interface IChunk
{
    /// <summary>
    ///     The chunk's four-byte identity.
    /// </summary>
    ChunkIdentity Identity { get; }
    
    /// <summary>
    ///     The chunk's four-byte length (determined during deserialization, updated during serialization).
    /// </summary>
    uint Length { get; }

    void Serialize(IGmDataSerializer serializer);

    void Deserialize(IGmDataDeserializer deserializer);
}