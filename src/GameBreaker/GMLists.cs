﻿/*
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

using System.Collections.Generic;

namespace GameBreaker;

// Callbacks for before/after serializing each element, for padding/etc.
public delegate void ListSerialize(GmDataWriter writer, int index, int count);
public delegate void ListDeserialize(GmDataReader reader, int index, int count);

// Callbacks for reading/writing each element, in special scenarios
public delegate void ListSerializeElement(GmDataWriter writer, IGMSerializable elem);
public delegate IGMSerializable ListDeserializeElement(GmDataReader reader, bool notLast);

/// <summary>
/// Basic array-like list type in a GameMaker data file.
/// </summary>
public class GMList<T> : List<T>, IGMSerializable where T : IGMSerializable, new()
{
    /// <summary>
    /// Initializes an empty <see cref="GMList{T}"/>.
    /// </summary>
    public GMList()
    {
    }

    /// <summary>
    /// Initializes an empty <see cref="GMList{T}"/> with a specified capacity.
    /// </summary>
    /// <param name="capacity">How many elements this <see cref="GMList{T}"/> should be able to hold.</param>
    public GMList(int capacity) : base(capacity)
    {
    }

    public virtual void Serialize(GmDataWriter writer, ListSerialize before = null,
        ListSerialize after = null,
        ListSerializeElement elemWriter = null)
    {
        writer.Write(Count);
        for (int i = 0; i < Count; i++)
        {
            before?.Invoke(writer, i, Count);

            // Write the current element in the list
            if (elemWriter == null)
                this[i].Serialize(writer);
            else
                elemWriter(writer, this[i]);

            after?.Invoke(writer, i, Count);
        }
    }

    public virtual void Serialize(GmDataWriter writer)
    {
        Serialize(writer, null, null, null);
    }

    public virtual void Deserialize(GmDataReader reader, ListDeserialize before = null,
        ListDeserialize after = null,
        ListDeserializeElement elemReader = null)
    {
        // Read the element count and begin reading elements
        int count = reader.ReadInt32();
        Capacity = count;
        for (int i = 0; i < count; i++)
        {
            before?.Invoke(reader, i, count);

            // Read the current element and add it to the list
            T elem;
            if (elemReader == null)
            {
                elem = new T();
                elem.Deserialize(reader);
            }
            else
                elem = (T)elemReader(reader, (i + 1 != count));
            Add(elem);

            after?.Invoke(reader, i, count);
        }
    }

    public virtual void Deserialize(GmDataReader reader)
    {
        Deserialize(reader, null, null, null);
    }
}

/// <summary>
/// A list of pointers to objects, forming a list, in a GameMaker data file.
/// </summary>
public class GMPointerList<T> : GMList<T> where T : IGMSerializable, new()
{
    /// <summary>
    /// TODO
    /// </summary>
    public bool UsePointerMap = true;

    /// <summary>
    /// Initializes an empty <see cref="GMPointerList{T}"/>.
    /// </summary>
    public GMPointerList()
    {
    }

    /// <summary>
    /// Initializes an empty <see cref="GMPointerList{T}"/> with a specified capacity.
    /// </summary>
    /// <param name="capacity">How many elements this <see cref="GMPointerList{T}"/> should be able to hold.</param>
    public GMPointerList(int capacity) : base(capacity)
    {
    }

    public void Serialize(GmDataWriter writer, ListSerialize before = null,
        ListSerialize after = null,
        ListSerializeElement elemWriter = null,
        ListSerializeElement elemPointerWriter = null)
    {
        writer.Write(Count);

        // Write each element's pointer
        for (int i = 0; i < Count; i++)
        {
            if (elemPointerWriter == null)
                writer.WritePointer(this[i]);
            else
                elemPointerWriter(writer, this[i]);
        }

        // Write each element
        for (int i = 0; i < Count; i++)
        {
            before?.Invoke(writer, i, Count);

            // Write the current element in the list
            if (elemWriter == null)
            {
                writer.WriteObjectPointer(this[i]);
                this[i].Serialize(writer);
            }
            else
                elemWriter(writer, this[i]);

            after?.Invoke(writer, i, Count);
        }
    }

    public override void Serialize(GmDataWriter writer, ListSerialize before = null,
        ListSerialize after = null,
        ListSerializeElement elemWriter = null)
    {
        Serialize(writer, before, after, elemWriter, null);
    }

    public override void Serialize(GmDataWriter writer)
    {
        Serialize(writer, null, null, null);
    }

    private static IGMSerializable DoReadPointerObject(GmDataReader reader, bool notLast)
    {
        return reader.ReadPointerObject<T>(reader.ReadInt32(), notLast);
    }

    private static IGMSerializable DoReadPointerObjectUnique(GmDataReader reader, bool notLast)
    {
        return reader.ReadPointerObject<T>(reader.ReadInt32(), notLast, unique: true);
    }

    public override void Deserialize(GmDataReader reader, ListDeserialize before = null,
        ListDeserialize after = null,
        ListDeserializeElement elemReader = null)
    {
        // Define a default pointer reader if none is set
        if (elemReader == null)
        {
            if (UsePointerMap)
                elemReader = DoReadPointerObject;
            else
                elemReader = DoReadPointerObjectUnique;
        }

        // Read the element count and begin reading elements
        int count = reader.ReadInt32();
        Capacity = count;
        for (int i = 0; i < count; i++)
        {
            before?.Invoke(reader, i, count);

            // Read the current element and add it to the list
            Add((T)elemReader(reader, i + 1 != count));

            after?.Invoke(reader, i, count);
        }
    }

    public override void Deserialize(GmDataReader reader)
    {
        Deserialize(reader, null, null, null);
    }
}

/// <summary>
/// A list of pointers to objects, forming a list, in a GameMaker data file. <br/>
/// The difference to the normal <see cref="GMPointerList{T}"/> is, that this one's objects are
/// specifically specified to not be adjacent, therefore the offset is reset at the end
/// to the offset after the final pointer. Also, writing does not serialize actual objects.
/// </summary>
public class GMRemotePointerList<T> : GMList<T> where T : IGMSerializable, new()
{

    public void Serialize(GmDataWriter writer, ListSerialize before = null,
        ListSerialize after = null,
        ListSerializeElement elemWriter = null,
        ListSerializeElement elemPointerWriter = null)
    {
        writer.Write(Count);

        // Write each element's pointer
        for (int i = 0; i < Count; i++)
        {
            if (elemPointerWriter == null)
                writer.WritePointer(this[i]);
            else
                elemPointerWriter(writer, this[i]);
        }
    }

    public override void Serialize(GmDataWriter writer, ListSerialize before = null,
        ListSerialize after = null,
        ListSerializeElement elemWriter = null)
    {
        Serialize(writer, before, after, elemWriter, null);
    }

    public override void Serialize(GmDataWriter writer)
    {
        Serialize(writer, null, null, null);
    }

    public override void Deserialize(GmDataReader reader, ListDeserialize before = null,
        ListDeserialize after = null,
        ListDeserializeElement elemReader = null)
    {
        // Read the element count and begin reading elements
        int count = reader.ReadInt32();
        for (int i = 0; i < count; i++)
        {
            before?.Invoke(reader, i, Count);

            // Read the current element and add it to the list
            T elem;
            if (elemReader == null)
            {
                elem = reader.ReadPointerObject<T>(reader.ReadInt32(), true);
            }
            else
            {
                elem = (T)elemReader(reader, true);
            }
            Add(elem);

            after?.Invoke(reader, i, Count);
        }
    }

    public override void Deserialize(GmDataReader reader)
    {
        Deserialize(reader, null, null, null);
    }
}

/// <summary>
/// A list of pointers to objects, forming a list, in a GameMaker data file. <br/>
/// This variant automatically sets <see cref="GMPointerList{T}.UsePointerMap"/> in the base class to false.
/// </summary>
public class GMUniquePointerList<T> : GMPointerList<T> where T : IGMSerializable, new()
{
    /// <summary>
    /// Initializes an empty <see cref="GMUniquePointerList{T}"/>.
    /// </summary>
    public GMUniquePointerList()
    {
        UsePointerMap = false;
    }

    /// <summary>
    /// Initializes an empty <see cref="GMUniquePointerList{T}"/> with a specified capacity.
    /// </summary>
    /// <param name="capacity">How many elements this <see cref="GMUniquePointerList{T}"/> should be able to hold.</param>
    public GMUniquePointerList(int capacity) : base(capacity)
    {
        UsePointerMap = false;
    }
}