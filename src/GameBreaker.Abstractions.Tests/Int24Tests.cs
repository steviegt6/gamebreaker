// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.Runtime.InteropServices;
using NUnit.Framework.Constraints;

namespace GameBreaker.Abstractions.Tests
{
    [TestFixture]
    public static class Int24Tests
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
        public static void Signed_MemoryMarshalTests(int value, bool expected) {
            EqualConstraint constraint = expected ? Is.EqualTo(value) : Is.Not.EqualTo(value);
            Assert.That(MemoryMarshal.Read<Int24>(GetBytes(value)).Value, constraint);
        }
        
        // Casting here is fine since min/max values are only 3 bytes. Same applies to the .Value cast in the method body.
        [TestCase((int) UInt24.MinValue - 1, false)]
        [TestCase((int) UInt24.MinValue, true)]
        [TestCase(0x0000FF, true)]
        [TestCase(0x00FFFF, true)]
        [TestCase((int) UInt24.MaxValue, true)]
        [TestCase((int) UInt24.MaxValue + 1, false)]
        public static void Unsigned_MemoryMarshalTests(int value, bool expected) {
            EqualConstraint constraint = expected ? Is.EqualTo(value) : Is.Not.EqualTo(value);
            Assert.That((int) MemoryMarshal.Read<UInt24>(GetBytes(value)).Value, constraint);
        }
        
        private static byte[] GetBytes(int value) {
            return new[] {(byte) (value & 0xFF), (byte) ((value >> 8) & 0xFF), (byte) ((value >> 16) & 0xFF)};
        }
    }
}