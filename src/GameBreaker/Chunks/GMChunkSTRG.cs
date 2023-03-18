﻿using System;
using System.Collections.Generic;
using System.Text;
using GameBreaker.Models;

namespace GameBreaker.Chunks
{
    public class GMChunkSTRG : GMChunk
    {
        public GMPointerList<GMString> List;

        public override void Serialize(GMDataWriter writer)
        {
            base.Serialize(writer);

            List.Serialize(writer, (writer, i, count) =>
            {
                // Align to 4 byte offsets if necessary
                if (writer.VersionInfo.AlignStringsTo4)
                    writer.Pad(4);
            });

            writer.Pad(128);
        }

        public override void Deserialize(GMDataReader reader)
        {
            base.Deserialize(reader);

            List = new GMPointerList<GMString>();
            List.Deserialize(reader, null, null, (GMDataReader reader, bool notLast) =>
            {
                int ptr = reader.ReadInt32();

                // Check if strings are aligned to 4 byte offsets
                reader.VersionInfo.AlignStringsTo4 &= (ptr % 4 == 0);

                return reader.ReadPointerObject<GMString>(ptr, notLast);
            });
        }
    }
}
