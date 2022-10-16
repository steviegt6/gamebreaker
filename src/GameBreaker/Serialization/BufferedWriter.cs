using System;
using System.IO;
using System.Text;
using GameBreaker.Abstractions;
using GameBreaker.Abstractions.Serialization;
using GameBreaker.Util;

namespace GameBreaker.Serialization
{
    public class BufferedWriter : IPositionableWriter
    {
        public virtual Encoding Encoding { get; }

        public virtual ulong Length { get; set; }

        public virtual ulong Position { get; set; }

        protected virtual BigArray<byte> Buffer { get; set; }
        
        public BufferedWriter(Encoding? encoding = null, ulong size = 1024 * 1024 * 32) {
            Encoding = encoding ?? Encoding.UTF8;

            // ReSharper disable once VirtualMemberCallInConstructor - Individuals overriding Buffer should adjust their constructors accordingly.
            Buffer = new BigArray<byte>(size);
        }

        public virtual void Write(byte value) {
            Increase(1);
            Buffer[Position++] = value;
        }

        public virtual void Write(byte[] value) {
            foreach (byte b in value) Write(b);
        }

        public virtual void Write(bool value) {
            Increase(1);
            Buffer[Position++] = value ? (byte) 1 : (byte) 0;
        }

        public virtual void Write(char value) {
            Increase(1);
            Buffer[Position++] = Convert.ToByte(value);
        }

        public virtual void Write(char[] value) {
            foreach (char c in value) Write(c);
        }

        public virtual void Write(short value) {
            Increase(2);
            Buffer[Position++] = (byte)(value & 0xFF);
            Buffer[Position++] = (byte)((value >> 8) & 0xFF);
        }

        public virtual void Write(ushort value) {
            Increase(2);
            Buffer[Position++] = (byte)(value & 0xFF);
            Buffer[Position++] = (byte)((value >> 8) & 0xFF);
        }

        public virtual void Write(Int24 value) {
            Increase(3);
            Buffer[Position++] = (byte)(value.Value & 0xFF);
            Buffer[Position++] = (byte)((value.Value >> 8) & 0xFF);
            Buffer[Position++] = (byte)((value.Value >> 16) & 0xFF);
        }

        public virtual void Write(UInt24 value) {
            Increase(3);
            Buffer[Position++] = (byte)(value.Value & 0xFF);
            Buffer[Position++] = (byte)((value.Value >> 8) & 0xFF);
            Buffer[Position++] = (byte)((value.Value >> 16) & 0xFF);
        }

        public virtual void Write(int value) {
            Increase(4);
            Buffer[Position++] = (byte)(value & 0xFF);
            Buffer[Position++] = (byte)((value >> 8) & 0xFF);
            Buffer[Position++] = (byte)((value >> 16) & 0xFF);
            Buffer[Position++] = (byte)((value >> 24) & 0xFF);
        }

        public virtual void Write(uint value) {
            Increase(4);
            Buffer[Position++] = (byte)(value & 0xFF);
            Buffer[Position++] = (byte)((value >> 8) & 0xFF);
            Buffer[Position++] = (byte)((value >> 16) & 0xFF);
            Buffer[Position++] = (byte)((value >> 24) & 0xFF);
        }

        public virtual void Write(long value) {
            Increase(8);
            Buffer[Position++] = (byte)(value & 0xFF);
            Buffer[Position++] = (byte)((value >> 8) & 0xFF);
            Buffer[Position++] = (byte)((value >> 16) & 0xFF);
            Buffer[Position++] = (byte)((value >> 24) & 0xFF);
            Buffer[Position++] = (byte)((value >> 32) & 0xFF);
            Buffer[Position++] = (byte)((value >> 40) & 0xFF);
            Buffer[Position++] = (byte)((value >> 48) & 0xFF);
            Buffer[Position++] = (byte)((value >> 56) & 0xFF);
        }

        public virtual void Write(ulong value) {
            Increase(8);
            Buffer[Position++] = (byte)(value & 0xFF);
            Buffer[Position++] = (byte)((value >> 8) & 0xFF);
            Buffer[Position++] = (byte)((value >> 16) & 0xFF);
            Buffer[Position++] = (byte)((value >> 24) & 0xFF);
            Buffer[Position++] = (byte)((value >> 32) & 0xFF);
            Buffer[Position++] = (byte)((value >> 40) & 0xFF);
            Buffer[Position++] = (byte)((value >> 48) & 0xFF);
            Buffer[Position++] = (byte)((value >> 56) & 0xFF);
        }

        public virtual void Write(float value) {
            Increase(4);
            byte[] bytes = BitConverter.GetBytes(value);
            Buffer[Position++] = bytes[0];
            Buffer[Position++] = bytes[1];
            Buffer[Position++] = bytes[2];
            Buffer[Position++] = bytes[3];
        }

        public virtual void Write(double value) {
            Increase(8);
            byte[] bytes = BitConverter.GetBytes(value);
            Buffer[Position++] = bytes[0];
            Buffer[Position++] = bytes[1];
            Buffer[Position++] = bytes[2];
            Buffer[Position++] = bytes[3];
            Buffer[Position++] = bytes[4];
            Buffer[Position++] = bytes[5];
            Buffer[Position++] = bytes[6];
            Buffer[Position++] = bytes[7];
        }

        public virtual void Write(GmString value) {
            int length = Encoding.GetByteCount(value.Value);
            Write(length);
            Write(Encoding.GetBytes(value.Value, 0, value.Value.Length));
        }

        protected virtual void Increase(ulong size) {
            Resize(Position + size);
        }
        
        protected virtual void Resize(ulong size) {
            BigArray<byte> buf = Buffer;

            while (size > buf.Size) BigArray<byte>.Resize(ref buf, buf.Size * 2);
            if (Length < size) Length = size;

            Buffer = buf;
        }

        public virtual void Flush(Stream stream) {
            for (int i = 0; i < Buffer.Arrays.Count; i++) {
                ulong Position = BigArray<byte>.MAXIMUM_SIZE * (ulong) i;
                int length = (int) Math.Min(BigArray<byte>.MAXIMUM_SIZE, Length - Position);
                
                // TODO: long is acceptable I guess smh
                stream.Seek((long) Position, SeekOrigin.Begin);
                stream.Write(Buffer.Arrays[i], 0, length);
            }
        }

        public virtual void Flush() {
            throw new NotImplementedException("BufferedReader does not support flushing to an assumed stream!");
        }
    }
}