namespace GameBreaker.Serial.IO.IFF;

/// <summary>
///     Represents the data stored within a chunk.
/// </summary>
public interface IChunkData {
    void Serialize(IWriter writer);
    
    void Deserialize(IReader reader, ChunkPosInfo posInfo);
}
