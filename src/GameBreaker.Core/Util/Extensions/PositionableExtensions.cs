// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.
// Copyright (c) colinator27. Licensed under the MIT License.
// See the LICENSE-DogScepter file in the repository root for full terms and conditions.

using GameBreaker.Core.Abstractions.Serialization;

namespace GameBreaker.Core.Util.Extensions;

public static class PositionableExtensions
{
    /// <summary>
    ///     Pads the position of this <paramref name="positionable"/> to the next multiple of <paramref name="alignment"/>.
    /// </summary>
    /// <param name="positionable"></param>
    /// <param name="alignment"></param>
    public static void Pad(this IPositionable positionable, long alignment) {
        if (positionable.Position % alignment != 0) positionable.Position += alignment - (positionable.Position % alignment);
    }
}