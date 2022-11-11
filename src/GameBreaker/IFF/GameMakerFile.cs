// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System;
using GameBreaker.Core.Abstractions.Serialization;
using GameBreaker.Core.IFF;
using GameBreaker.Core.Util.Extensions;

namespace GameBreaker.Core.Abstractions.IFF.GM
{
    public class GameMakerFile : RootedFile
    {
        protected class GmChunkedFile : ChunkedFile
        {
            public GmChunkedFile(IChunkedFileMetadata metadata) : base(metadata) { }

            protected override void SerializeChunk(IChunk chunk, IGmDataSerializer serializer, bool last) {
                if (serializer.GameMakerFile is not GameMakerFile gmFile) throw new Exception(); // TODO
                if (gmFile.VersionInfo.AlignChunksTo16 && !last) serializer.Pad(16);
            }

            public override void Deserialize(IGmDataDeserializer deserializer) {
                if (deserializer.GameMakerFile is not GameMakerFile gmFile) throw new Exception(); // TODO
                long start = deserializer.Position;
                
                while (true) {
                    // Ensure enough room to read the header.
                    if (deserializer.Position + 4 > deserializer.Length) break;

                    byte[] header = deserializer.ReadBytes(4);

                    // Ensure enough room to read the chunk length.
                    if (deserializer.Position + 4 > deserializer.Length) {
                        deserializer.Position -= 4;
                        break;
                    }

                    uint length = deserializer.ReadUInt32();

                    // Ensure enough room to read the chunk,
                    if (deserializer.Position + length > deserializer.Length) {
                        deserializer.Position -= 8;
                        break;
                    }

                    var id = new ChunkIdentity(header);
            
                    if (id.Value == "SEQN") gmFile.VersionInfo.SetVersion(2, 3);
                    else if (id.Value == "FEDS") gmFile.VersionInfo.SetVersion(2, 3, 6);
                    else if (id.Value == "FEAT") gmFile.VersionInfo.SetVersion(2022, 8);

                    if (deserializer.Position < deserializer.Length) gmFile.VersionInfo.AlignChunksTo16 &= deserializer.Position % 16 == 0;
                }

                deserializer.Position = start;
                base.Deserialize(deserializer);
            }

            protected override void DeserializeChunk(IChunk chunk, IGmDataDeserializer deserializer) {
                base.DeserializeChunk(chunk, deserializer);
                if (deserializer.GameMakerFile is not GameMakerFile gmFile) throw new Exception(); // TODO
                if (gmFile.VersionInfo.AlignChunksTo16) deserializer.Pad(16);
            }
        }
        
        public virtual GmVersionInfo VersionInfo { get; } = new();

        public GameMakerFile(IChunkedFileMetadata metadata) : base(metadata) { }

        public virtual GmString DefineString(string value, out int index) {
            throw new System.NotImplementedException();
        }

        protected override IChunkedFile CreateRoot(IChunkedFileMetadata metadata) {
            return new GmChunkedFile(metadata);
        }
    }
}