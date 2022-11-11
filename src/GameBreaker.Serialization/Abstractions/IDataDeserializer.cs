// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using GameBreaker.DataTypes;

namespace GameBreaker.Serialization.Abstractions
{
    public interface IDataDeserializer : IPositionable
    {
        byte ReadByte();

        byte[] ReadBytes(int count);
        
        bool ReadBoolean(bool wide = true);

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
    }
}