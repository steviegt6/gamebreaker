// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.Runtime.InteropServices;
using GameBreaker.Abstractions;
using GameBreaker.Tests.Utilities;

namespace GameBreaker.Tests.DataTypeTests;

partial class Int24Tests
{
    // Casting here is fine since min/max values are only 3 bytes. Same applies to the .Value cast in the method body.
    [TestCase((int) UInt24.MinValue - 1, false)]
    [TestCase((int) UInt24.MinValue, true)]
    [TestCase(0x0000FF, true)]
    [TestCase(0x00FFFF, true)]
    [TestCase((int) UInt24.MaxValue, true)]
    [TestCase((int) UInt24.MaxValue + 1, false)]
    public static void UInt24_MemoryMarshalTests(int value, bool expected) {
        var constraint = expected ? Is.EqualTo(value) : Is.Not.EqualTo(value);
        Assert.That((int) MemoryMarshal.Read<UInt24>(GetBytes(value)).Value, constraint);
    }
    
    [TestCase((int) UInt24.MinValue)]
    [TestCase((int) UInt24.MaxValue)]
    public static void UInt24_SerializationTests(int value) {
        (
            var ms,
            var serializer,
            var deserializer
        ) = SerializationUtilities.PrepareSerializationTest();
        
        UInt24 directUInt = new((uint) value);
        serializer.Write(directUInt);

        ms.Position = 0;

        var readerUInt = deserializer.ReadUInt24();

        Assert.That(value, Is.EqualTo(directUInt.Value));
        Assert.That(value, Is.EqualTo(readerUInt.Value));
    }
}