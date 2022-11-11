// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System;
using System.Collections.Generic;
using GameBreaker.Chunks;
using GameBreaker.Core.Abstractions.IFF.GM;
using GameBreaker.Core.Abstractions.Serialization;

namespace GameBreaker.Util;

partial class Extensions
{
    public static void WriteGuid(this IGmDataSerializer s, Guid guid) {
        s.Write(guid.ToByteArray());
    }

    public static void WriteGameMakerVersion(this IGmDataSerializer s, int major, int minor, int release, int build, GameMakerFile gmFile) {
        s.Write(major);
        s.Write(minor);
        s.Write(release);
        s.Write(build);
        gmFile.VersionInfo.SetVersion(major, minor, release, build);
    }

    public static void WriteInfoFlags(this IGmDataSerializer s, GMSChunk.GEN8.InfoFlags info) {
        s.Write((uint) info);
    }

    public static void WriteFunctionClassification(this IGmDataSerializer s, GMSChunk.GEN8.FunctionClassification functionClassification) {
        s.Write((ulong) functionClassification);
    }

    public static void WriteRoomOrder(this IGmDataSerializer s, List<int> roomOrder) {
        s.Write(roomOrder.Count);
        foreach (int room in roomOrder) s.Write(room);
    }

    public static void WriteRandomUID(this IGmDataSerializer s, List<long> randomUid, GMSChunk.GEN8 chunk, GameMakerFile gmFile) {
        randomUid.Clear();

        int seed = (int) (chunk.Timestamp & 0xFFFFFFFF);
        int numerator = (int) chunk.Timestamp & 0xFFFF;
        int quotient = numerator / 7;
        int infoLocation = Math.Abs(quotient + (chunk.GameID - chunk.DefaultWindowWidth) + chunk.RoomOrder.Count) % 4;
        
        var rand = new Random(seed);
        long firstRand = (long) rand.Next() << 32 | (long) rand.Next();
        long infoNumber = GetInfoNumber(firstRand, gmFile.VersionInfo.RunFromIDE, chunk);

        s.Write(firstRand);
        randomUid.Add(firstRand);

        for (int i = 0; i < 4; i++) {
            if (infoLocation == i) {
                s.Write(infoNumber);
                randomUid.Add(infoNumber);
            }
            else {
                int first = rand.Next();
                int second = rand.Next();
                s.Write(first);
                s.Write(second);
                randomUid.Add(((long) first << 32) | (long) second);
            }
        }
    }
}