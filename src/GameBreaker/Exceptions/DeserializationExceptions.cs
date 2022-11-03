// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System;
using System.Runtime.Serialization;

namespace GameBreaker.Exceptions;

[Serializable]
public class GmDeserializationException : Exception
{
    public GmDeserializationException() { }
    public GmDeserializationException(string message) : base(message) { }
    public GmDeserializationException(string message, Exception inner) : base(message, inner) { }

    protected GmDeserializationException(
        SerializationInfo info,
        StreamingContext context
    ) : base(info, context) { }
}

[Serializable]
public class FormGmDeserializationException : GmDeserializationException
{
    public FormGmDeserializationException() { }
    public FormGmDeserializationException(string message) : base(message) { }
    public FormGmDeserializationException(string message, Exception inner) : base(message, inner) { }

    protected FormGmDeserializationException(
        SerializationInfo info,
        StreamingContext context
    ) : base(info, context) { }
}