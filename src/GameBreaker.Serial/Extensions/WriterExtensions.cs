using System;
using GameBreaker.Serial.IO;

namespace GameBreaker.Serial.Extensions;

public static class WriterExtensions {
    public static void WriteLength(this IWriter writer, Action action) {
        var start = writer.Position;
        writer.Write(DEAD_GAME);
        action();
        var end = writer.Position;
        var length = end - start;
        writer.Position = start;
        writer.Write(length);
        writer.Position = end;
    }
}
