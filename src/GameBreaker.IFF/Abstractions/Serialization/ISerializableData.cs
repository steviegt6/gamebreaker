// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using GameBreaker.Serialization.Abstractions;

namespace GameBreaker.IFF.Abstractions.Serialization;

public delegate void SerializeDataDelegate(IDataSerializer serializer, IChunkedFile file, IChunk chunk);

public delegate void DeserializeDataDelegate(IDataDeserializer deserializer, IChunkedFile file, IChunk chunk);

public interface ISerializableData
{
    SerializeDataDelegate Serializer { get; }

    DeserializeDataDelegate Deserializer { get; }

    object? Value { get; set; }
}

public delegate void SerializeDelegate<in T>(IDataSerializer serializer, IChunkedFile file, IChunk chunk);

public delegate void DeserializeDelegate<out T>(IDataDeserializer deserializer, IChunkedFile file, IChunk chunk);

public interface ISerializableData<T> : ISerializableData
{
    SerializeDataDelegate ISerializableData.Serializer => (s, f, c) => Serialize(s, f, c);

    DeserializeDataDelegate ISerializableData.Deserializer => (d, f, c) => Deserialize(d, f, c);

    object? ISerializableData.Value {
        get => Value;
        set => Value = (T?) value;
    }
    
    new SerializeDelegate<T> Serialize { get; }

    new DeserializeDelegate<T> Deserialize { get; }
    
    new T? Value { get; set; }
}