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
    public class GMChunkOBJT : GMChunk
    {
        public GMUniquePointerList<GMObject> List;

        public override void Serialize(GMDataWriter writer)
        {
            base.Serialize(writer);

            List.Serialize(writer);
        }

        public override void Deserialize(GMDataReader reader)
        {
            base.Deserialize(reader);

            DoFormatCheck(reader);

            List = new GMUniquePointerList<GMObject>();
            List.Deserialize(reader);
        }

        private void DoFormatCheck(GMDataReader reader)
        {
            // Do a length check on first object to see if 2022.5+
            if (reader.VersionInfo.IsVersionAtLeast(2, 3) && !reader.VersionInfo.IsVersionAtLeast(2022, 5))
            {
                int returnTo = reader.Offset;

                int objectCount = reader.ReadInt32();
                if (objectCount > 0)
                {
                    int firstObjectPtr = reader.ReadInt32();
                    reader.Offset = firstObjectPtr + 64;

                    int vertexCount = reader.ReadInt32();
                    int jumpAmount = 12 + (vertexCount * 8);

                    if (reader.Offset + jumpAmount >= EndOffset || jumpAmount < 0)
                    {
                        // Failed bounds check; 2022.5+
                        reader.VersionInfo.SetVersion(2022, 5);
                    }
                    else
                    {
                        // Jump ahead to the rest of the data
                        reader.Offset += jumpAmount;
                        int eventCount = reader.ReadInt32();
                        if (eventCount != 15)
                        {
                            // Failed event list count check; 2022.5+
                            reader.VersionInfo.SetVersion(2022, 5);
                        }
                        else
                        {
                            int firstEventPtr = reader.ReadInt32();
                            if (reader.Offset + 56 != firstEventPtr)
                            {
                                // Failed first event pointer check (should be right after pointers); 2022.5+
                                reader.VersionInfo.SetVersion(2022, 5);
                            }
                        }
                    }
                }

                reader.Offset = returnTo;
            }
        }
    }
}
