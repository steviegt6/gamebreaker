/*
 * Copyright (c) 2023 Tomat & GameBreaker Contributors
 * Copyright (c) 2020 colinator27
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

using GameBreaker.Models;

namespace GameBreaker;

/// <summary>
/// A GameMaker resource that can be read and written.
/// </summary>
public interface IGMSerializable
{
    /// <summary>
    /// Serializes this GameMaker resource into a specified <see cref="GmDataWriter"/>.
    /// </summary>
    /// <param name="writer">The <see cref="GmDataWriter"/> from where to serialize to.</param>
    void Serialize(GmDataWriter writer);

    /// <summary>
    /// Deserializes a GameMaker resource from a specified <see cref="GmDataReader"/>.
    /// </summary>
    /// <param name="reader">The <see cref="GmDataReader"/> from where to deserialize from.</param>
    void Deserialize(GmDataReader reader);
}

/// <summary>
/// A GameMaker resource, which contains a name, that can be read and written.
/// </summary>
public interface IGMNamedSerializable : IGMSerializable
{
    /// <summary>
    /// The name of a GameMaker resource.
    /// </summary>
    public GMString Name { get; set; }
}