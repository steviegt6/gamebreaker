/* MIT License
 * 
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

namespace GameBreaker.Models
{
    /// <summary>
    /// Contains metadata about GameMaker texture groups.
    /// </summary>
    public class GMTextureGroupInfo : IGMSerializable
    {
        public GMString Name;
        public GMList<ResourceID> TexturePageIDs = new GMList<ResourceID>();
        public GMList<ResourceID> SpriteIDs = new GMList<ResourceID>();
        public GMList<ResourceID> SpineSpriteIDs = new GMList<ResourceID>();
        public GMList<ResourceID> FontIDs = new GMList<ResourceID>();
        public GMList<ResourceID> TilesetIDs = new GMList<ResourceID>();

        // 2022.9+ fields
        public GMString Directory;
        public GMString Extension;
        public TextureGroupLoadType LoadType;

        public enum TextureGroupLoadType
        {
            InFile = 0,
            SeparateGroup = 1,
            SeparateTextures = 2
        }

        public void Serialize(GMDataWriter writer)
        {
            writer.WritePointerString(Name);

            if (writer.VersionInfo.IsVersionAtLeast(2022, 9))
            {
                writer.WritePointerString(Directory);
                writer.WritePointerString(Extension);
                writer.Write((int)LoadType);
            }

            writer.WritePointer(TexturePageIDs);
            writer.WritePointer(SpriteIDs);
            writer.WritePointer(SpineSpriteIDs);
            writer.WritePointer(FontIDs);
            writer.WritePointer(TilesetIDs);

            writer.WriteObjectPointer(TexturePageIDs);
            TexturePageIDs.Serialize(writer);
            writer.WriteObjectPointer(SpriteIDs);
            SpriteIDs.Serialize(writer);
            writer.WriteObjectPointer(SpineSpriteIDs);
            SpineSpriteIDs.Serialize(writer);
            writer.WriteObjectPointer(FontIDs);
            FontIDs.Serialize(writer);
            writer.WriteObjectPointer(TilesetIDs);
            TilesetIDs.Serialize(writer);
        }

        public void Deserialize(GMDataReader reader)
        {
            Name = reader.ReadStringPointerObject();
            if (reader.VersionInfo.IsVersionAtLeast(2022, 9))
            {
                Directory = reader.ReadStringPointerObject();
                Extension = reader.ReadStringPointerObject();
                LoadType = (TextureGroupLoadType)reader.ReadInt32();
            }
            TexturePageIDs = reader.ReadPointerObjectUnique<GMList<ResourceID>>();
            SpriteIDs = reader.ReadPointerObjectUnique<GMList<ResourceID>>();
            SpineSpriteIDs = reader.ReadPointerObjectUnique<GMList<ResourceID>>();
            FontIDs = reader.ReadPointerObjectUnique<GMList<ResourceID>>();
            TilesetIDs = reader.ReadPointerObjectUnique<GMList<ResourceID>>();
        }

        public override string ToString()
        {
            return $"Texture Group Info: \"{Name.Content}\"";
        }

        public class ResourceID : IGMSerializable
        {
            public int ID;

            public void Serialize(GMDataWriter writer)
            {
                writer.Write(ID);
            }

            public void Deserialize(GMDataReader reader)
            {
                ID = reader.ReadInt32();
            }
        }
    }
}
