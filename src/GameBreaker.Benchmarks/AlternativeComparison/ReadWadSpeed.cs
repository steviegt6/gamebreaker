using System;
using System.IO;
using BenchmarkDotNet.Attributes;
using GameBreaker.Serial;

namespace GameBreaker.Benchmarks.AlternativeComparison;

public class ReadWadSpeed {
    [Benchmark]
    public void GameBreakerReadWads() {
        foreach (var wad in GetWadFiles()) {
            using var stream = File.OpenRead(wad);
            using var binaryReader = BufferBinaryReader.FromStream(stream);
            using var dataReader = new GmDataReader(binaryReader, wad);
            dataReader.Deserialize();
        }
    }

    private static string[] GetWadFiles() {
        // Benchmarks set the current directory to the temp folder in bin.
        var dir = Environment.CurrentDirectory;
        dir = Path.Combine(
            dir,
            "..",
            "..",
            "..",
            "..",
            "..",
            "..",
            "..",
            "wads"
        );
        return Directory.GetFiles(dir, "*.wad");
    }
}
