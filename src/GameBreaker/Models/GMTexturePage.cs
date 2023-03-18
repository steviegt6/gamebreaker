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

using GameBreaker.Chunks;
using ICSharpCode.SharpZipLib.BZip2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GameBreaker.Models
{
    /// <summary>
    /// Contains a GameMaker texture page.
    /// </summary>
    public class GMTexturePage : IGMSerializable
    {
        public uint Scaled;
        public uint GeneratedMips;
        public GMTextureData TextureData;

        // 2022.9+ fields
        public int TextureWidth;
        public int TextureHeight;
        public int IndexInGroup;

        public void Serialize(GMDataWriter writer)
        {
            writer.Write(Scaled);
            if (writer.VersionInfo.Major >= 2) 
                writer.Write(GeneratedMips);
            if (writer.VersionInfo.IsVersionAtLeast(2022, 3))
            {
                TextureData.WriteLengthOffset = writer.Offset;
                writer.Write(0);
            }
            if (writer.VersionInfo.IsVersionAtLeast(2022, 9))
            {
                writer.Write(TextureWidth);
                writer.Write(TextureHeight);
                writer.Write(IndexInGroup);
            }
            writer.WritePointer(TextureData);
        }

        public void Deserialize(GMDataReader reader)
        {
            Scaled = reader.ReadUInt32();
            if (reader.VersionInfo.Major >= 2) 
                GeneratedMips = reader.ReadUInt32();
            if (reader.VersionInfo.IsVersionAtLeast(2022, 3))
                reader.ReadInt32(); // Ignore the data length (for now, at least)
            if (reader.VersionInfo.IsVersionAtLeast(2022, 9))
            {
                TextureWidth = reader.ReadInt32();
                TextureHeight = reader.ReadInt32();
                IndexInGroup = reader.ReadInt32();
            }
            TextureData = reader.ReadPointerObjectUnique<GMTextureData>();
        }
    }

    public class GMTextureData : IGMSerializable
    {
        // The PNG or QOI+BZip2 data
        public BufferRegion Data;

        // Fields specifically for QOI *only*
        public bool IsQoi;
        public bool IsBZip2;
        public short QoiWidth = -1;
        public short QoiHeight = -1;
        public uint QoiLength = 0;

        public static readonly byte[] PNGHeader = new byte[8] { 137, 80, 78, 71, 13, 10, 26, 10 };
        public static readonly byte[] QOIandBZip2Header = new byte[4] { 50, 122, 111, 113 };
        public static readonly byte[] QOIHeader = new byte[4] { 102, 105, 111, 113 };

        // Offset to write the data length to when serializing
        public int WriteLengthOffset = -1;

        private void WriteLength(GMDataWriter writer, int length)
        {
            if (writer.VersionInfo.IsVersionAtLeast(2022, 3))
            {
                int returnTo = writer.Offset;

                writer.Offset = WriteLengthOffset;
                writer.Write(length);

                writer.Offset = returnTo;
            }
        }

        public void Serialize(GMDataWriter writer)
        {
            writer.Pad(128);
            writer.WriteObjectPointer(this);
            if (IsQoi && IsBZip2)
            {
                // Need to compress the data now
                writer.Write(QOIandBZip2Header);
                writer.Write(QoiWidth);
                writer.Write(QoiHeight);
                if (writer.VersionInfo.IsVersionAtLeast(2022, 5))
                    writer.Write(QoiLength);
                using MemoryStream input = new MemoryStream(Data.Memory.ToArray());
                using MemoryStream output = new MemoryStream(1024);
                BZip2.Compress(input, output, false, 9);
                byte[] final = output.ToArray();
                writer.Write(final);
                WriteLength(writer, final.Length + 8);
            }
            else
            {
                writer.Write(Data);
                WriteLength(writer, Data.Length);
            }
        }

        public void Deserialize(GMDataReader reader)
        {
            int startOffset = reader.Offset;

            byte[] header = reader.ReadBytes(8).Memory.ToArray();
            if (!header.SequenceEqual(PNGHeader))
            {
                reader.Offset = startOffset;
                if (header.Take(4).SequenceEqual(QOIandBZip2Header))
                {
                    // This is in QOI + BZip2 format
                    IsQoi = true;
                    IsBZip2 = true;
                    reader.VersionInfo.SetVersion(2022, 1);
                    reader.Offset += 4;

                    QoiWidth = reader.ReadInt16();
                    QoiHeight = reader.ReadInt16();
                    if (reader.VersionInfo.IsVersionAtLeast(2022, 5))
                        QoiLength = reader.ReadUInt32();

                    // Queue the data to be decompressed later, and in parallel
                    reader.TexturesToDecompress.Add((this, reader.Offset));
                    return;
                }
                else if (header.Take(4).SequenceEqual(QOIHeader))
                {
                    // This is in QOI format
                    IsQoi = true;
                    reader.VersionInfo.SetVersion(2022, 1);

                    int dataStart = reader.Offset;

                    reader.Offset += 8; // skip header and Width/Height, not needed
                    int length = reader.ReadInt32();

                    reader.Offset = dataStart;
                    Data = reader.ReadBytes(length + 12);
                    return;
                }
                else
                    reader.Warnings.Add(new GMWarning("PNG, QOI, or QOI+BZ2 header expected.", GMWarning.WarningLevel.Bad));
            }

            // Parse PNG data
            int type;
            do
            {
                uint length = (uint)reader.ReadByte() << 24 | (uint)reader.ReadByte() << 16 | (uint)reader.ReadByte() << 8 | (uint)reader.ReadByte();
                type = reader.ReadInt32();
                reader.Offset += (int)length + 4;
            }
            while (type != 0x444E4549 /* IEND */);

            int texLength = reader.Offset - startOffset;
            reader.Offset = startOffset;
            Data = reader.ReadBytes(texLength);
        }
    }
}
