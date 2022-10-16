using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using GameBreaker.Abstractions;
using GameBreaker.Abstractions.Positionable;
using GameBreaker.Util;

namespace GameBreaker.Positionable
{
    public class BufferedReader : IPositionableReader
    {
        public virtual Encoding Encoding { get; }

        public virtual ulong Length { get; set; }

        public virtual ulong Position { get; set; }

        protected virtual BigArray<byte> Buffer { get; set; }

        public BufferedReader(Encoding? encoding = null, ulong size = 1024 * 1024 * 32) {
            Encoding = encoding ?? Encoding.UTF8;

            // ReSharper disable once VirtualMemberCallInConstructor - Individuals overriding Buffer should adjust their constructors accordingly.
            Buffer = new BigArray<byte>(size);
        }

        public virtual byte ReadByte() {
            Debug.Assert(Position + 1 <= Length, "ReadByte: Read out of bounds.");
            return Buffer[Position++];
        }

        // TODO: lol this is terrible
        public virtual byte[] ReadBytes(int count) {
            Debug.Assert(Position + (ulong) count <= Length, "ReadBytes: Read out of bounds.");
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
            Debug.Assert(Position + (ulong) count <= Length, "ReadChars: Read out of bounds.");
            StringBuilder sb = new();
            for (int i = 0; i < count; i++) sb.Append(ReadChar());
            // TODO: Just convert from a list? Just return a string? What to do?
            return sb.ToString().ToCharArray();
        }

        public virtual short ReadInt16() {
            Debug.Assert(Position + 2 <= Length, "ReadInt16: Read out of bounds.");
            return (short) (Buffer[Position++] | Buffer[Position++] << 8);
        }

        public virtual ushort ReadUInt16() {
            Debug.Assert(Position + 2 <= Length, "ReadUInt16: Read out of bounds.");
            return (ushort) (Buffer[Position++] | Buffer[Position++] << 8);
        }

        public virtual Int24 ReadInt24() {
            Debug.Assert(Position + 3 <= Length, "ReadInt24: Read out of bounds.");
            return new Int24(Buffer[Position++] | Buffer[Position++] << 8 | (sbyte) Buffer[Position++] << 16);
        }

        public virtual UInt24 ReadUInt24() {
            Debug.Assert(Position + 3 <= Length, "ReadUInt24: Read out of bounds.");
            return new UInt24((uint) (Buffer[Position++] | Buffer[Position++] << 8 | Buffer[Position++] << 16));
        }

        public virtual int ReadInt32() {
            Debug.Assert(Position + 4 <= Length, "ReadInt32: Read out of bounds.");
            return Buffer[Position++] | Buffer[Position++] << 8 | Buffer[Position++] << 16 | (sbyte) Buffer[Position++] << 24;
        }

        public virtual uint ReadUInt32() {
            Debug.Assert(Position + 4 <= Length, "ReadUInt32: Read out of bounds.");
            return (uint) (Buffer[Position++] | Buffer[Position++] << 8 | Buffer[Position++] << 16 | Buffer[Position++] << 24);
        }

        public virtual long ReadInt64() {
            Debug.Assert(Position + 8 <= Length, "ReadInt64: Read out of bounds.");
            long val = BitConverter.ToInt64(ReadBytes(8), 0);
            return val;
        }

        public virtual ulong ReadUInt64() {
            Debug.Assert(Position + 8 <= Length, "ReadUInt64: Read out of bounds.");
            ulong val = BitConverter.ToUInt64(ReadBytes(8), 0);
            return val;
        }

        public virtual float ReadSingle() {
            Debug.Assert(Position + 4 <= Length, "ReadSingle: Read out of bounds.");
            float val = BitConverter.ToSingle(ReadBytes(4), 0);
            return val;
        }

        public virtual double ReadDouble() {
            Debug.Assert(Position + 8 <= Length, "ReadDouble: Read out of bounds.");
            double val = BitConverter.ToDouble(ReadBytes(8), 0);
            return val;
        }

        public virtual GmString ReadGmString() {
            Debug.Assert(Position + 4 <= Length, "ReadGmString: Read out of bounds.");
            Position += 4; // Skip length; unreliable according to DogScepter.
            ulong pos = Position;
            while (Buffer[Position] != 0) Position++;
            int length = (int) (Position - pos);
            string val = Encoding.GetString(ReadBytes(length), 0, length);
            Position++; // Skip null terminator.
            return new GmString(val);
        }
    }
}