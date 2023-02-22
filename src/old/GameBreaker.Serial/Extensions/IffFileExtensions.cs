using System.Linq;
using GameBreaker.Serial.IO.IFF;

namespace GameBreaker.Serial.Extensions;

public static class IffFileExtensions {
    public static T? GetChunk<T>(this IffFile iffFile) where T : IChunkData {
        return (T?) iffFile.Root.Data.Chunks
                           .FirstOrDefault(x => x.Value.Data is T)
                           .Value.Data;
    }
}
