// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using GameBreaker.Serialization.Abstractions;

namespace GameBreaker.IFF.Abstractions.Serialization;

public delegate void SerializeDataDelegate(IDataSerializer serializer, IChunk chunk, object? value);

public delegate object? DeserializeDataDelegate(IDataDeserializer deserializer, IChunk chunk);

public interface ISerializableData
{
    SerializeDataDelegate Serializer { get; }

    DeserializeDataDelegate Deserializer { get; }

    DeserializationState DeserializationState { get; }

    object? Value { get; }
}

public delegate void SerializeDelegate<in T>(IDataSerializer serializer, IChunk chunk, T? value);

public delegate T? DeserializeDelegate<out T>(IDataDeserializer deserializer, IChunk chunk);

public interface ISerializableData<T> : ISerializableData
{
    SerializeDataDelegate ISerializableData.Serializer => (s, c, v) => Serializer(s, c, (T?) v);

    DeserializeDataDelegate ISerializableData.Deserializer => (d, c) => Deserializer(d, c);

    object? ISerializableData.Value => Value;
    
    new SerializeDelegate<T> Serializer { get; }

    new DeserializeDelegate<T> Deserializer { get; }
    
    new T Value { get; }
}