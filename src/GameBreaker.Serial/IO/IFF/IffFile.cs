using GameBreaker.Serial.Exceptions;

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

        Root.Serialize(writer);
    }

    public virtual void Deserialize(IReader reader) {
        if (new string(reader.ReadChars(4)) != FORM)
            throw new DeserializationException("Invalid IFF: wrong header");

        Root.Deserialize(reader);
    }
}
