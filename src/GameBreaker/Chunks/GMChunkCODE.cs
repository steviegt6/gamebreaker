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
    public class GMChunkCODE : GMChunk
    {
        public GMUniquePointerList<GMCode> List;

        public override void Serialize(GmDataWriter writer)
        {
            base.Serialize(writer);

            if (List != null)
            {
                if (writer.VersionInfo.FormatID <= 14)
                    List.Serialize(writer);
                else
                {
                    List.Serialize(writer, (GmDataWriter writer, int index, int count) =>
                    {
                        if (index == 0)
                        {
                            // Serialize bytecode before entries
                            foreach (GMCode c in List)
                            {
                                if (!writer.PointerOffsets.ContainsKey(c.BytecodeEntry))
                                {
                                    writer.WriteObjectPointer(c.BytecodeEntry);
                                    c.BytecodeEntry.Serialize(writer);
                                }
                            }
                        }
                    });
                }
            }
        }

        public override void Deserialize(GmDataReader reader)
        {
            base.Deserialize(reader);

            if (Length == 0)
                return; // In YYC, before bytecode 17, CODE is empty

            List = new GMUniquePointerList<GMCode>();
            List.Deserialize(reader);
        }
    }
}
