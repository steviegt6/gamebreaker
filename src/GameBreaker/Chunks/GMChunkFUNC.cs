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
    public class GMChunkFUNC : GMChunk
    {
        public GMList<GMFunctionEntry> FunctionEntries;
        public GMList<GMLocalsEntry> Locals;

        public override void Serialize(GmDataWriter writer)
        {
            base.Serialize(writer);

            if (writer.VersionInfo.FormatID <= 14)
            {
                for (int i = 0; i < FunctionEntries.Count; i++)
                {
                    FunctionEntries[i].Serialize(writer);
                }
            }
            else
            {
                FunctionEntries.Serialize(writer);
                Locals.Serialize(writer);
            }
        }

        public override void Deserialize(GmDataReader reader)
        {
            base.Deserialize(reader);

            FunctionEntries = new GMList<GMFunctionEntry>();
            Locals = new GMList<GMLocalsEntry>();

            if (reader.VersionInfo.FormatID <= 14)
            {
                int startOff = reader.Offset;
                while (reader.Offset + 12 <= startOff + Length)
                {
                    GMFunctionEntry entry = new GMFunctionEntry();
                    entry.Deserialize(reader);
                    FunctionEntries.Add(entry);
                }
            }
            else
            {
                FunctionEntries.Deserialize(reader);
                Locals.Deserialize(reader);
            }
        }

        public GMFunctionEntry FindOrDefine(string name, GMData data)
        {
            // Search for an existing function entry
            // todo? might want to cache this with a map?
            foreach (var func in FunctionEntries)
            {
                if (func.Name.Content == name)
                    return func;
            }

            // Create a new function, add to list, and return it
            GMFunctionEntry res = new()
            {
                Name = data.DefineString(name)
            };
            FunctionEntries.Add(res);
            return res;
        }

        public GMLocalsEntry FindLocalsEntry(string name)
        {
            // todo? might want to cache this with a map?
            foreach (var entry in Locals)
            {
                if (entry.Name.Content == name)
                    return entry;
            }
            return null;
        }
    }
}
