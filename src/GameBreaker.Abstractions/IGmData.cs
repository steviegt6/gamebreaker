// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;

namespace GameBreaker.Abstractions
{
    public interface IGmData
    {
        GmVersionInfo VersionInfo { get; }

        IRootGmChunk Root { get; }

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

        /// <summary>
        ///     The length of the data file in bytes.
        /// </summary>
        long Length { get; }

        T? GetChunk<T>()
            where T : IGmChunk;

        GmString DefineString(string value, out int index);
    }
}