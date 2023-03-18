using System;
using System.Collections.Generic;
using System.Text;
using GameBreaker.Models;

namespace GameBreaker.Chunks
{
    public class GMChunkTPAG : GMChunk
    {
        public GMPointerList<GMTextureItem> List;

        public override void Serialize(GMDataWriter writer)
        {
            base.Serialize(writer);

            List.Serialize(writer);
        }

        public override void Deserialize(GMDataReader reader)
        {
            base.Deserialize(reader);

            List = new GMPointerList<GMTextureItem>();
            List.Deserialize(reader);
        }
    }
}
