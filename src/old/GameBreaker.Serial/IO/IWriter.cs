using System;
using System.Collections.Generic;

namespace GameBreaker.Serial.IO;

public interface IWriter : IPositionable,
                           IEncodable,
                           IDisposable {
    Dictionary<IPointerSerializable, int> Pointers { get; }

    Dictionary<IPointerSerializable, List<int>> PointerWrites { get; }

    void Write(byte value);

    void Write(byte[] buffer);
    
    void Write(char value);
    
    void Write(char[] buffer);
    
    void Write(bool value, bool wide = false);
    
    void Write(short value);
    
    void Write(ushort value);
    
    void WriteInt24(int value);
    
    void WriteUInt24(uint value);
    
    void Write(int value);
    
    void Write(uint value);

    void Write(long value);
    
    void Write(ulong value);
    
    void Write(float value);
    
    void Write(double value);

    void WritePointer(IPointerSerializable pointer);

    void WriteObjectPointer(IPointerSerializable pointer);
}
