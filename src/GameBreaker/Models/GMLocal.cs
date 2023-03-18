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
using System.Diagnostics;
using System.Text;
using GameBreaker.Chunks;

namespace GameBreaker.Models
{
    /// <summary>
    /// Contains a list of local variables, per script entry (Assigned to scripts by having the same Name property).
    /// </summary>
    public class GMLocalsEntry : IGMSerializable
    {
        public GMString Name;
        public List<GMLocal> Entries;

        public GMLocalsEntry()
        {
            // Default constructor
        }

        public GMLocalsEntry(GMString name)
        {
            Name = name;
            Entries = new();
        }

        public void Serialize(GMDataWriter writer)
        {
            writer.Write(Entries.Count);
            writer.WritePointerString(Name);
            for (int i = 0; i < Entries.Count; i++)
            {
                Entries[i].Serialize(writer);
            }
        }

        public void Deserialize(GMDataReader reader)
        {
            Entries = new List<GMLocal>();

            int count = reader.ReadInt32();
            Name = reader.ReadStringPointerObject();

            for (int i = 0; i < count; i++)
            {
                GMLocal local = new GMLocal();
                local.Deserialize(reader);
                Entries.Add(local);
            }
        }

        /// <summary>
        /// Adds a new local to this code local entry.
        /// Updates relevant related information in other locations.
        /// </summary>
        public void AddLocal(GMData data, string name, GMCode code = null)
        {
            Entries.Add(new GMLocal(data, Entries, name));
            var vari = data.GetChunk<GMChunkVARI>();
            if (vari.MaxLocalVarCount < Entries.Count)
                vari.MaxLocalVarCount = Entries.Count;
            if (code != null)
                code.LocalsCount = (short)Entries.Count;
        }

        /// <summary>
        /// Clears all locals from this code local entry.
        /// Updates relevant related information in other locations.
        /// </summary>
        public void ClearLocals(GMCode code)
        {
            Entries.Clear();
            code.LocalsCount = 0;
        }

        /// <summary>
        /// Returns the local ID for the local of the given name, or -1 if not found.
        /// </summary>
        public int LocalIdByName(string name)
        {
            foreach (var local in Entries)
            {
                if (local.Name.Content == name)
                    return local.Index;
            }   
            return -1;
        }

        public override string ToString()
        {
            return $"Locals for \"{Name.Content}\"";
        }
    }

    [DebuggerDisplay("{Name.Content}")]
    public class GMLocal : IGMSerializable
    {
        public int Index;
        public GMString Name;

        public GMLocal()
        {
            // Default constructor
        }

        public GMLocal(GMData data, IList<GMLocal> list, string name)
        {
            if (data.VersionInfo.IsVersionAtLeast(2, 3))
                Name = data.DefineString(name, out Index);
            else
            {
                Name = data.DefineString(name);
                Index = list.Count;
            }
        }    

        public void Serialize(GMDataWriter writer)
        {
            writer.Write(Index);
            writer.WritePointerString(Name);
        }

        public void Deserialize(GMDataReader reader)
        {
            Index = reader.ReadInt32();
            Name = reader.ReadStringPointerObject();
        }

        public override string ToString()
        {
            return Name.Content;
        }


    }
}
