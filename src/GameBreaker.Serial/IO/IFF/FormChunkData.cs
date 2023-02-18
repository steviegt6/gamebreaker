using System.Collections.Generic;
using GameBreaker.Serial.Exceptions;
using GameBreaker.Serial.Extensions;
using Factory = System.Func<GameBreaker.Serial.IO.IFF.IChunkData>;
using Factories = System.Collections.Generic.Dictionary<
    string,
    System.Func<GameBreaker.Serial.IO.IFF.IChunkData>
>;
using Chunks = System.Collections.Generic.Dictionary<
    string,
    GameBreaker.Serial.IO.IFF.Chunk
>;

namespace GameBreaker.Serial.IO.IFF;

public abstract class FormChunkData : IChunkData {
    protected Factories Factories { get; }

    protected List<string> ChunkNames { get; } = new();

    protected Chunks Chunks { get; } = new();

    protected FormChunkData(Factories? factories = null) {
        Factories = factories ?? new Factories();
    }

    public virtual void RegisterFactory(string id, Factory factory) {
        Factories.Add(id, factory);
    }

    public virtual void Serialize(IWriter writer) {
        writer.WriteLength(() => {
            foreach (var t in ChunkNames) {
                if (!Chunks.TryGetValue(t, out var chunk))
                    continue;

                writer.Write(new SerializableChunk(chunk));

                // TODO: PADDING CODE UGH
            }
        });
    }

    public virtual void Deserialize(IReader reader, ChunkPosInfo posInfo) {
        var offsets = ResolveChunks(reader, posInfo);

        reader.Position = posInfo.Start;

        for (var i = 0; i < ChunkNames.Count; i++) {
            reader.Position = offsets[i] + 4; // 4 to skip past chunk name

            if (!Factories.TryGetValue(ChunkNames[i], out var factory))
                throw new DeserializationException(
                    $"Invalid IFF: unknown/unsupported chunk \"{ChunkNames[i]}\""
                );

            var chunk = new Chunk(factory());
            chunk.Deserialize(reader);
            Chunks.Add(ChunkNames[i], chunk);
        }
    }

    protected abstract List<int> ResolveChunks(
        IReader reader,
        ChunkPosInfo posInfo
    );
}
