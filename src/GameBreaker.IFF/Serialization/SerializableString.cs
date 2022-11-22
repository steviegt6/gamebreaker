// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System;
using GameBreaker.IFF.Abstractions.Serialization;
using GameBreaker.IFF.Utilities;

namespace GameBreaker.IFF.Serialization;

public class SerializableString : ISerializableData<string>
{
    public SerializeDelegate<string> Serialize =>
        (s, _, _) =>
        {
            if (Value is null) throw new Exception(); // TODO
            s.WriteGmString(Value);
        };

    public DeserializeDelegate<string> Deserialize => (d, _, _) => { Value = d.ReadGmString(); };

    public string? Value { get; set; }
}