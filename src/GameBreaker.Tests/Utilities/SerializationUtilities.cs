// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.IO;
using GameBreaker.Core.Abstractions.Serialization;
using GameBreaker.Core.Serialization;

namespace GameBreaker.Tests.Utilities;

public static class SerializationUtilities
{
    public static (MemoryStream ms, IGmDataSerializer serializer, IGmDataDeserializer deserializer) PrepareSerializationTest() {
        MemoryStream ms = new();
        IGmDataSerializer serializer = new GmDataSerializer(new GmWriter(ms));
        IGmDataDeserializer deserializer = new GmDataDeserializer(new GmReader(ms));
        return (ms, serializer, deserializer);
    }
}