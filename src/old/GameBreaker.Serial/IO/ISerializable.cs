using GameBreaker.Serial.IO.IFF;

namespace GameBreaker.Serial.IO;

public interface ISerializable {
    object? Value { get; set; }

    void Serialize(IWriter writer, IChunkData chunk, IffFile iffFile);

    void Deserialize(IReader reader, IChunkData chunk, IffFile iffFile);
}

public interface ISerializable<T> : ISerializable {
    new T? Value { get; set; }

    object? ISerializable.Value {
        get => Value;
        set => Value = (T?) value;
    }
}
