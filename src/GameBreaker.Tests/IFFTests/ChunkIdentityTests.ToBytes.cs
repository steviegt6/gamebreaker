// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using GameBreaker.Abstractions.IFF;

namespace GameBreaker.Tests.IFFTests;

partial class ChunkIdentityTests
{
    // TODO: Probably doesn't need many tests, but I don't know. Constructor tests are in charge of testing actual input validity...
    [TestCase(new byte[] { 0x46, 0x4F, 0x52, 0x4D }, "FORM")]
    [TestCase(new byte[] { 0x46, 0x00, 0x00, 0x00 }, "F\0\0\0")]
    public static void ToBytes_ReturnTests(byte[] utf8Bytes, string expectedReturn) {
        Assert.That(new ChunkIdentity(utf8Bytes).ToBytes(), Is.EqualTo(expectedReturn));
    }
}