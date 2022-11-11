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
    public static Guid ReadGuid(this IGmDataDeserializer deserializer) {
        return new Guid(deserializer.ReadBytes(16));
    }

    public static (int, int, int, int) ReadGameMakerVersion(this IGmDataDeserializer d, GameMakerFile gmFile) {
        int major = d.ReadInt32();
        int minor = d.ReadInt32();
        int release = d.ReadInt32();
        int build = d.ReadInt32();
        gmFile.VersionInfo.SetVersion(major, minor, release, build);
        return (major, minor, release, build);
    }

    public static GMSChunk.GEN8.InfoFlags ReadInfoFlags(this IGmDataDeserializer d) {
        return (GMSChunk.GEN8.InfoFlags) d.ReadUInt32();
    }

    public static GMSChunk.GEN8.FunctionClassification ReadFunctionClassification(this IGmDataDeserializer d) {
        return (GMSChunk.GEN8.FunctionClassification) d.ReadUInt64();
    }

    public static List<int> ReadRoomOrder(this IGmDataDeserializer d) {
        int count = d.ReadInt32();
        List<int> order = new(count);
        for (int i = 0; i < count; i++) order.Add(d.ReadInt32());
        return order;
    }

    public static List<long> ReadRandomUID(this IGmDataDeserializer d, GMSChunk.GEN8 chunk, GameMakerFile gmFile) {
        int seed = (int) (chunk.Timestamp & 0xFFFFFFFF);
        int numerator = (int) chunk.Timestamp & 0xFFFF;
        int quotient = numerator / 7;
        int infoLocation = Math.Abs(quotient + (chunk.GameID - chunk.DefaultWindowWidth) + chunk.RoomOrder.Count) % 4;

        List<long> randomUid = new();
        var rand = new Random(seed);

        long firstRand = (long) rand.Next() << 32 | (long) rand.Next();
        if (d.ReadInt64() != firstRand) throw new Exception(); // TODO

        for (int i = 0; i < 4; i++) {
            if (infoLocation == i) {
                long curr = d.ReadInt64();
                randomUid.Add(curr);

                if (curr != GetInfoNumber(firstRand, false, chunk)) {
                    if (curr != GetInfoNumber(firstRand, true, chunk))
                        throw new Exception(); // TODO
                    else
                        gmFile.VersionInfo.RunFromIDE = true;
                }
            }
            else {
                int first = d.ReadInt32();
                int second = d.ReadInt32();
                if (first != rand.Next() || second != rand.Next()) throw new Exception(); // TODO
                randomUid.Add((long) (first << 32) | (long) second); // TODO: shifting by 32 equates to 0...
            }
        }

        return randomUid;
    }
}