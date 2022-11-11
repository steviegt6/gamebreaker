// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using GameBreaker.Serialization.Abstractions;

namespace GameBreaker.IFF.Abstractions.Serialization;

public delegate void SerializeDataDelegate(IDataSerializer serializer, IChunkedFile file, IChunk chunk, object? value);

public delegate object? DeserializeDataDelegate(IDataDeserializer deserializer, IChunkedFile file, IChunk chunk);

public interface ISerializableData
{
    SerializeDataDelegate Serializer { get; }

    DeserializeDataDelegate Deserializer { get; }

    DeserializationState DeserializationState { get; }

    object? Value { get; set; }
}

public delegate void SerializeDelegate<in T>(IDataSerializer serializer, IChunkedFile file, IChunk chunk, T? value);

public delegate T? DeserializeDelegate<out T>(IDataDeserializer deserializer, IChunkedFile file, IChunk chunk);

public interface ISerializableData<T> : ISerializableData
{
    SerializeDataDelegate ISerializableData.Serializer => (s, f, c, v) => Serializer(s, f, c, (T?) v);

    DeserializeDataDelegate ISerializableData.Deserializer => (d, f, c) => Deserializer(d, f, c);

    object? ISerializableData.Value {
        get => Value;
        set => Value = (T?) value;
    }
    
    new SerializeDelegate<T> Serializer { get; }

    new DeserializeDelegate<T> Deserializer { get; }
    
    new T? Value { get; set; }
}