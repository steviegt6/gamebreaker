// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System;
using GameBreaker.Core.Abstractions;
using GameBreaker.Core.Abstractions.Exceptions;
using GameBreaker.Tests.Utilities;

namespace GameBreaker.Tests.DataTypeTests;

[TestFixture]
public static class GmStringTests
{
    [TestCase("", false)]
    [TestCase("\0", false)]
    [TestCase("Hello, world!", false)]
    [TestCase("Hello, world!\0", false)]
    [TestCase(null, true)]
    public static void InitializationTests(string? value, bool exceptionExpected) {
        try {
            GmString gmString = new(value!);
            Assert.That(gmString.Value, Is.EqualTo(value));
        }
        catch (UninitializedGmStringException e) {
            if (exceptionExpected)
                Assert.Pass();
            else
                Assert.Fail("GmString was initialized with a null value unexpectedly.");
        }
    }

    [TestCase("")]
    [TestCase("\0")]
    [TestCase("Hello, world!")]
    [TestCase("Hello, world!\0")]
    public static void SerializationTests(string value) {
        (
            var ms,
            var serializer,
            var deserializer
        ) = SerializationUtilities.PrepareSerializationTest();

        void InnerTest(Action<GmString> write) {
            ms.Position = 0;

            GmString directString = new(value);
            write(directString);

            ms.Position = 0;

            var readerString = deserializer.ReadGmString();

            ms.Position = 0;

            GmString deserializerString = new();
            deserializerString.Deserialize(deserializer);

            Assert.That(value, Is.EqualTo(directString.Value));
            Assert.That(value, Is.EqualTo(readerString.Value));
            Assert.That(value, Is.EqualTo(deserializerString.Value));
        }

        InnerTest(x => serializer.Write(x));
        InnerTest(x => x.Serialize(serializer));
    }
}