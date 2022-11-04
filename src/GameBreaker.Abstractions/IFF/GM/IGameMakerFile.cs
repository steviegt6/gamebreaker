// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;

namespace GameBreaker.Abstractions.IFF.GM
{
    public interface IGameMakerFile : IRootedFile
    {
        GmVersionInfo VersionInfo { get; }

        ActionBlock<KeyValuePair<string, byte[]>> FileWrites { get; }

        /// <summary>
        ///     The directory of this data file.
        /// </summary>
        string Directory { get; }

        /// <summary>
        ///     The file name of this data file.
        /// </summary>
        string Filename { get; }

        /// <summary>
        ///     The <see cref="SHA1"/> hash of this data file.
        /// </summary>
        byte[] Hash { get; }

        GmString DefineString(string value, out int index);
    }
}