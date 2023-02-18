using System.IO;

namespace GameBreaker.Serial.Tests; 

public static class Util {
    public static Stream? FromAssembly(string path) {
        path = path.Replace(Path.DirectorySeparatorChar, '.');
        path = typeof(Util).Assembly.GetName().Name! + "." + path;
        return typeof(Util).Assembly.GetManifestResourceStream(path);
    }

    public static byte[] ToBytes(this Stream stream) {
        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        return ms.ToArray();
    }
}
