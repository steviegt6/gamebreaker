// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.Collections.Generic;
using GameBreaker.Core.Abstractions.Serialization;

namespace GameBreaker.Core.Collections.Abstractions;

/// <summary>
///     A list of <see cref="IGmSerializable"/>s with support for handling serialization.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IGmList<T> : IList<T>, IGmCollection<T>
    where T : IGmSerializable, new() // TODO
{ }

/// <summary>
///     A list of pointers to <see cref="IGmSerializable"/> objects, with support for handling serialization.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IGmPointerList<T> : IGmList<T>, IGmPointerCollection<T>
    where T : IGmSerializable, new()
{ }

/// <summary>
///     A list of pointers to <see cref="IGmSerializable"/> objects, with support for handling serialization. <br />
///     Objects in this list are not adjacent, and thus the position is reset at the end to the position after the final pointer. <br />
///     Writing does not serialize objects.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IGmRemotePointerList<T> : IGmList<T>, IGmRemotePointerCollection<T>
    where T : IGmSerializable, new()
{ }

/// <summary>
///     A list of pointers to <see cref="IGmSerializable"/> objects, with support for handling serialization. <br />
///     <see cref="IGmPointerCollection{T}.UsePointerMap"/> is set to false.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IGmUniquePointerList<T> : IGmPointerList<T>, IGmUniquePointerCollection<T>
    where T : IGmSerializable, new()
{ }