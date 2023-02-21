using System.Collections.Generic;
using GameBreaker.Serial.Exceptions;
using GameBreaker.Serial.Extensions;
using CdFactory = System.Func<GameBreaker.Serial.IO.IFF.IChunkData>;
using CdFactories = System.Collections.Generic.Dictionary<
    string,
    System.Func<GameBreaker.Serial.IO.IFF.IChunkData>
>;
using IffFactory = System.Func<GameBreaker.Serial.IO.IFF.IffFile>;
using IffFactories = System.Collections.Generic.Dictionary<
    string,
    System.Func<GameBreaker.Serial.IO.IFF.IffFile>
>;
using Chunks = System.Collections.Generic.Dictionary<
    string,
    GameBreaker.Serial.IO.IFF.Chunk
>;

namespace GameBreaker.Serial.IO.IFF;

public abstract class FormChunkData : IChunkData {
    protected CdFactories CdFactories { get; }

    protected IffFactories IffFactories { get; }

    protected internal IffFile? IffFile { get; internal set; }

    protected List<string> ChunkNames { get; } = new();

    public Chunks Chunks { get; } = new();

    protected FormChunkData(
        CdFactories? cdFactories = null,
        IffFactories? iffFactories = null
    ) {
        CdFactories = cdFactories ?? new CdFactories();
        IffFactories = iffFactories ?? new IffFactories();
    }

    protected void RegisterFactory(
        string id,
        CdFactory cdFactory,
        IffFactory iffFactory
    ) {
        CdFactories.Add(id, cdFactory);
        IffFactories.Add(id, iffFactory);
    }

    public virtual void Serialize(IWriter writer, IffFile iffFile) {
        foreach (var name in ChunkNames) {
            if (!Chunks.TryGetValue(name, out var chunk))
                continue;

            writer.Write(name.ToCharArray());
            writer.WriteLength(() => {
                new SerializableChunk(chunk).Serialize(writer, this, iffFile);

                if (iffFile.Metadata.AlignFinalChunk || name != ChunkNames[^1])
                    writer.Align(iffFile.Metadata.ChunkAlignment);
            });
        }
    }

    public virtual void Deserialize(
        IReader reader,
        IffFile iffFile,
        ChunkPosInfo posInfo
    ) {
        var offsets = ResolveChunks(reader, iffFile, posInfo);

        reader.Position = posInfo.Start;

        for (var i = 0; i < ChunkNames.Count; i++) {
            reader.Position = offsets[i] + 4; // 4 to skip past chunk name

            if (!CdFactories.TryGetValue(ChunkNames[i], out var cdFactory))
                throw new DeserializationException(
                    $"Invalid IFF: unknown/unsupported chunk \"{ChunkNames[i]}\""
                );

            if (!IffFactories.TryGetValue(ChunkNames[i], out var iffFactory))
                throw new DeserializationException(
                    $"Invalid IFF: unknown/unsupported chunk \"{ChunkNames[i]}\""
                );

            var chunk = new Chunk(cdFactory(), iffFactory());
            chunk.Deserialize(reader);
            Chunks.Add(ChunkNames[i], chunk);
        }
    }

    protected abstract List<int> ResolveChunks(
        IReader reader,
        IffFile iffFile,
        ChunkPosInfo posInfo
    );
}
