// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.IO;
using GameBreaker.Serialization;
using GameBreaker.Serialization.Abstractions;

namespace GameBreaker.Tests.Utilities;

public static class SerializationUtilities
{
    public static (MemoryStream ms, IDataSerializer serializer, IDataDeserializer deserializer) PrepareSerializationTest() {
        MemoryStream ms = new();
        IDataSerializer serializer = new DataSerializer(ms);
        IDataDeserializer deserializer = new DataDeserializer(ms);
        return (ms, serializer, deserializer);
    }
}