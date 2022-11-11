// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using GameBreaker.Chunks;

namespace GameBreaker.Util;

public static partial class Extensions
{
    private static long GetInfoNumber(long firstRandom, bool runFromIDE, GMSChunk.GEN8 chunk) {
        long infoNumber = chunk.Timestamp;
        if (!runFromIDE) infoNumber -= 1000;
        ulong temp = (ulong) infoNumber;
        infoNumber = (long) (
              (temp << 56 & 0xFF00000000000000)
            | (temp >> 8  & 0xFF000000000000)
            | (temp << 32 & 0xFF0000000000)
            | (temp >> 16 & 0xFF00000000)
            | (temp << 8  & 0xFF000000)
            | (temp >> 24 & 0xFF0000)
            | (temp >> 16 & 0xFF00)
            | (temp >> 32 & 0xFF)
        );
        infoNumber ^= firstRandom;
        infoNumber = ~infoNumber;
        infoNumber ^= (long) chunk.GameID << 32 | (long) chunk.GameID;
        infoNumber ^= (
              (long) (chunk.DefaultWindowWidth  + (int) chunk.Info) << 48
            | (long) (chunk.DefaultWindowHeight + (int) chunk.Info) << 32
            | (long) (chunk.DefaultWindowHeight + (int) chunk.Info) << 16
            | (long) (chunk.DefaultWindowWidth  + (int) chunk.Info)
        );
        infoNumber ^= chunk.FormatID;
        return infoNumber;
    }
}