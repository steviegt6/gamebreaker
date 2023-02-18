namespace GameBreaker.Serial.IO;

public interface ISerializable {
    object? Value { get; set; }

    void Serialize(IWriter writer);

    void Deserialize(IReader reader);
}

public interface ISerializable<T> : ISerializable {
    new T? Value { get; set; }

    object? ISerializable.Value {
        get => Value;
        set => Value = (T?) value;
    }
}
