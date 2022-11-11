// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System;
using System.Runtime.Serialization;

namespace GameBreaker.Core.Exceptions;

[Serializable]
public class GameMakerDeserializationException : Exception
{
    public GameMakerDeserializationException() { }
    public GameMakerDeserializationException(string message) : base(message) { }
    public GameMakerDeserializationException(string message, Exception inner) : base(message, inner) { }

    protected GameMakerDeserializationException(
        SerializationInfo info,
        StreamingContext context
    ) : base(info, context) { }
}

[Serializable]
public class FormGameMakerDeserializationException : GameMakerDeserializationException
{
    public FormGameMakerDeserializationException() { }
    public FormGameMakerDeserializationException(string message) : base(message) { }
    public FormGameMakerDeserializationException(string message, Exception inner) : base(message, inner) { }

    protected FormGameMakerDeserializationException(
        SerializationInfo info,
        StreamingContext context
    ) : base(info, context) { }
}