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
    /// Contains a GameMaker sequence.
    /// </summary>
    public class GMAnimCurve : IGMSerializable
    {
        public enum GraphTypeEnum
        {
            unknown1 = 0,
            unknown2 = 1
        }

        public GMString Name;
        public GraphTypeEnum GraphType;
        public GMList<Channel> Channels;

        public void Serialize(GmDataWriter writer)
        {
            Serialize(writer, true);
        }

        public void Serialize(GmDataWriter writer, bool includeName)
        {
            if (includeName)
                writer.WritePointerString(Name);
            writer.Write((uint)GraphType);

            Channels.Serialize(writer);
        }

        public void Deserialize(GmDataReader reader)
        {
            Deserialize(reader, true);
        }

        public void Deserialize(GmDataReader reader, bool includeName)
        {
            if (includeName)
                Name = reader.ReadStringPointerObject();
            GraphType = (GraphTypeEnum)reader.ReadUInt32();

            Channels = new GMList<Channel>();
            Channels.Deserialize(reader);
        }

        public override string ToString()
        {
            return $"Animation Curve: \"{Name.Content}\"";
        }

        public class Channel : IGMSerializable
        {
            public enum FunctionTypeEnum
            {
                Linear = 0,
                Smooth = 1,
                Bezier = 2
            }

            public GMString Name;
            public FunctionTypeEnum FunctionType;
            public ushort Iterations;
            public GMList<Point> Points;

            public void Serialize(GmDataWriter writer)
            {
                writer.WritePointerString(Name);
                writer.Write((uint)FunctionType);
                writer.Write((uint)Iterations);

                Points.Serialize(writer);
            }

            public void Deserialize(GmDataReader reader)
            {
                Name = reader.ReadStringPointerObject();
                FunctionType = (FunctionTypeEnum)reader.ReadUInt32();
                Iterations = (ushort)reader.ReadUInt32();

                Points = new GMList<Point>();
                Points.Deserialize(reader);
            }

            public override string ToString()
            {
                return $"Animation Curve Channel: \"{Name.Content}\"";
            }

            public class Point : IGMSerializable
            {
                public float X;
                public float Value;

                public float BezierX0; // Bezier only
                public float BezierY0;
                public float BezierX1;
                public float BezierY1;

                public void Serialize(GmDataWriter writer)
                {
                    writer.Write(X);
                    writer.Write(Value);

                    if (writer.VersionInfo.IsVersionAtLeast(2, 3, 1))
                    {
                        writer.Write(BezierX0);
                        writer.Write(BezierY0);
                        writer.Write(BezierX1);
                        writer.Write(BezierY1);
                    }
                    else
                        writer.Write(0);
                }

                public void Deserialize(GmDataReader reader)
                {
                    X = reader.ReadSingle();
                    Value = reader.ReadSingle();

                    if (reader.ReadUInt32() != 0) // in 2.3 a int with the value of 0 would be set here,
                    {                             // it cannot be version 2.3 if this value isn't 0
                        reader.VersionInfo.SetVersion(2, 3, 1);
                        reader.Offset -= 4;
                    }
                    else
                    {
                        if (reader.ReadUInt32() == 0)              // At all points (besides the first one)
                            reader.VersionInfo.SetVersion(2, 3, 1); // if BezierX0 equals to 0 (the above check)
                        reader.Offset -= 8;                        // then BezierY0 equals to 0 as well (the current check)
                    }

                    if (reader.VersionInfo.IsVersionAtLeast(2, 3, 1))
                    {
                        BezierX0 = reader.ReadSingle();
                        BezierY0 = reader.ReadSingle();
                        BezierX1 = reader.ReadSingle();
                        BezierY1 = reader.ReadSingle();
                    }
                    else
                        reader.Offset += 4;
                }
            }
        }
    }
}
