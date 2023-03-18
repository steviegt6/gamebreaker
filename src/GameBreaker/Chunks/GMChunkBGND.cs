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
using GameBreaker.Serial;

namespace GameBreaker.Chunks
{
    public class GMChunkBGND : GMChunk
    {
        public GMUniquePointerList<GMBackground> List;

        public override void Serialize(GmDataWriter writer)
        {
            base.Serialize(writer);

            List.Serialize(writer, (writer, i, count) =>
            {
                // Align to 8 byte offsets if necessary
                if (writer.VersionInfo.AlignBackgroundsTo8)
                    writer.Pad(8);
            });
        }

        public override void Deserialize(GmDataReader reader)
        {
            base.Deserialize(reader);

            List = new GMUniquePointerList<GMBackground>();
            reader.VersionInfo.AlignBackgroundsTo8 = reader.VersionInfo.IsVersionAtLeast(2, 3); // only occurs on newer 2.3.1 versions
            List.Deserialize(reader, null, null, (GmDataReader reader, bool notLast) =>
            {
                int ptr = reader.ReadInt32();

                // Check if backgrounds are aligned to 8 byte offsets
                reader.VersionInfo.AlignBackgroundsTo8 &= (ptr % 8 == 0);

                return reader.ReadPointerObject<GMBackground>(ptr, notLast);
            });
        }
    }
}
