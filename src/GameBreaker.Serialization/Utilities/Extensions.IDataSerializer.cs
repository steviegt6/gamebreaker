// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System;
using GameBreaker.Serialization.Abstractions;

namespace GameBreaker.Serialization.Utilities;

partial class Extensions
{
    /*// T/O/D/O/: More WriteAt overloads?
    public static void WriteAt(this IDataSerializer serializer, long pos, int value) {
        long origPos = serializer.Position;
        serializer.Position = pos;
        serializer.Write(value);
        serializer.Position = origPos;
    }*/

    /// <summary>
    ///     Writes four dummy bytes at the <paramref name="serializer"/>'s current position and returns the position. <br />
    ///     Used for writing the size of an arbitrary chunk of data alongside <see cref="EndLength"/>.
    /// </summary>
    /// <param name="serializer">The <see cref="IDataSerializer"/> to use.</param>
    /// <returns>The <see cref="serializer"/>'s position after jumping four bytes.</returns>
    /// <remarks>Used in conjunction with <see cref="EndLength"/>.</remarks>
    /// <seealso cref="EndLength"/>
    public static uint BeginLength(this IDataSerializer serializer) {
        serializer.Write(0xDEAD6A3E);
        return (uint) serializer.Position;
    }

    /// <summary>
    ///     Writes the length of the data between the <paramref name="begin"/>ning position and the <paramref name="serializer"/>'s current position.
    /// </summary>
    /// <param name="serializer">The <see cref="IDataSerializer"/> to use.</param>
    /// <param name="begin">The beginning length to record from.</param>
    /// <remarks>Used in conjunction with <see cref="BeginLength"/>, see <see cref="BeginLength"/>'s summary for more details.</remarks>
    /// <seealso cref="BeginLength"/>
    public static void EndLength(this IDataSerializer serializer, uint begin) {
        long offset = serializer.Position;
        serializer.Position = begin - 4;

        // TODO: Is the uint32 conversion here safe?
        serializer.Write((uint) (offset - begin));

        serializer.Position = offset;
    }

    /// <summary>
    ///     Boilerplate for <see cref="BeginLength"/> and <see cref="EndLength"/>; records the beginning position with <see cref="BeginLength"/> and writes with <see cref="EndLength"/> appropriately after running the provided <paramref name="action"/>.
    /// </summary>
    /// <param name="serializer">The <see cref="IDataSerializer"/> to use.</param>
    /// <param name="action">The action to run between <see cref="BeginLength"/> and <see cref="EndLength"/>.</param>
    public static void WriteWithLength(this IDataSerializer serializer, Action<IDataSerializer> action) {
        uint begin = serializer.BeginLength();
        action(serializer);
        serializer.EndLength(begin);
    }
}