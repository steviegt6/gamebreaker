// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.
// Copyright (c) colinator27. Licensed under the MIT License.
// See the LICENSE-DogScepter file in the repository root for full terms and conditions.

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

        /// <summary>
        ///     Writes four bytes of dummy data to the <paramref name="writer"/>'s current position and returns the new position. <br />
        ///     Designed to be used in conjunction with <see cref="EndLength"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public static uint BeginLength(this IPositionableWriter writer) {
            writer.Write(DEADGAME);
            return (uint) writer.Position;
        }

        /// <summary>
        ///     Writes the length of a block (start position is <paramref name="begin"/>, end position is the current position of the <paramref name="writer"/> to the location where the dummy data from <see cref="BeginLength"/> was written to. <br />
        ///     Designed to be used in conjunction with <see cref="BeginLength"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="begin"></param>
        public static void EndLength(this IPositionableWriter writer, uint begin) {
            long offset = writer.Position;
            writer.Position = begin - 4;

            // TODO: Is the uint32 conversion here safe?
            writer.Write((uint) (offset - begin));

            writer.Position = offset;
        }
    }
}