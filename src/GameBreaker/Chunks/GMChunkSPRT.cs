﻿using System;
using System.Collections.Generic;
using System.Text;
using GameBreaker.Models;

namespace GameBreaker.Chunks
{
    public class GMChunkSPRT : GMChunk
    {
        public GMUniquePointerList<GMSprite> List;

        public override void Serialize(GMDataWriter writer)
        {
            base.Serialize(writer);

            List.Serialize(writer, (writer, i, count) =>
            {
                writer.Pad(4);
            });
        }

        public override void Deserialize(GMDataReader reader)
        {
            base.Deserialize(reader);

            List = new GMUniquePointerList<GMSprite>();
            List.Deserialize(reader);
        }
    }
}
