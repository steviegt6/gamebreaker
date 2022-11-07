// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System;
using System.Text;
using GameBreaker.Abstractions;
using GameBreaker.Abstractions.Serialization;

namespace GameBreaker.Serialization;

public class SlicedReader : IPositionableReader
{
    public Encoding Encoding => Reader.Encoding;

    public virtual long Length {
        get => EndSlice - StartSlice;
        set => throw new NotSupportedException("Cannot adjust length of a sliced reader!");
    }

    public virtual long Position {
        get => Reader.Position - StartSlice;
        set => Reader.Position = value + StartSlice;
    }

    protected virtual StreamedReader Reader { get; }

    protected virtual long StartSlice { get; }

    protected virtual long EndSlice { get; }

    public SlicedReader(long startSlice, long endSlice, StreamedReader reader) {
        Reader = reader;
        StartSlice = startSlice;
        EndSlice = endSlice;
    }

    public virtual byte ReadByte() {
        return Reader.ReadByte();
    }

    public virtual byte[] ReadBytes(int count) {
        return Reader.ReadBytes(count);
    }

    public virtual bool ReadBoolean() {
        return Reader.ReadBoolean();
    }

    public virtual char ReadChar() {
        return Reader.ReadChar();
    }

    public virtual char[] ReadChars(int count) {
        return Reader.ReadChars(count);
    }

    public virtual short ReadInt16() {
        return Reader.ReadInt16();
    }

    public virtual ushort ReadUInt16() {
        return Reader.ReadUInt16();
    }

    public virtual Int24 ReadInt24() {
        return Reader.ReadInt24();
    }

    public virtual UInt24 ReadUInt24() {
        return Reader.ReadUInt24();
    }

    public virtual int ReadInt32() {
        return Reader.ReadInt32();
    }

    public virtual uint ReadUInt32() {
        return Reader.ReadUInt32();
    }

    public virtual long ReadInt64() {
        return Reader.ReadInt64();
    }

    public virtual ulong ReadUInt64() {
        return Reader.ReadUInt64();
    }

    public virtual float ReadSingle() {
        return Reader.ReadSingle();
    }

    public virtual double ReadDouble() {
        return Reader.ReadDouble();
    }

    public virtual GmString ReadGmString() {
        return Reader.ReadGmString();
    }

    public virtual IPositionableReader CreateChildReader(long length) {
        return Reader.CreateChildReader(length);
    }

    protected virtual void Dispose(bool disposing) {
        if (disposing) {
            Reader.Dispose();
        }
    }

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}