// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.Text;
using GameBreaker.Core.Abstractions.Exceptions;

namespace GameBreaker.Core.Abstractions.IFF;

/// <summary>
///     A read-only wrapper around a <see cref="string"/> with constructor checks to ensure that the <see cref="Value"/> is only ever four bytes.
/// </summary>
public readonly record struct ChunkIdentity
{
    /// <summary>
    ///     The <see cref="Encoding"/> of chunk identities - <see cref="Encoding.UTF8"/>.
    /// </summary>
    public static readonly Encoding IDENTITY_ENCODING = Encoding.UTF8;

    /// <summary>
    ///     The byte length of a chunk identity - 4.
    /// </summary>
    public const int IDENTITY_LENGTH = 4;

    public const string INVALID_STRING_LENGTH = "Chunk identities must be four bytes long, but instead got \"{0}\" (\"{1}\" bytes), is this encoded in UTF8?";
    public const string INVALID_BYTE_COUNT = "Chunk identities must be four bytes, but you provided {0} bytes.";
    public const string INVALID_RETURN_COUNT = "Expected to return {0} bytes, but somehow returned {1} bytes.";

    /// <summary>
    ///     The four-byte header as a <see cref="string"/>.
    /// </summary>
    public string Value { get; }

    public ChunkIdentity(string value) {
        int length = IDENTITY_ENCODING.GetByteCount(value);
        if (length != IDENTITY_LENGTH) throw new InvalidChunkIdentityLengthException(string.Format(INVALID_STRING_LENGTH, value, length));
        Value = value;
    }

    public ChunkIdentity(params byte[] bytes) {
        if (bytes.Length != IDENTITY_LENGTH) throw new InvalidChunkIdentityLengthException(string.Format(INVALID_BYTE_COUNT, bytes.Length));
        Value = IDENTITY_ENCODING.GetString(bytes);
    }

    public byte[] ToBytes() {
        byte[] bytes = new byte[IDENTITY_LENGTH];
        int length = IDENTITY_ENCODING.GetBytes(Value, bytes);
        // This is an extra safety check that probably doesn't need to exist, but *eh*.
        if (length != IDENTITY_LENGTH) throw new InvalidChunkIdentityLengthException(string.Format(INVALID_RETURN_COUNT, IDENTITY_LENGTH, length));
        return bytes;
    }

    public static implicit operator ChunkIdentity(string value) => new(value);
    public static implicit operator ChunkIdentity(byte[] bytes) => new(bytes);
    public static implicit operator string(ChunkIdentity identity) => identity.Value;
    public static implicit operator byte[](ChunkIdentity identity) => identity.ToBytes();
}