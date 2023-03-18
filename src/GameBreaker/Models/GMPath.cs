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

namespace GameBreaker.Models
{
    /// <summary>
    /// Contains a GameMaker path.
    /// </summary>
    public class GMPath : IGMNamedSerializable
    {
        public GMString Name { get; set; }
        public bool Smooth;
        public bool Closed;
        public uint Precision;
        public GMList<Point> Points;

        public void Serialize(GmDataWriter writer)
        {
            writer.WritePointerString(Name);
            writer.WriteWideBoolean(Smooth);
            writer.WriteWideBoolean(Closed);
            writer.Write(Precision);
            Points.Serialize(writer);
        }

        public void Deserialize(GmDataReader reader)
        {
            Name = reader.ReadStringPointerObject();
            Smooth = reader.ReadBoolean(wide: true);
            Closed = reader.ReadBoolean(wide: true);
            Precision = reader.ReadUInt32();
            Points = new GMList<Point>();
            Points.Deserialize(reader);
        }

        public override string ToString()
        {
            return $"Path: \"{Name.Content}\"";
        }

        public class Point : IGMSerializable
        {
            public float X;
            public float Y;
            public float Speed;

            public void Serialize(GmDataWriter writer)
            {
                writer.Write(X);
                writer.Write(Y);
                writer.Write(Speed);
            }

            public void Deserialize(GmDataReader reader)
            {
                X = reader.ReadSingle();
                Y = reader.ReadSingle();
                Speed = reader.ReadSingle();
            }
        }
    }
}
