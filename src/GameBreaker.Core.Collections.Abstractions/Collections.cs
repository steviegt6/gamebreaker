// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.Collections.Generic;
using GameBreaker.Core.Abstractions.Serialization;

namespace GameBreaker.Core.Collections.Abstractions;

/// <summary>
///     A collection of <see cref="IGmSerializable"/>s with support for handling serialization.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IGmCollection<T> : ICollection<T>, IGmSerializable
    where T : IGmSerializable, new()
{
    void Serialize(
        IGmDataSerializer serializer,
        SerializeCollectionDelegate? before = null,
        SerializeCollectionDelegate? after = null,
        SerializeCollectionElementDelegate? elementWriter = null
    );

    void Deserialize(
        IGmDataDeserializer deserializer,
        DeserializeCollectionDelegate? before = null,
        DeserializeCollectionDelegate? after = null,
        DeserializeCollectionElementDelegate? elementReader = null
    );

    void IGmSerializable.Serialize(IGmDataSerializer serializer) => Serialize(serializer);

    void IGmSerializable.Deserialize(IGmDataDeserializer deserializer) => Deserialize(deserializer);
}

/// <summary>
///     A collection of pointers to <see cref="IGmSerializable"/> objects, with support for handling serialization.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IGmPointerCollection<T> : IGmCollection<T>
    where T : IGmSerializable, new()
{
    bool UsePointerMap { get; }

    void Serialize(
        IGmDataSerializer serializer,
        SerializeCollectionDelegate? before = null,
        SerializeCollectionDelegate? after = null,
        SerializeCollectionElementDelegate? elementWriter = null,
        SerializeCollectionElementDelegate? elementPointerWriter = null
    );

    void Deserialize(
        IGmDataDeserializer deserializer,
        DeserializeCollectionDelegate? before = null,
        DeserializeCollectionDelegate? after = null,
        DeserializeCollectionElementDelegate? elementReader = null,
        DeserializeCollectionElementDelegate? elementPointerReader = null
    );

    void IGmCollection<T>.Serialize(
        IGmDataSerializer serializer,
        SerializeCollectionDelegate? before,
        SerializeCollectionDelegate? after,
        SerializeCollectionElementDelegate? elementWriter
    ) {
        Serialize(serializer, before, after, elementWriter);
    }

    void IGmCollection<T>.Deserialize(
        IGmDataDeserializer deserializer,
        DeserializeCollectionDelegate? before,
        DeserializeCollectionDelegate? after,
        DeserializeCollectionElementDelegate? elementReader
    ) {
        Deserialize(deserializer, before, after, elementReader);
    }
}

/// <summary>
///     A collection of pointers to <see cref="IGmSerializable"/> objects, with support for handling serialization. <br />
///     Objects in this collection are not adjacent, and thus the position is reset at the end to the position after the final pointer. <br />
///     Writing does not serialize objects.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IGmRemotePointerCollection<T> : IGmCollection<T>
    where T : IGmSerializable, new()
{ }

/// <summary>
///     A collection of pointers to <see cref="IGmSerializable"/> objects, with support for handling serialization. <br />
///     <see cref="IGmPointerCollection{T}.UsePointerMap"/> is set to false.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IGmUniquePointerCollection<T> : IGmPointerCollection<T>
    where T : IGmSerializable, new()
{
    bool IGmPointerCollection<T>.UsePointerMap => false;
}