// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using GameBreaker.IFF.Abstractions;
using GameBreaker.IFF.Exceptions;

namespace GameBreaker.Tests.IFFTests;

partial class ChunkIdentityTests
{
    // TODO: Needs more tests, such as for UTF16 characters.
    [TestCase(new byte[] { 0x46, 0x4F, 0x52, 0x4D }, "FORM", false)]
    [TestCase(new byte[] { 0x46, 0x00, 0x00, 0x00 }, "F\0\0\0", false)]
    [TestCase(new byte[] { 0x46 }, "F", true)]
    [TestCase(new byte[] { 0x00 }, "\0", true)]
    [TestCase(new byte[] { 0x46, 0x4F, 0x52, 0x4D, 0x46, 0x4F, 0x52, 0x4D }, "FORMFORM", true)]
    public static void Constructor_StringTests(byte[] utf8Bytes, string utf16Chars, bool expectedException) {
        try {
            byte[] utf16Bytes = new byte[ChunkIdentity.IDENTITY_ENCODING.GetByteCount(utf16Chars)];
            ChunkIdentity.IDENTITY_ENCODING.GetBytes(utf16Chars, utf16Bytes);
            
            ChunkIdentity id = new(utf16Chars);
            
            Assert.Multiple(() =>
            {
                Assert.That(utf16Bytes, Is.EqualTo(utf8Bytes));
                Assert.That(ChunkIdentity.IDENTITY_ENCODING.GetString(utf8Bytes), Is.EqualTo(utf16Chars));
                Assert.That(ChunkIdentity.IDENTITY_ENCODING.GetString(utf16Bytes), Is.EqualTo(utf16Chars));
                Assert.That(ChunkIdentity.IDENTITY_ENCODING.GetString(utf8Bytes), Is.EqualTo(id.Value));
                Assert.That(ChunkIdentity.IDENTITY_ENCODING.GetString(utf16Bytes), Is.EqualTo(id.Value));
            });
        }
        catch (InvalidChunkIdentityLengthException e) {
            if (!expectedException) throw;
        }
    }
    
    // TODO: Needs more tests, such as for UTF16 characters.
    [TestCase(new byte[] { 0x46, 0x4F, 0x52, 0x4D }, "FORM", false)]
    [TestCase(new byte[] { 0x46, 0x00, 0x00, 0x00 }, "F\0\0\0", false)]
    [TestCase(new byte[] { 0x46 }, "F", true)]
    [TestCase(new byte[] { 0x00 }, "\0", true)]
    [TestCase(new byte[] { 0x46, 0x4F, 0x52, 0x4D, 0x46, 0x4F, 0x52, 0x4D }, "FORMFORM", true)]
    public static void Constructor_ByteArrayTests(byte[] utf8Bytes, string utf16Chars, bool expectedException) {
        try {
            byte[] utf16Bytes = new byte[ChunkIdentity.IDENTITY_ENCODING.GetByteCount(utf16Chars)];
            ChunkIdentity.IDENTITY_ENCODING.GetBytes(utf16Chars, utf16Bytes);

            Assert.Multiple(() =>
            {
                Assert.That(utf16Bytes, Is.EqualTo(utf8Bytes));
                Assert.That(ChunkIdentity.IDENTITY_ENCODING.GetString(utf8Bytes), Is.EqualTo(utf16Chars));
                Assert.That(ChunkIdentity.IDENTITY_ENCODING.GetString(utf16Bytes), Is.EqualTo(utf16Chars));
            });

            _ = new ChunkIdentity(utf8Bytes);
        }
        catch (InvalidChunkIdentityLengthException e) {
            if (!expectedException) throw;
        }
    }
}