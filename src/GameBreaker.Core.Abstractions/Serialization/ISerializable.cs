// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using GameBreaker.Core.Abstractions.IFF;

namespace GameBreaker.Core.Abstractions.Serialization;

public delegate void SerializeDelegate(IGmDataSerializer serializer, IChunk chunk, object? value);

public delegate object? DeserializeDelegate(IGmDataDeserializer deserializer, IChunk chunk);

public enum DeserializationState
{
    NotDeserialized,
    Deserialized,
    Skipped
}

public interface ISerializable
{
    SerializeDelegate Serializer { get; }

    DeserializeDelegate Deserializer { get; }

    DeserializationState DeserializationState { get; }

    object? Value { get; }
}

public delegate void SerializeDelegate<in T>(IGmDataSerializer serializer, IChunk chunk, T? value);

public delegate T? DeserializeDelegate<out T>(IGmDataDeserializer deserializer, IChunk chunk);

public interface ISerializable<T> : ISerializable
{
    SerializeDelegate ISerializable.Serializer => (s, c, v) => Serializer(s, c, (T?) v);

    DeserializeDelegate ISerializable.Deserializer => (d, c) => Deserializer(d, c);

    object? ISerializable.Value => Value;
    
    new SerializeDelegate<T> Serializer { get; }

    new DeserializeDelegate<T> Deserializer { get; }
    
    new T Value { get; }
}