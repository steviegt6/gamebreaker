/*
 * Copyright (c) 2023 Tomat & GameBreaker Contributors
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System.Collections.Generic;
using GameBreaker.Models;

namespace GameBreaker.Serial;

// TODO: Reconsider mandatory properties, summaries, etc.
/// <summary>
///     An extension to <see cref="IBinaryReader"/> which provides additional
///     methods for reading GameMaker data.
/// </summary>
public interface IDataReader : IBinaryReader {
    /// <summary>
    ///     The <see cref="GMData"/> instance this reader is working with.
    /// </summary>
    GMData Data { get; }

    /// <summary>
    ///     The <see cref="GMData.GMVersionInfo"/> instance this reader is
    ///     working with.
    /// </summary>
    GMData.GMVersionInfo VersionInfo { get; }

    // These properties probably need to get moved or dealt with elsewhere
    // somehow.
    List<GMWarning> Warnings { get; }

    Dictionary<int, IGMSerializable> PointerOffsets { get; }

    Dictionary<int, GMCode.Bytecode.Instruction> Instructions { get; }

    List<(GMTextureData, int)> TexturesToDecompress { get; }

    /// <summary>
    ///     Deserializes the <see cref="Data"/>.
    /// </summary>
    /// <param name="clearData">
    ///     Whether pointers and instructions should be cleared.
    /// </param>
    void Deserialize(bool clearData = true);

    /// <summary>
    ///     Reads a (possibly empty) object of the given type, at the specified
    ///     pointer address.
    /// </summary>
    /// <param name="ptr">The pointer address.</param>
    /// <typeparam name="T">The object type.</typeparam>
    /// <returns>A (possibly empty) instance.</returns>
    T ReadPointer<T>(int ptr) where T : IGMSerializable, new();

    /// <summary>
    ///     Reads a (possibly empty) object of the given type, at the current
    ///     position.
    /// </summary>
    /// <typeparam name="T">The object type.</typeparam>
    /// <returns>A (possibly empty) instance.</returns>
    T ReadPointer<T>() where T : IGMSerializable, new();

    /// <summary>
    ///     Follows the specified pointer address for an object type,
    ///     deserializes it, and returns the deserialized instance.
    /// </summary>
    /// <param name="ptr">The pointer address.</param>
    /// <param name="returnAfter">
    ///     Whether to return to the original reader position after
    ///     deserializing.
    /// </param>
    /// <param name="unique">
    ///     Whether this should be considered unique, meaning it is only read
    ///     once is and not cached.
    /// </param>
    /// <typeparam name="T">The object type.</typeparam>
    /// <returns>The deserialized object instance.</returns>
    T ReadPointerObject<T>(int ptr, bool returnAfter = true, bool unique = false)
        where T : IGMSerializable, new();

    /// <summary>
    ///     Follows the current pointer address for an object type, deserializes
    ///     it, and returns the deserialized instance.
    /// </summary>
    /// <param name="unique">
    ///     Whether this should be considered unique, meaning it is only read
    ///     once is and not cached.
    /// </param>
    /// <typeparam name="T">The object type.</typeparam>
    /// <returns>The deserialized object instance.</returns>
    T ReadPointerObject<T>(bool unique = false)
        where T : IGMSerializable, new();

    /// <summary>
    ///     Reads a <see cref="GMString"/> without parsing it.
    /// </summary>
    /// <returns>The unparsed <see cref="GMString"/>.</returns>
    GMString ReadStringPointer();

    /// <summary>
    ///     Reads a <see cref="GMString"/> and parses it.
    /// </summary>
    /// <returns>The parsed <see cref="GMString"/>.</returns>
    GMString ReadStringPointerObject();

    /// <summary>
    ///     Reads a GameMaker-style string from the buffer.
    /// </summary>
    /// <returns>The read string.</returns>
    string ReadGMString();
}
