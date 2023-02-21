using GameBreaker.Serial.Exceptions;
using GameBreaker.Serial.Extensions;

namespace GameBreaker.Serial.IO.IFF;

public class IffFile {
    public IffMetadata Metadata { get; }

    public Chunk<FormChunkData> Root { get; }

    public IffFile(FormChunkData root, IffMetadata metadata) {
        root.IffFile = this;
        Root = new Chunk<FormChunkData>(root, this);
        Metadata = metadata;
    }

    public virtual void Serialize(IWriter writer) {
        writer.Write(FORM.ToCharArray());

        // Originally left length serialization up to Chunk, but that fell apart
        // due to needing to align chunks. So now we do it here (and also align
        // chunks in FormChunkData).
        writer.WriteLength(() => {
            Root.Serialize(writer);
        });
    }

    public virtual void Deserialize(IReader reader) {
        if (new string(reader.ReadChars(4)) != FORM)
            throw new DeserializationException("Invalid IFF: wrong header");

        Root.Deserialize(reader);
        
        if (Metadata.VersionInfo.IsInferring())
            Metadata.VersionInfo.MarkInferred();
    }
}
