// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.Runtime.InteropServices;
using GameBreaker.Core.Abstractions;
using GameBreaker.Tests.Utilities;

namespace GameBreaker.Tests.DataTypeTests;

partial class Int24Tests
{
    [TestCase(Int24.MinValue - 1, false)]
    [TestCase(Int24.MinValue, true)]
    [TestCase(-0x00FFFF, true)]
    [TestCase(-0x0000FF, true)]
    [TestCase(0x000000, true)]
    [TestCase(0x0000FF, true)]
    [TestCase(0x00FFFF, true)]
    [TestCase(Int24.MaxValue, true)]
    [TestCase(Int24.MaxValue + 1, false)]
    [TestCase(0xFFFFFF, false)]
    public static void Int24_MemoryMarshalTests(int value, bool expected) {
        var constraint = expected ? Is.EqualTo(value) : Is.Not.EqualTo(value);
        Assert.That(MemoryMarshal.Read<Int24>(GetBytes(value)).Value, constraint);
    }

    [TestCase(Int24.MinValue)]
    [TestCase(Int24.MaxValue)]
    public static void Int24_SerializationTests(int value) {
        (
            var ms,
            var serializer,
            var deserializer
        ) = SerializationUtilities.PrepareSerializationTest();
        
        Int24 directInt = new(value);
        serializer.Write(directInt);

        ms.Position = 0;

        var readerInt = deserializer.ReadInt24();

        Assert.That(value, Is.EqualTo(directInt.Value));
        Assert.That(value, Is.EqualTo(readerInt.Value));
    }
}