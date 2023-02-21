using System;
using System.Collections.Generic;
using System.Diagnostics;
using GameBreaker.Serial.IO;
using GameBreaker.Serial.IO.IFF;

namespace GameBreaker.Serial.Chunks;

public class SerializableGen8InfoFlags : Serializable<Gen8InfoFlags> {
    public override void Serialize(IWriter writer) {
        writer.Write((uint) Value);
    }

    public override void Deserialize(IReader reader) {
        Value = (Gen8InfoFlags) reader.ReadUInt32();
    }
}

public class SerializableGen8FunctionClassification :
    Serializable<Gen8FunctionClassification> {
    public override void Serialize(IWriter writer) {
        writer.Write((ulong) Value);
    }

    public override void Deserialize(IReader reader) {
        Value = (Gen8FunctionClassification) reader.ReadUInt64();
    }
}

public class SerializableGen8RoomData : Serializable<Gen8RoomData> {
    public override void Serialize(IWriter writer) {
        Debug.Assert(Value is not null);
        writer.Write(Value.RoomOrder.Count);
        foreach (var room in Value.RoomOrder)
            writer.Write(room);
    }

    public override void Deserialize(IReader reader) {
        var count  = reader.ReadInt32();
        Value = new Gen8RoomData {
            RoomOrder = new List<int>(count),
        };
        for (var i = 0; i < count; i++)
            Value.RoomOrder.Add(reader.ReadInt32());
    }
}

public class SerializableGen8RandomUid : Serializable<Gen8RandomUid> {
    [Obsolete("Use Serialize(IWriter,Gen8ChunkData,IffFile)", error: true)]
    public override void Serialize(IWriter writer) {
        throw new InvalidOperationException(
            "Cannot serialize random UID without additional context."
        );
    }

    public void Serialize(
        IWriter writer,
        Gen8ChunkData chunk,
        IffFile iffFile
    ) {
        Debug.Assert(Value is not null);

        // TODO: Option to write random UID as is instead of recalculating it?

        Value.RandomUid.Clear();

        var seed = (int) (chunk.Timestamp.Value & 0xFFFFFFFF);
        var numerator = (int) chunk.Timestamp.Value & 0xFFFF;
        var quotient = numerator / 7;
        var infoLocation =
            Math.Abs(quotient
                   + (chunk.GameId.Value - chunk.DefaultWindowWidth.Value)
                   + chunk.RoomData.Value!.RoomOrder.Count)
          % 4;

        var rand = new Random(seed);
        var firstRand = (long) rand.Next() << 32 | (long) rand.Next();
        var infoNumber = GetInfoNumber(
            firstRand,
            iffFile.Metadata.RunFromIde,
            chunk
        );

        writer.Write(firstRand);
        Value.RandomUid.Add(firstRand);

        for (var i = 0; i < 4; i++) {
            if (infoLocation == i) {
                writer.Write(infoNumber);
                Value.RandomUid.Add(infoNumber);
            }
            else {
                var first = rand.Next();
                var second = rand.Next();
                writer.Write(first);
                writer.Write(second);
                Value.RandomUid.Add(((long) first << 32) | (long) second);
            }
        }
    }

    [Obsolete("Use Deserialize(IReader,Gen8ChunkData,IffFile)", error: true)]
    public override void Deserialize(IReader reader) {
        throw new InvalidOperationException(
            "Cannot deserialize random UID without additional context."
        );
    }

    public void Deserialize(
        IReader reader,
        Gen8ChunkData chunk,
        IffFile iffFile
    ) {
        var seed = (int) (chunk.Timestamp.Value & 0xFFFFFFFFL);
        var numerator = (int) chunk.Timestamp.Value & 0xFFFF;
        var quotient = numerator / 7;
        var infoLocation =
            Math.Abs(
                quotient
              + (chunk.GameId.Value - chunk.DefaultWindowWidth.Value)
              + chunk.RoomData.Value!.RoomOrder.Count
            )
          % 4;

        var randomUid = new List<long>();
        var rand = new Random(seed);

        var firstRand = (long) rand.Next() << 32 | (long) rand.Next();
        if (reader.ReadInt64() != firstRand)
            throw new Exception(); // TODO

        for (var i = 0; i < 4; i++) {
            if (infoLocation == i) {
                var curr = reader.ReadInt64();
                randomUid.Add(curr);

                if (curr == GetInfoNumber(firstRand, false, chunk))
                    continue;

                if (curr != GetInfoNumber(firstRand, true, chunk))
                    throw new Exception(); // TODO

                iffFile.Metadata.RunFromIde = true;
            }
            else {
                var first = reader.ReadInt32();
                var second = reader.ReadInt32();
                if (first != rand.Next())
                    throw new Exception(); // tODO
                if (second != rand.Next())
                    throw new Exception(); // TODO

                // TODO: Shifting by 32 seems to be a no-op?
                // ReSharper disable once ShiftExpressionRealShiftCountIsZero
                randomUid.Add((long) (first << 32) | (long) second);
            }
        }

        Value = new Gen8RandomUid {
            RandomUid = randomUid,
        };
    }

    private static long GetInfoNumber(
        long firstRandom,
        bool runFromIde,
        Gen8ChunkData c
    ) {
        var infoNumber = c.Timestamp.Value;
        if (!runFromIde)
            infoNumber -= 1000;
        var temp = (ulong) infoNumber;
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
        infoNumber ^= (long) c.GameId.Value << 32 | (long) c.GameId.Value;
        infoNumber ^= (
            (long) (c.DefaultWindowWidth.Value  + (int) c.Info.Value) << 48
          | (long) (c.DefaultWindowHeight.Value + (int) c.Info.Value) << 32
          | (long) (c.DefaultWindowHeight.Value + (int) c.Info.Value) << 16
          | (long) (c.DefaultWindowWidth.Value  + (int) c.Info.Value)
        );
        return infoNumber ^ c.FormatId.Value;
    }
}
