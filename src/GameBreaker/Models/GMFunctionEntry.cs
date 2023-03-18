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
    /// Contains information about a GameMaker function.
    /// </summary>
    public class GMFunctionEntry : IGMSerializable
    {
        public GMString Name;
        public int Occurrences;

        public void Serialize(GMDataWriter writer)
        {
            writer.WritePointerString(Name);

            List<(int, GMCode.Bytecode.Instruction.VariableType)> references;
            if (writer.FunctionReferences.TryGetValue(this, out references))
                Occurrences = references.Count;
            else
                Occurrences = 0;

            writer.Write(Occurrences);
            if (Occurrences > 0)
            {
                if (writer.VersionInfo.IsVersionAtLeast(2, 3))
                    writer.Write(references[0].Item1 + 4);
                else
                    writer.Write(references[0].Item1);

                int returnTo = writer.Offset;
                for (int i = 0; i < references.Count; i++)
                {
                    int curr = references[i].Item1;

                    int nextDiff;
                    if (i < references.Count - 1)
                        nextDiff = references[i + 1].Item1 - curr;
                    else
                        nextDiff = ((GMChunkSTRG)writer.Data.Chunks["STRG"]).List.IndexOf(Name);

                    writer.Offset = curr + 4;
                    writer.Write((nextDiff & 0x07FFFFFF) | (((int)references[i].Item2 & 0xF8) << 24));
                }
                writer.Offset = returnTo;
            }
            else
                writer.Write((int)-1);
        }

        public void Deserialize(GMDataReader reader)
        {
            Name = reader.ReadStringPointerObject();
            Occurrences = reader.ReadInt32();
            if (Occurrences > 0)
            {
                int addr;
                if (reader.VersionInfo.IsVersionAtLeast(2, 3))
                    addr = reader.ReadInt32() - 4;
                else
                    addr = reader.ReadInt32();

                // Parse reference chain
                GMCode.Bytecode.Instruction curr;
                for (int i = Occurrences; i > 0; i--)
                {
                    curr = reader.Instructions[addr];
                    if (curr.Function == null)
                    {
                        curr.Function = new GMCode.Bytecode.Instruction.Reference<GMFunctionEntry>((int)curr.Value);
                        curr.Value = null;
                    }
                    curr.Function.Target = this;
                    addr += curr.Function.NextOccurrence;
                }
            }
            else
            {
                if (reader.ReadInt32() != -1)
                    reader.Warnings.Add(new GMWarning("Function with no occurrences, but still has a first occurrence address"));
            }
        }

        public override string ToString()
        {
            return $"Function: \"{Name.Content}\"";
        }

    }
}
