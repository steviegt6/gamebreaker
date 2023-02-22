using System;
using System.Collections.Generic;
using GameBreaker.Serial.IO;
using GameBreaker.Serial.IO.IFF;

namespace GameBreaker.Serial.Chunks;

/*
 * internal class TestFormChunkData : FormChunkData {
    protected override List<int> ResolveChunks(
        IReader reader,
        ChunkPosInfo posInfo
    ) {
        ChunkNames.Clear();
        var offsets = new List<int>();

        while (reader.Position < posInfo.End) {
            offsets.Add(reader.Position);
            var name = new string(reader.ReadChars(4));
            ChunkNames.Add(name);
            RegisterFactory(name, () => new RawByteChunkData(), () => IffFile!);
            var len = reader.ReadInt32();
            reader.Position += len;
        }

        return offsets;
    }
}
 */

public class ModernFormChunkData : FormChunkData {
    private static readonly Dictionary<string, Func<IChunkData>> chunks = new();

    static ModernFormChunkData() {
        Func<IChunkData> defaultFactory<T>() where T : IChunkData, new() {
            return () => new T();
        }

        chunks.Add(GEN8, defaultFactory<Gen8ChunkData>());
    }

    public ModernFormChunkData() {
        Func<IffFile> defaultFactory() {
            return () => IffFile!;
        }

        foreach (var (id, cdFactory) in chunks) {
            RegisterFactory(id, cdFactory, defaultFactory());
        }
    }

    protected override List<int> ResolveChunks(
        IReader reader,
        IffFile iffFile,
        ChunkPosInfo posInfo
    ) {
        ChunkNames.Clear();
        var offsets = new List<int>();

        while (reader.Position < posInfo.End) {
            offsets.Add(reader.Position);
            var name = new string(reader.ReadChars(4));
            ChunkNames.Add(name);

            if (!CdFactories.ContainsKey(name))
                RegisterFactory(
                    name,
                    () => new RawByteChunkData(),
                    () => IffFile!
                );

            var len = reader.ReadInt32();
            reader.Position += len;

            switch (name) {
                case "SEQN":
                    iffFile.Metadata.VersionInfo.AttemptUpdate(
                        GmsVersion.GMS_2_3
                    );
                    break;

                case "FEDS":
                    iffFile.Metadata.VersionInfo.AttemptUpdate(
                        GmsVersion.GMS_2_3_6
                    );
                    break;

                case "FEAT":
                    iffFile.Metadata.VersionInfo.AttemptUpdate(
                        GmsVersion.GMS_2022_8
                    );
                    break;
            }

            if (reader.Position >= posInfo.End)
                continue;

            // Assume chunks are aligned to 16 - if they aren't, uh oh?
            if (reader.Position % 16 == 0)
                iffFile.Metadata.ChunkAlignment = 16;
        }

        return offsets;
    }
}
