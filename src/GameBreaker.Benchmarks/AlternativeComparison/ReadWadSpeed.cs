using System;
using System.Collections.Generic;
using System.IO;
using BenchmarkDotNet.Attributes;
using DogScepterLib.Core;
using GameBreaker.Serial;
using UndertaleModLib;

namespace GameBreaker.Benchmarks.AlternativeComparison;

[MemoryDiagnoser]
public class ReadWadSpeed {
    // ReSharper disable once UnassignedField.Global
    [Params(1, 10)]
    public int N;

    [Benchmark]
    public void GameBreakerReadWads() {
        for (var i = 0; i < N; i++) {
            foreach (var wad in GetWadFiles()) {
                using var stream = File.OpenRead(wad);
                using var dataReader = GmDataReader.FromStream(stream, wad);
                dataReader.Deserialize();
            }
        }
    }

    [Benchmark]
    public void DogScepterReadWads() {
        for (var i = 0; i < N; i++) {
            foreach (var wad in GetWadFiles()) {
                using var stream = File.OpenRead(wad);
                var dataReader = new GMDataReader(stream, wad);
                dataReader.Deserialize();
            }
        }
    }

    [Benchmark]
    public void UndertaleModLibReadWads() {
        for (var i = 0; i < N; i++) {
            foreach (var wad in GetWadFiles()) {
                using var stream = File.OpenRead(wad);
                using var reader = UndertaleIO.Read(stream);
            }
        }
    }

    private static IEnumerable<string> GetWadFiles() {
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
