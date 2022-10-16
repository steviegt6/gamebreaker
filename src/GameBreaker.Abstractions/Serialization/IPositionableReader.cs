namespace GameBreaker.Abstractions.Serialization
{
    public interface IPositionableReader : IPositionable
    {
        byte ReadByte();

        byte[] ReadBytes(int count);
        
        bool ReadBoolean();

        char ReadChar();

        char[] ReadChars(int count);

        short ReadInt16();

        ushort ReadUInt16();

        Int24 ReadInt24();

        UInt24 ReadUInt24();

        int ReadInt32();

        uint ReadUInt32();

        long ReadInt64();

        ulong ReadUInt64();

        float ReadSingle();

        double ReadDouble();

        GmString ReadGmString();
    }
}