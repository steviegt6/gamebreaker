namespace GameBreaker.Serial.IO;

public interface IPointerSerializable {
    int Pointer { get; set; }

    int PointerOffset { get; }

    object? Value { get; set; }

    void Serialize(IWriter writer);

    void Deserialize(IReader reader);
}

public interface IPointerSerializable<T> : IPointerSerializable {
    new T? Value { get; set; }

    object? IPointerSerializable.Value {
        get => Value;
        set => Value = (T?) value;
    }
}
