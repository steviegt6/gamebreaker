// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using GameBreaker.Serialization.Abstractions;

namespace GameBreaker.IFF.Abstractions;

/// <summary>
///     Describes an abstracted implementation of GameMaker's variant of the IFF file format,
///     which contains a <see cref="IRootChunk"/> (<see cref="Root"/>) encapsulating its own collection of <see cref="IChunk"/>s. <br />
///     Chunks have an eight-byte header, the first four denoting its name and the last four, its length
/// </summary>
public interface IChunkedFile
{
    IRootChunk Root { get; }

    void Serialize(IDataSerializer serializer);

    void Deserialize(IDataDeserializer deserializer);
}