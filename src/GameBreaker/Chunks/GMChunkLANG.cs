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
    public class GMChunkLANG : GMChunk
    {
        // Basic format for anyone interested:
        // There's a list of entries wtih string identifiers, and Language objects that contain the values for those entries

        public int Unknown1;
        public int LanguageCount;
        public int EntryCount;

        public List<GMString> EntryIDs = new List<GMString>();
        public List<Language> Languages = new List<Language>();

        public override void Serialize(GMDataWriter writer)
        {
            base.Serialize(writer);

            writer.Write(Unknown1);
            LanguageCount = Languages.Count;
            writer.Write(LanguageCount);
            EntryCount = EntryIDs.Count;
            writer.Write(EntryCount);

            foreach (GMString s in EntryIDs)
                writer.WritePointerString(s);

            foreach (Language l in Languages)
                l.Serialize(writer);
        }

        public override void Deserialize(GMDataReader reader)
        {
            base.Deserialize(reader);

            Unknown1 = reader.ReadInt32();
            LanguageCount = reader.ReadInt32();
            EntryCount = reader.ReadInt32();

            // Read the identifiers for each entry
            for (int i = 0; i < EntryCount; i++)
                EntryIDs.Add(reader.ReadStringPointerObject());

            // Read the data for each language
            for (int i = 0; i < LanguageCount; i++)
            {
                Language l = new Language();
                l.Deserialize(reader, EntryCount);
                Languages.Add(l);
            }
        }

        public class Language : IGMSerializable
        {
            public GMString Name;
            public GMString Region;
            public List<GMString> Entries = new List<GMString>();
            // values that correspond to EntryIDs/EntryCount in main chunk

            public void Serialize(GMDataWriter writer)
            {
                writer.WritePointerString(Name);
                writer.WritePointerString(Region);
                foreach (GMString s in Entries)
                    writer.WritePointerString(s);
            }

            public void Deserialize(GMDataReader reader, int entryCount)
            {
                Name = reader.ReadStringPointerObject();
                Region = reader.ReadStringPointerObject();
                for (uint i = 0; i < entryCount; i++)
                    Entries.Add(reader.ReadStringPointerObject());
            }

            public void Deserialize(GMDataReader reader)
            {
                Deserialize(reader, 0);
            }
        }
    }
}
