namespace GameBreaker.Serial.IO.IFF;

/// <summary>
///     Represents the data stored within a chunk.
/// </summary>
public interface IChunkData {
    void Serialize(IWriter writer, IffFile iffFile);
    
    void Deserialize(IReader reader, IffFile iffFile, ChunkPosInfo posInfo);
}
