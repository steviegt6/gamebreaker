// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System;
using System.Text;

namespace GameBreaker.Abstractions.Serialization
{
    public interface IPositionable : IDisposable
    {
        Encoding Encoding { get; }

        long Length { get; set; }

        long Position { get; set; }
    }
}