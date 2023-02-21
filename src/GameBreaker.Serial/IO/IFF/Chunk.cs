using GameBreaker.Serial.Extensions;

namespace GameBreaker.Serial.IO.IFF;

public class Chunk {
    public IChunkData Data { get; set; }
    
    public IffFile IffFile { get; set; }

    public Chunk(IChunkData data, IffFile iffFile) {
        Data = data;
        IffFile = iffFile;
    }

    public virtual void Serialize(IWriter writer) {
        writer.WriteLength(() => Data.Serialize(writer));
    }

    public virtual void Deserialize(IReader reader) {
        var len = reader.ReadInt32();
        var pos = reader.Position;
        Data.Deserialize(reader, new ChunkPosInfo(len, pos, pos + len));
    }
}

public class Chunk<T> : Chunk where T : IChunkData {
    public new T Data {
        get => (T) base.Data;
        set => base.Data = value;
    }

    public Chunk(T data, IffFile iffFile) : base(data, iffFile) { }
}
