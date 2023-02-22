namespace GameBreaker.Serial.IO.IFF;

public abstract class ChunkData : IChunkData {
    public abstract void Serialize(IWriter writer, IffFile iffFile);

    public abstract void Deserialize(
        IReader reader,
        IffFile iffFile,
        ChunkPosInfo posInfo
    );
}
