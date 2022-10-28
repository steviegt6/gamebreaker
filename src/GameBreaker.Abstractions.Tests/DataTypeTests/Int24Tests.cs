// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

namespace GameBreaker.Abstractions.Tests.DataTypeTests
{
    [TestFixture]
    public static partial class Int24Tests
    {
        private static byte[] GetBytes(int value) {
            return new[] {(byte) (value & 0xFF), (byte) ((value >> 8) & 0xFF), (byte) ((value >> 16) & 0xFF)};
        }
    }
}