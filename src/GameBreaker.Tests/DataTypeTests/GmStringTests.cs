// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System;
using GameBreaker.IFF.Serialization;
using GameBreaker.IFF.Utilities;
using GameBreaker.Tests.Utilities;

namespace GameBreaker.Tests.DataTypeTests;

[TestFixture]
public static class GmStringTests
{
    [TestCase("")]
    [TestCase("\0")]
    [TestCase("Hello, world!")]
    [TestCase("Hello, world!\0")]
    public static void SerializationTests(string value) {
        var (ms, serializer, deserializer) = SerializationUtilities.PrepareSerializationTest();

        SerializableString directString = new() { Value = value };
        directString.Serialize(serializer, null!, null!);

        ms.Position = 0;

        string readerString = deserializer.ReadGmString();
        ms.Position = 0;

        SerializableString deserializerString = new();
        deserializerString.Deserialize(deserializer, null!, null!);

        Assert.That(value, Is.EqualTo(directString.Value));
        Assert.That(value, Is.EqualTo(readerString));
        Assert.That(value, Is.EqualTo(deserializerString.Value));
    }
}