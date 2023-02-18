namespace GameBreaker.Serial.IO.IFF;

public abstract class ChunkData : IChunkData {
    public abstract void Serialize(IWriter writer);

    public abstract void Deserialize(IReader reader, ChunkPosInfo posInfo);
}
