﻿using System;
using System.Collections.Generic;
using System.Text;
using GameBreaker.Models;

namespace GameBreaker.Chunks
{
    public class GMChunkSEQN : GMChunk
    {
        public GMUniquePointerList<GMSequence> List;

        public override void Serialize(GMDataWriter writer)
        {
            base.Serialize(writer);

            if (List == null)
                return;

            writer.Write((uint)1);

            List.Serialize(writer);
        }

        public override void Deserialize(GMDataReader reader)
        {
            base.Deserialize(reader);
            
            // This chunk can just be empty sometimes
            if (Length == 0)
                return;

            int chunkVersion = reader.ReadInt32();
            if (chunkVersion != 1)
                reader.Warnings.Add(new GMWarning($"SEQN version is {chunkVersion}, expected 1"));

            List = new GMUniquePointerList<GMSequence>();
            List.Deserialize(reader);
        }
    }
}
