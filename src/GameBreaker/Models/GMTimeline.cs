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

using System;
using System.Collections.Generic;
using System.Text;

namespace GameBreaker.Models
{
    /// <summary>
    /// Contains a GameMaker timeline.
    /// </summary>
    public class GMTimeline : IGMSerializable
    {
        public GMString Name;
        public List<(int, GMPointerList<GMObject.Event.Action>)> Moments;
        
        public void Serialize(GmDataWriter writer)
        {
            writer.WritePointerString(Name);
            writer.Write(Moments.Count);
            foreach (var m in Moments)
            {
                writer.Write(m.Item1);
                writer.WritePointer(m.Item2);
            }
            foreach (var m in Moments)
            {
                writer.WriteObjectPointer(m.Item2);
                m.Item2.Serialize(writer);
            }
        }

        public void Deserialize(GmDataReader reader)
        {
            Name = reader.ReadStringPointerObject();
            int count = reader.ReadInt32();
            Moments = new List<(int, GMPointerList<GMObject.Event.Action>)>(count);
            for (int i = count; i > 0; i--)
            {
                int time = reader.ReadInt32();
                Moments.Add((time, reader.ReadPointerObject<GMPointerList<GMObject.Event.Action>>(unique: true)));
            }
        }

        public override string ToString()
        {
            return $"Timeline: \"{Name.Content}\"";
        }
    }
}
