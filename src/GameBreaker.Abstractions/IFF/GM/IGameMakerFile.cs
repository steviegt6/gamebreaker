// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;

namespace GameBreaker.Abstractions.IFF.GM
{
    public interface IGameMakerFile : IRootedFile
    {
        GmVersionInfo VersionInfo { get; }

        GmString DefineString(string value, out int index);
    }
}