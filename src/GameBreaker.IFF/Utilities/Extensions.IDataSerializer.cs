// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.Diagnostics;
using System.Text;
using GameBreaker.Serialization.Abstractions;

namespace GameBreaker.IFF.Utilities;

partial class Extensions
{
    public static void WriteGmString(this IDataSerializer serializer, string value) {
        byte[] bytes = serializer.Encoding.GetBytes(value);
        serializer.Write(bytes.Length);
        serializer.Write(bytes);
        serializer.Write(0);
    }
}