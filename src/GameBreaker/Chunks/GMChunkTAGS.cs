/*
 * Copyright (c) 2023 Tomat & GameBreaker Contributors
 * Copyright (c) 2020 colinator27
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Text;
using GameBreaker.Models;

namespace GameBreaker.Chunks
{
    public class GMChunkTAGS : GMChunk
    {
        public List<GMString> AllTags;
        public GMUniquePointerList<AssetTags> AssetTagsList;

        public override void Serialize(GMDataWriter writer)
        {
            base.Serialize(writer);

            writer.Pad(4);
            writer.Write(1);

            writer.Write(AllTags.Count);
            foreach (GMString s in AllTags)
                writer.WritePointerString(s);

            AssetTagsList.Serialize(writer);
        }

        public override void Deserialize(GMDataReader reader)
        {
            base.Deserialize(reader);

            reader.Pad(4);

            int chunkVersion = reader.ReadInt32();
            if (chunkVersion != 1)
                reader.Warnings.Add(new GMWarning($"TAGS version is {chunkVersion}, expected 1"));

            int count = reader.ReadInt32();
            AllTags = new List<GMString>(count);
            for (int i = count; i > 0; i--)
                AllTags.Add(reader.ReadStringPointerObject());

            AssetTagsList = new GMUniquePointerList<AssetTags>();
            AssetTagsList.Deserialize(reader);
        }

        public class AssetTags : IGMSerializable
        {
            public int ID;
            public List<GMString> Tags;

            public void Serialize(GMDataWriter writer)
            {
                writer.Write(ID);
                writer.Write(Tags.Count);
                foreach (GMString s in Tags)
                    writer.WritePointerString(s);
            }

            public void Deserialize(GMDataReader reader)
            {
                ID = reader.ReadInt32();
                int count = reader.ReadInt32();
                Tags = new List<GMString>(count);
                for (int i = count; i > 0; i--)
                    Tags.Add(reader.ReadStringPointerObject());
            }
        }
    }
}
