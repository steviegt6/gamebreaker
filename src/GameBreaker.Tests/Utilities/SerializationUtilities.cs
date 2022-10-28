// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.IO;
using GameBreaker.Abstractions.Serialization;
using GameBreaker.Serialization;

namespace GameBreaker.Tests.Utilities;

public static class SerializationUtilities
{
    public static (
        MemoryStream ms,
        IPositionableWriter writer,
        IPositionableReader reader,
        IGmDataSerializer serializer,
        IGmDataDeserializer deserializer)
        PrepareSerializationTest() {
        MemoryStream ms = new();
        IPositionableWriter writer = new GmWriter(ms);
        IPositionableReader reader = new GmReader(ms);
        IGmDataSerializer serializer = new GmDataSerializer(writer);
        IGmDataDeserializer deserializer = new GmDataDeserializer(reader);

        return (ms, writer, reader, serializer, deserializer);
    }
}