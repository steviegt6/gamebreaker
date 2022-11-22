// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.Diagnostics;
using GameBreaker.Serialization.Abstractions;

namespace GameBreaker.IFF.Utilities;

partial class Extensions
{
    public static string ReadGmString(this IDataDeserializer deserializer) {
        Debug.Assert(deserializer.Position + 4 <= deserializer.Length, "ReadGmString: Read out of bounds.");
        int length = deserializer.ReadInt32();
        string res = deserializer.Encoding.GetString(deserializer.ReadBytes(length));
        Debug.Assert(deserializer.ReadByte() == 0, "ReadGmString: String was not null-terminated!");
        return res;
    }
}