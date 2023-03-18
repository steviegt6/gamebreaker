using System;

namespace GameBreaker.Util;

/// <summary>
/// Represents a part of a buffer. Keeps a reference to the source array for its lifetime.
/// </summary>
public class BufferRegion {
    private readonly byte[] _internalRef;
    public Memory<byte> Memory;

    public int Length => Memory.Length;

    public BufferRegion(byte[] data) {
        _internalRef = data;
        Memory = _internalRef.AsMemory();
    }

    public BufferRegion(byte[] source, int start, int count) {
        _internalRef = source;
        Memory = _internalRef.AsMemory().Slice(start, count);
    }
}
