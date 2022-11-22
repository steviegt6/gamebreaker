// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System;
using System.Text;

namespace GameBreaker.Serialization.Abstractions
{
    /// <summary>
    ///     Describes a disposable object aware of its <see cref="Position"/>, <see cref="Length"/>, and <see cref="Encoding"/>.
    /// </summary>
    public interface IPositionable : IDisposable
    {
        /// <summary>
        ///     The <see cref="System.Text.Encoding"/> to use when handling strings.
        /// </summary>
        Encoding Encoding { get; }

        /// <summary>
        ///     The length of this positionable.
        /// </summary>
        long Length { get; set; }

        /// <summary>
        ///     The position of this positionable.
        /// </summary>
        long Position { get; set; }
    }
}