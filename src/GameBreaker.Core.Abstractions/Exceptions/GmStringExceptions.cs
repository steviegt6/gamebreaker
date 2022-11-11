// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System;
using System.Runtime.Serialization;

namespace GameBreaker.Core.Abstractions.Exceptions
{
    [Serializable]
    public class UninitializedGmStringException : Exception
    {
        public UninitializedGmStringException() { }
        public UninitializedGmStringException(string message) : base(message) { }
        public UninitializedGmStringException(string message, Exception inner) : base(message, inner) { }

        protected UninitializedGmStringException(
            SerializationInfo info,
            StreamingContext context
        ) : base(info, context) { }
    }
}