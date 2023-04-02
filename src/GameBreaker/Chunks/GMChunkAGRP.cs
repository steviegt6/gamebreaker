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
using System.IO;
using System.Text;
using GameBreaker.Models;
using GameBreaker.Serial;

namespace GameBreaker.Chunks
{
    public class GMChunkAGRP : GMChunk
    {
        public GMUniquePointerList<GMAudioGroup> List;
        public Dictionary<int, GMData> AudioData;

        public override void Serialize(GmDataWriter writer)
        {
            base.Serialize(writer);

            List.Serialize(writer);

            // Now save the audio groups if possible
            string dir = writer.Data.Directory;
            if (dir != null && AudioData != null)
            {
                foreach (var pair in AudioData)
                {
                    string fname = $"audiogroup{pair.Key}.dat";
                    string path = Path.Combine(dir, fname);
                    using (FileStream fs = new FileStream(path, FileMode.Create))
                    {
                        GMData data = AudioData[pair.Key];
                        writer.Data.Logger?.Invoke($"Writing audio group \"{fname}\"...");
                        using (var groupWriter = new GmDataWriter(data.Length, data, fs.Name))
                        {
                            groupWriter.Write();
                            groupWriter.Flush(fs);
                            foreach (GMWarning w in groupWriter.Warnings)
                            {
                                w.File = fname;
                                writer.Warnings.Add(w);
                            }
                        }
                    }
                }
            }
        }

        public override void Deserialize(GmDataReader reader)
        {
            base.Deserialize(reader);

            List = new GMUniquePointerList<GMAudioGroup>();
            List.Deserialize(reader);

            // Now load the audio groups if possible
            string dir = reader.Data.Directory;
            if (dir != null)
            {
                AudioData = new Dictionary<int, GMData>();
                for (int i = 1; i < List.Count; i++)
                {
                    string fname = $"audiogroup{i}.dat";
                    string path = Path.Combine(dir, fname);
                    if (File.Exists(path))
                    {
                        reader.Data.Logger?.Invoke($"Reading audio group \"{fname}\"...");
                        using (FileStream fs = new FileStream(path, FileMode.Open))
                        {
                            using GmDataReader groupReader = GmDataReader.FromStream(fs, fs.Name);
                            groupReader.Data.Logger = reader.Data.Logger;
                            groupReader.Deserialize();
                            AudioData[i] = groupReader.Data;
                            foreach (GMWarning w in groupReader.Warnings)
                            {
                                w.File = fname;
                                reader.Warnings.Add(w);
                            }
                        }
                    }
                }
            }
        }
    }
}
