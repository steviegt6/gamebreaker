// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.Collections.Generic;

namespace GameBreaker.Abstractions;

public interface IRootGmChunk : IGmChunk
{
    Dictionary<string, IGmChunk> Chunks { get; }
}