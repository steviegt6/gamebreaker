// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.
// Copyright (c) kryz-h. Licensed under the GPL License, version 3.
// See the LICENSE-UndertaleModTool file in the repository root for full terms and conditions.

using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using GameBreaker.Abstractions;
using GameBreaker.Abstractions.Serialization;

namespace GameBreaker.Serialization
{
    public class StreamedReader : IPositionableReader
    {
        public virtual Encoding Encoding { get; }

        public virtual long Length {
            get => Stream.Length;
            set => Stream.SetLength(value);
        }

        public virtual long Position {
            get => Stream.Position;
            set => Stream.Position = value;
        }

        protected virtual Stream Stream { get; }

        public StreamedReader(Stream stream, Encoding? encoding = null, ulong size = 1024 * 1024 * 32) {
            Stream = stream;
            Encoding = encoding ?? Encoding.UTF8;
        }

        public virtual byte ReadByte() {
            Debug.Assert(Position + 1 <= Length, "ReadByte: Read out of bounds.");
            return (byte) Stream.ReadByte();
        }

        // TODO: lol this is terrible
        public virtual byte[] ReadBytes(int count) {
            Debug.Assert(Position + count <= Length, "ReadBytes: Read out of bounds.");
            List<byte> bytes = new();
            for (int i = 0; i < count; i++) bytes.Add(ReadByte());
            return bytes.ToArray();
        }

        public virtual bool ReadBoolean() {
            Debug.Assert(Position + 1 <= Length, "ReadBoolean: Read out of bounds.");
            int val = ReadByte();
            Debug.Assert(val is 0 or 1, $"ReadBoolean: Value was not 0 or 1 ({val}).");
            return val != 0;
        }

        public virtual char ReadChar() {
            Debug.Assert(Position + 1 <= Length, "ReadBoolean: Read out of bounds.");
            return Convert.ToChar(ReadByte());
        }

        public virtual char[] ReadChars(int count) {
            Debug.Assert(Position + count <= Length, "ReadChars: Read out of bounds.");
            StringBuilder sb = new();
            for (int i = 0; i < count; i++) sb.Append(ReadChar());
            // TODO: Just convert from a list? Just return a string? What to do?
            return sb.ToString().ToCharArray();
        }

        public virtual short ReadInt16() {
            Debug.Assert(Position + 2 <= Length, "ReadInt16: Read out of bounds.");
            return BinaryPrimitives.ReadInt16LittleEndian(ReadBytes(2));
        }

        public virtual ushort ReadUInt16() {
            Debug.Assert(Position + 2 <= Length, "ReadUInt16: Read out of bounds.");
            return BinaryPrimitives.ReadUInt16LittleEndian(ReadBytes(2));
        }

        // TODO: Moving away from direct byte reading and more into BinaryPrimitives/MemoryMarshal would be preferable...
        public virtual Int24 ReadInt24() {
            Debug.Assert(Position + 3 <= Length, "ReadInt24: Read out of bounds.");
            return new Int24(ReadByte() | ReadByte() << 8 | (sbyte) ReadByte() << 16);
        }

        public virtual UInt24 ReadUInt24() {
            Debug.Assert(Position + 3 <= Length, "ReadUInt24: Read out of bounds.");
            return new UInt24((uint) (ReadByte() | ReadByte() << 8 | ReadByte() << 16));
        }

        public virtual int ReadInt32() {
            Debug.Assert(Position + 4 <= Length, "ReadInt32: Read out of bounds.");
            return BinaryPrimitives.ReadInt32LittleEndian(ReadBytes(4));
        }

        public virtual uint ReadUInt32() {
            Debug.Assert(Position + 4 <= Length, "ReadUInt32: Read out of bounds.");
            return BinaryPrimitives.ReadUInt32LittleEndian(ReadBytes(4));
        }

        public virtual long ReadInt64() {
            Debug.Assert(Position + 8 <= Length, "ReadInt64: Read out of bounds.");
            return BitConverter.ToInt64(ReadBytes(8), 0);
        }

        public virtual ulong ReadUInt64() {
            Debug.Assert(Position + 8 <= Length, "ReadUInt64: Read out of bounds.");
            return BitConverter.ToUInt64(ReadBytes(8), 0);
        }

        public virtual float ReadSingle() {
            Debug.Assert(Position + 4 <= Length, "ReadSingle: Read out of bounds.");
            return BitConverter.ToSingle(ReadBytes(4), 0);
        }

        public virtual double ReadDouble() {
            Debug.Assert(Position + 8 <= Length, "ReadDouble: Read out of bounds.");
            return BitConverter.ToDouble(ReadBytes(8), 0);
        }

        public virtual GmString ReadGmString() {
            bool Dbg() {
                byte nullTerm = ReadByte();
                Position--;
                return nullTerm == 0;
            }

            Debug.Assert(Position + 4 <= Length, "ReadGmString: Read out of bounds.");
            int length = ReadInt32();
            string res;
            if (length > 1024) {
                byte[] buf = new byte[length];
                _ = Stream.Read(buf, 0, length);
                res = Encoding.GetString(buf);
            }
            else {
                Span<byte> buf = stackalloc byte[length];
                _ = Stream.Read(buf);
                res = Encoding.GetString(buf);
            }

            Debug.Assert(Dbg(), "ReadGmString: String was not null-terminated!");
            Position++;
            return new GmString(res);
        }

        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                Stream.Close();
                Stream.Dispose();
            }
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}