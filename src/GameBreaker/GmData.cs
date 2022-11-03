// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks.Dataflow;
using GameBreaker.Abstractions;
using GameBreaker.Abstractions.Exceptions;

namespace GameBreaker;

public class GmData : IGmData
{
    public virtual GmVersionInfo VersionInfo { get; } = new();

    public virtual IRootGmChunk Root => throw new System.NotImplementedException();
    
    public virtual ActionBlock<KeyValuePair<string, byte[]>> FileWrites { get; }
    
    public virtual string Directory { get; }
    
    public virtual string Filename { get; }
    
    public virtual byte[] Hash { get; }
    
    public virtual long Length { get; }

    public virtual T? GetChunk<T>()
        where T : IGmChunk {
        if (Root.Chunks.TryGetValue(CHUNKS_R[typeof(T)], out IGmChunk? chunk)) return (T) chunk;
        return default;
    }

    public virtual GmString DefineString(string value, out int index) {
        if (value is null) throw new UninitializedGmStringException("Attempted to define GmString with a null value!");
        throw new System.NotImplementedException();
    }
}