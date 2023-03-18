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

using GameBreaker.Chunks;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameBreaker.Models
{
    /// <summary>
    /// Contains a GameMaker sound file.
    /// </summary>
    public class GMSound : IGMNamedSerializable
    {
        [Flags]
        public enum AudioEntryFlags : uint
        {
            IsEmbedded = 0x1,
            IsCompressed = 0x2,
            Regular = 0x64,
        }

        public GMString Name { get; set; }
        public AudioEntryFlags Flags;
        public GMString Type;
        public GMString File;
        public uint Effects;
        public float Volume;
        public float Pitch;
        public int AudioID;
        public int GroupID; // In older versions this can also be a "preload" boolean, but it's always true and for now we don't care

        public bool Preload; // legacy (format ID < 14)

        public void Serialize(GmDataWriter writer)
        {
            writer.WritePointerString(Name);
            writer.Write((uint)Flags);
            writer.WritePointerString(Type);
            writer.WritePointerString(File);
            writer.Write(Effects);
            writer.Write(Volume);
            writer.Write(Pitch);
            if (writer.VersionInfo.FormatID >= 14)
            {
                writer.Write(GroupID);
                writer.Write(AudioID);
            }
            else
            {
                // Legacy
                writer.Write(AudioID);
                writer.WriteWideBoolean(Preload);
            }
        }

        public void Deserialize(GmDataReader reader)
        {
            Name = reader.ReadStringPointerObject();
            Flags = (AudioEntryFlags)reader.ReadUInt32();
            Type = reader.ReadStringPointerObject();
            File = reader.ReadStringPointerObject();
            Effects = reader.ReadUInt32();
            Volume = reader.ReadSingle();
            Pitch = reader.ReadSingle();
            if (reader.VersionInfo.FormatID >= 14)
            {
                GroupID = reader.ReadInt32();
                AudioID = reader.ReadInt32();
            }
            else
            {
                // Legacy
                GroupID = -1;
                AudioID = reader.ReadInt32();
                Preload = reader.ReadBoolean(wide: true);
            }
        }

        public override string ToString()
        {
            return $"Sound: \"{Name.Content}\"";
        }
    }
}
