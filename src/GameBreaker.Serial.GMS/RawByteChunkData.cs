using System.Diagnostics;
using GameBreaker.Serial.IO;
using GameBreaker.Serial.IO.IFF;

namespace GameBreaker.Serial.GMS; 

/// <summary>
///     Chunk data that specifically just stores the raw byte contents of a
///     chunk. Intended to be used for testing on in situations where a chunk is
///     not yet implemented.
/// </summary>
public class RawByteChunkData : ChunkData {
    private byte[]? data;

    public override void Serialize(IWriter writer) {
        Debug.Assert(data is not null);
        writer.Write(data);
    }

    public override void Deserialize(IReader reader, ChunkPosInfo posInfo) {
        data = reader.ReadBytes(posInfo.Length);
    }
}
