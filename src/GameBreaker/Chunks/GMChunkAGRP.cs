﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GameBreaker.Models;

namespace GameBreaker.Chunks
{
    public class GMChunkAGRP : GMChunk
    {
        public GMUniquePointerList<GMAudioGroup> List;
        public Dictionary<int, GMData> AudioData;

        public override void Serialize(GMDataWriter writer)
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
                        using (GMDataWriter groupWriter = new GMDataWriter(data, fs, fs.Name, data.Length))
                        {
                            groupWriter.Write();
                            groupWriter.Flush();
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

        public override void Deserialize(GMDataReader reader)
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
                            GMDataReader groupReader = new GMDataReader(fs, fs.Name);
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
