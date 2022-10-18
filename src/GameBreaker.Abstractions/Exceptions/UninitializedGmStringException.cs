// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System;
using System.Runtime.Serialization;

namespace GameBreaker.Abstractions.Exceptions
{
    [Serializable]
    public class UninitializedGmStringExceptionException : Exception
    {
        public UninitializedGmStringExceptionException() { }
        public UninitializedGmStringExceptionException(string message) : base(message) { }
        public UninitializedGmStringExceptionException(string message, Exception inner) : base(message, inner) { }

        protected UninitializedGmStringExceptionException(
            SerializationInfo info,
            StreamingContext context
        ) : base(info, context) { }
    }
}