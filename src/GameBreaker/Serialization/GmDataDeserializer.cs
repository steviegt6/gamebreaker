// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.
// Copyright (c) colinator27. Licensed under the MIT License.
// See the LICENSE-DogScepter file in the repository root for full terms and conditions.

using System;
using System.Collections.Generic;
using System.Text;
using GameBreaker.Abstractions;
using GameBreaker.Abstractions.IFF;
using GameBreaker.Abstractions.IFF.GM;
using GameBreaker.Abstractions.Serialization;
using GameBreaker.IFF;

namespace GameBreaker.Serialization
{
    public class GmDataDeserializer : IGmDataDeserializer
    {
        public virtual IGameMakerFile GameMakerFile { get; } = null!;

        public virtual GmVersionInfo VersionInfo => GameMakerFile.VersionInfo;

        protected virtual IPositionableReader Reader { get; }

        protected virtual Dictionary<int, IGmSerializable> PointerOffsets { get; } = new();

        // protected virtual Dictionary<int, Instruction> Instructions { get; } = new();

        // protected virtual List<(GmTextureData, int)> TexturesToDecompress { get; } = new();

        public GmDataDeserializer(IPositionableReader reader) {
            Reader = reader;
        }

        #region IGmDataDeserializer Impl

        public virtual T ReadPointer<T>(int ptr)
            where T : IGmSerializable, new() {
            throw new NotImplementedException();
        }

        public virtual T ReadPointer<T>()
            where T : IGmSerializable, new() {
            throw new NotImplementedException();
        }

        public virtual T ReadPointerObject<T>(int ptr, bool returnAfter = true)
            where T : IGmSerializable, new() {
            throw new NotImplementedException();
        }

        public virtual T ReadPointerObject<T>()
            where T : IGmSerializable, new() {
            throw new NotImplementedException();
        }

        public virtual T ReadPointerObjectUnique<T>(int ptr, bool returnAfter = true)
            where T : IGmSerializable, new() {
            throw new NotImplementedException();
        }

        public virtual T ReadPointerObjectUnique<T>()
            where T : IGmSerializable, new() {
            throw new NotImplementedException();
        }

        public virtual GmString ReadStringPointer() {
            throw new NotImplementedException();
        }

        public virtual GmString ReadStringPointerObject() {
            throw new NotImplementedException();
        }

        public virtual IGmDataDeserializer CreateChildDeserializer(long length) {
            return new GmDataDeserializer(Reader.CreateChildReader(length));
        }

        #endregion

        #region IPositionableReader Impl

        public virtual Encoding Encoding => Reader.Encoding;

        public virtual long Length {
            get => Reader.Length;
            set => Reader.Length = value;
        }

        public virtual long Position {
            get => Reader.Position;
            set => Reader.Position = value;
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

        #endregion

        #region IDisposable Impl

        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                Reader.Dispose();
            }
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}