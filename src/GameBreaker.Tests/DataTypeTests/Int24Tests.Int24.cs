// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.IO;
using System.Runtime.InteropServices;
using GameBreaker.Abstractions;
using GameBreaker.Abstractions.Serialization;
using GameBreaker.Tests.Utilities;
using NUnit.Framework.Constraints;

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
        EqualConstraint constraint = expected ? Is.EqualTo(value) : Is.Not.EqualTo(value);
        Assert.That(MemoryMarshal.Read<Int24>(GetBytes(value)).Value, constraint);
    }

    [TestCase(Int24.MinValue)]
    [TestCase(Int24.MaxValue)]
    public static void Int24_SerializationTests(int value) {
        (
            MemoryStream ms,
            IPositionableWriter writer,
            IPositionableReader reader,
            IGmDataSerializer _,
            IGmDataDeserializer _
        ) = SerializationUtilities.PrepareSerializationTest();
        
        Int24 directInt = new(value);
        writer.Write(directInt);

        ms.Position = 0;

        Int24 readerInt = reader.ReadInt24();

        Assert.That(value, Is.EqualTo(directInt.Value));
        Assert.That(value, Is.EqualTo(readerInt.Value));
    }
}