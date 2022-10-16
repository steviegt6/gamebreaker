// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.Text;

namespace GameBreaker.Abstractions.Serialization
{
    public interface IPositionable
    {
        Encoding Encoding { get; }

        ulong Length { get; set; }

        ulong Position { get; set; }
    }
}