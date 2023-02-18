using System.Diagnostics;
using GameBreaker.Serial.IO;
using GameBreaker.Serial.IO.IFF;

namespace GameBreaker.Serial.GMS; 

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
