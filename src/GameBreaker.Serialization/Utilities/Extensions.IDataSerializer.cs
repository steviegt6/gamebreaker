// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System;
using GameBreaker.Serialization.Abstractions;

namespace GameBreaker.Serialization.Utilities;

partial class Extensions
{
    // TODO: More WriteAt overloads?
    public static void WriteAt(this IDataSerializer serializer, long pos, int value) {
        long origPos = serializer.Position;
        serializer.Position = pos;
        serializer.Write(value);
        serializer.Position = origPos;
    }
    
    public static uint BeginLength(this IDataSerializer serializer) {
        serializer.Write(0xDEAD6A3E);
        return (uint) serializer.Position;
    }
    
    public static void EndLength(this IDataSerializer serializer, uint begin) {
        long offset = serializer.Position;
        serializer.Position = begin - 4;

        // TODO: Is the uint32 conversion here safe?
        serializer.Write((uint) (offset - begin));

        serializer.Position = offset;
    }

    public static void WriteWithLength(this IDataSerializer serializer, Action<IDataSerializer> action) {
        uint begin = serializer.BeginLength();
        action(serializer);
        serializer.EndLength(begin);
    }
}