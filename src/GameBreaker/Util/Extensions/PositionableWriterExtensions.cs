// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using GameBreaker.Abstractions.Serialization;

namespace GameBreaker.Util.Extensions
{
    public static class PositionableWriterExtensions
    {
        // TODO: More WriteAt overloads?
        public static void WriteAt(this IPositionableWriter writer, long pos, int value) {
            long origPos = writer.Position;
            writer.Position = pos;
            writer.Write(value);
            writer.Position = origPos;
        }
    }
}