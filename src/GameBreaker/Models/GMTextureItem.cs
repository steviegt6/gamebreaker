﻿/*
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

using GameBreaker.Project;
using GameBreaker.Project.Util;

namespace GameBreaker.Models
{
    /// <summary>
    /// Contains a GameMaker texture item.
    /// A texture item is the location and size of a single graphic within a texture page.
    /// </summary>
    public class GMTextureItem : IGMSerializable
    {
        /*
         * The way this works is:
         * It renders in a box of size BoundWidth x BoundHeight at some position.
         * TargetX/Y/W/H is relative to the bounding box, anything outside of that is just transparent.
         * SourceX/Y/W/H is part of TexturePage that is drawn over TargetX/Y/W/H
         */

        public ushort SourceX; // The position in the texture sheet.
        public ushort SourceY;
        public ushort SourceWidth; // The dimensions of the image in the texture sheet.
        public ushort SourceHeight;

        public ushort TargetX; // The offset of the image, to account for trimmed 
        public ushort TargetY;
        public ushort TargetWidth; // The dimensions to scale the image to.
        public ushort TargetHeight;

        public ushort BoundWidth; // The image's dimensions.
        public ushort BoundHeight;

        public short TexturePageID = -1; // -1 means this is a user-created item

        // Used for convenience in the project system primarily
        public bool _TileHorizontally = false;
        public bool _TileVertically = false;
        public bool _HasExtraBorder = false;
        public bool _EmptyBorder = false;
        public GMTextureItem _DuplicateOf = null;
        // public TexturePacker.Page.Item _PackItem = null;
        public DSImage _Image = null;
        public DSImage _ImageBeforeCrop = null;

        public GMTextureItem()
        {
        }

        // Creates a new texture entry from a bitmap
        public GMTextureItem(DSImage bitmap)
        {
            ReplaceWith(bitmap);
        }

        public void ReplaceWith(DSImage image)
        {
            _Image = image;

            BoundWidth = (ushort)image.Width;
            BoundHeight = (ushort)image.Height;
            SourceX = 0;
            SourceY = 0;
            SourceWidth = BoundWidth;
            SourceHeight = BoundHeight;
            TargetX = 0;
            TargetY = 0;
            TargetWidth = BoundWidth;
            TargetHeight = BoundHeight;

            TexturePageID = -1;
            _TileHorizontally = false;
            _TileVertically = false;
            _HasExtraBorder = false;
            _EmptyBorder = false;
            _DuplicateOf = null;
            // _PackItem = null;
        }

        public unsafe void Crop()
        {
            _ImageBeforeCrop = _Image;

            int left = BoundWidth, top = BoundHeight, right = 0, bottom = 0;

            fixed (byte* bytePtr = &_Image.Data[0])
            {
                int* ptr = (int*)bytePtr;
                int* basePtr = ptr;

                for (int y = 0; y < BoundHeight; y++)
                {
                    for (int x = 0; x < BoundWidth; x++)
                    {
                        if (*((byte*)ptr + 3) != 0)
                        {
                            if (x < left)
                                left = x;
                            if (y < top)
                                top = y;
                            if (x > right)
                                right = x;
                            if (y > bottom)
                                bottom = y;
                        }
                        ptr++;
                    }
                    ptr += _Image.Width - BoundWidth;
                }
            }

            if (left == BoundWidth && top == BoundHeight)
            {
                // This is fully transparent, just grab one pixel
                SourceWidth = 1;
                SourceHeight = 1;
            }
            else if (left != 0 || top != 0 || right != BoundWidth - 1 || bottom != BoundHeight - 1)
            {
                // We can crop this image
                right++;
                bottom++;
                SourceWidth = (ushort)(right - left);
                SourceHeight = (ushort)(bottom - top);
                TargetWidth = SourceWidth;
                TargetHeight = SourceHeight;
                TargetX = (ushort)left;
                TargetY = (ushort)top;
                _Image = new DSImage(_Image, left, top, right - left, bottom - top);
            }
        }

        public void Serialize(GMDataWriter writer)
        {
            writer.Write(SourceX);
            writer.Write(SourceY);
            writer.Write(SourceWidth);
            writer.Write(SourceHeight);
            writer.Write(TargetX);
            writer.Write(TargetY);
            writer.Write(TargetWidth);
            writer.Write(TargetHeight);
            writer.Write(BoundWidth);
            writer.Write(BoundHeight);
            writer.Write(TexturePageID);
        }

        public void Deserialize(GMDataReader reader)
        {
            SourceX = reader.ReadUInt16();
            SourceY = reader.ReadUInt16();
            SourceWidth = reader.ReadUInt16();
            SourceHeight = reader.ReadUInt16();
            TargetX = reader.ReadUInt16();
            TargetY = reader.ReadUInt16();
            TargetWidth = reader.ReadUInt16();
            TargetHeight = reader.ReadUInt16();
            BoundWidth = reader.ReadUInt16();
            BoundHeight = reader.ReadUInt16();
            TexturePageID = reader.ReadInt16();
        }
    }
}
