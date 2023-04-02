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
    public class GMChunkTGIN : GMChunk
    {
        public GMUniquePointerList<GMTextureGroupInfo> List;

        public override void Serialize(GmDataWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1);

            List.Serialize(writer);
        }

        public override void Deserialize(GmDataReader reader)
        {
            base.Deserialize(reader);

            int chunkVersion = reader.ReadInt32();
            if (chunkVersion != 1)
                reader.Warnings.Add(new GMWarning($"TGIN version is {chunkVersion}, expected 1"));

            DoFormatCheck(reader);

            List = new GMUniquePointerList<GMTextureGroupInfo>();
            List.Deserialize(reader);
        }

        private void DoFormatCheck(GmDataReader reader)
        {
            // Do a length check on first entry to see if this is 2022.9
            if (reader.VersionInfo.IsVersionAtLeast(2, 3) && !reader.VersionInfo.IsVersionAtLeast(2022, 9))
            {
                int returnTo = reader.Offset;

                int tginCount = reader.ReadInt32();
                if (tginCount > 0)
                {
                    int tginPtr = reader.ReadInt32();
                    int secondTginPtr = (tginCount >= 2) ? reader.ReadInt32() : EndOffset;
                    reader.Offset = tginPtr + 4;

                    // Check to see if the pointer located at this address points within this object
                    // If not, then we know we're using a new format!
                    int ptr = reader.ReadInt32();
                    if (ptr < tginPtr || ptr >= secondTginPtr)
                    {
                        reader.VersionInfo.SetVersion(2022, 9);
                    }
                }

                reader.Offset = returnTo;
            }
            
            // Check if this version is 2023.1 or later.
            if (
                reader.VersionInfo.IsVersionAtLeast(2022, 9)
             && !reader.VersionInfo.IsVersionAtLeast(2023, 1)
            ) {
                var returnTo = reader.Offset;

                reader.Offset += 4; // Skip count.

                var firstPtr = reader.ReadUInt32();

                // Navigate to the fourth list pointer, which is different
                // depending on whether this is 2023.1+ or not (either "FontIDs"
                // or "SpineSpriteIDs").
                reader.Offset = (int)(firstPtr + 16 + (sizeof(uint) * 3));
                var fourthPtr = reader.ReadUInt32();

                // We read either the "TexturePageIDs" count or the pointer to
                // the fifth list pointer. If it's a count, it will be less
                // than the previous pointer. Similarly, we can rely on the next
                // pointer being greater than the fourth pointer. This lets us
                // safely assume that this is a 2023.1+ file.
                if (reader.ReadUInt32() <= fourthPtr)
                    reader.VersionInfo.SetVersion(2023, 1);

                reader.Offset = returnTo;
            }
        }
    }
}
