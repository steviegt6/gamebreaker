// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using GameBreaker.DataTypes;

namespace GameBreaker.Serialization.Abstractions;

public interface IDataSerializer : IPositionable
{
    void Write(byte value);

    void Write(byte[] value);

    void Write(bool value);

    void Write(char value);

    void Write(char[] value);

    void Write(short value);

    void Write(ushort value);

    void Write(Int24 value);

    void Write(UInt24 value);

    void Write(int value);

    void Write(uint value);

    void Write(long value);

    void Write(ulong value);

    void Write(float value);

    void Write(double value);
}