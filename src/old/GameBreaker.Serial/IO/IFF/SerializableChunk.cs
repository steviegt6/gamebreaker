using System.Diagnostics;

namespace GameBreaker.Serial.IO.IFF;

public class SerializableChunk : Serializable<Chunk> {
    public SerializableChunk(Chunk value) : base(value) { }

    public override void Serialize(
        IWriter writer,
        IChunkData chunk,
        IffFile iffFile
    ) {
        Debug.Assert(Value is not null);
        Value.Serialize(writer);
    }

    public override void Deserialize(
        IReader reader,
        IChunkData chunk,
        IffFile iffFile
    ) {
        Debug.Assert(Value is not null);
        Value.Deserialize(reader);
    }
}

public class SerializableChunk<T> : Serializable<Chunk<T>>
    where T : IChunkData {
    public SerializableChunk(Chunk<T> value) : base(value) { }

    public override void Serialize(
        IWriter writer,
        IChunkData chunk,
        IffFile iffFile
    ) {
        Debug.Assert(Value is not null);
        Value.Serialize(writer);
    }

    public override void Deserialize(
        IReader reader,
        IChunkData chunk,
        IffFile iffFile
    ) {
        Debug.Assert(Value is not null);
        Value.Deserialize(reader);
    }
}
