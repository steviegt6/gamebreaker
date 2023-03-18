﻿/* MIT License
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
using GameBreaker.Models;

namespace GameBreaker.Chunks
{
    public class GMChunkSHDR : GMChunk
    {
        public List<GMShader> List;

        public override void Serialize(GMDataWriter writer)
        {
            base.Serialize(writer);

            writer.Write(List.Count);
            foreach (GMShader s in List)
                writer.WritePointer(s);
            foreach (GMShader s in List)
            {
                writer.WriteObjectPointer(s);
                s.Serialize(writer);
            }
        }

        public override void Deserialize(GMDataReader reader)
        {
            base.Deserialize(reader);

            reader.Offset -= 4;
            int chunkEnd = reader.Offset + 4 + reader.ReadInt32();

            int count = reader.ReadInt32();
            int[] ptrs = new int[count];
            for (int i = 0; i < count; i++)
                ptrs[i] = reader.ReadInt32();
            List = new List<GMShader>();
            for (int i = 0; i < count; i++)
            {
                GMShader s = new GMShader();
                reader.Offset = ptrs[i];
                if (i < count - 1)
                    s.Deserialize(reader, ptrs[i + 1]);
                else
                    s.Deserialize(reader, chunkEnd);
                List.Add(s);
            }
        }
    }
}
