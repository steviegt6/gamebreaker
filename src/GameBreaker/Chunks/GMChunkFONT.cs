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
    public class GMChunkFONT : GMChunk
    {
        public GMUniquePointerList<GMFont> List;

        public BufferRegion Padding;

        public override void Serialize(GMDataWriter writer)
        {
            base.Serialize(writer);

            List.Serialize(writer);

            if (Padding == null)
            {
                for (ushort i = 0; i < 0x80; i++)
                    writer.Write(i);
                for (ushort i = 0; i < 0x80; i++)
                    writer.Write((ushort)0x3f);
            }
            else
                writer.Write(Padding);
        }

        public override void Deserialize(GMDataReader reader)
        {
            base.Deserialize(reader);

            DoFormatCheck(reader);

            List = new GMUniquePointerList<GMFont>();
            List.Deserialize(reader);

            Padding = reader.ReadBytes(512);
        }

        private void DoFormatCheck(GMDataReader reader)
        {
            // Check for new "Ascender" field introduced in 2022.2, by attempting to parse old font data format
            if (reader.VersionInfo.IsVersionAtLeast(2, 3) && !reader.VersionInfo.IsVersionAtLeast(2022, 2))
            {
                int returnTo = reader.Offset;

                int fontCount = reader.ReadInt32();
                if (fontCount > 0)
                {
                    int lowerBound = reader.Offset + (fontCount * 4);
                    int upperBound = EndOffset - 512;

                    int firstFontPtr = reader.ReadInt32();
                    int endPtr = (fontCount >= 2 ? reader.ReadInt32() : upperBound);

                    reader.Offset = firstFontPtr + (11 * 4);

                    int glyphCount = reader.ReadInt32();
                    bool invalidFormat = false;
                    if (glyphCount > 0)
                    {
                        int glyphPtrOffset = reader.Offset;

                        if (glyphCount >= 2)
                        {
                            // Check validity of first glyph
                            int firstGlyph = reader.ReadInt32() + (7 * 2);
                            int secondGlyph = reader.ReadInt32();
                            if (firstGlyph < lowerBound || firstGlyph > upperBound ||
                                secondGlyph < lowerBound || secondGlyph > upperBound)
                            {
                                invalidFormat = true;
                            }

                            if (!invalidFormat)
                            {
                                // Check the length of the end of this glyph
                                reader.Offset = firstGlyph;
                                int kerningLength = (reader.ReadUInt16() * 4);
                                reader.Offset += kerningLength;

                                if (reader.Offset != secondGlyph)
                                    invalidFormat = true;
                            }
                        }

                        if (!invalidFormat)
                        {
                            // Check last glyph
                            reader.Offset = glyphPtrOffset + ((glyphCount - 1) * 4);

                            int lastGlyph = reader.ReadInt32() + (7 * 2);
                            if (lastGlyph < lowerBound || lastGlyph > upperBound)
                                invalidFormat = true;
                            if (!invalidFormat)
                            {
                                // Check the length of the end of this glyph (done when checking endPtr below)
                                reader.Offset = lastGlyph;
                                int kerningLength = (reader.ReadUInt16() * 4);
                                reader.Offset += kerningLength;

                                if (fontCount == 1 && reader.VersionInfo.AlignChunksTo16)
                                {
                                    // If we only have one font, align to 16 byte chunk boundary if needed
                                    reader.Pad(16);
                                }
                            }
                        }
                    }

                    if (invalidFormat || reader.Offset != endPtr)
                    {
                        // We didn't end up where we expected! This is most likely 2022.2+ font data
                        reader.VersionInfo.SetVersion(2022, 2);
                    }
                }

                reader.Offset = returnTo;
            }
        }
    }
}
