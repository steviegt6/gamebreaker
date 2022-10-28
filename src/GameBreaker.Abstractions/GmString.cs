// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System;
using GameBreaker.Abstractions.Exceptions;
using GameBreaker.Abstractions.Serialization;

namespace GameBreaker.Abstractions
{
    public record GmString : IGmSerializable
    {
        public const string DEFAULT_CTOR_FAILURE_MESSAGE = "GmString value was null, the default constructor should only be used during deserialization.";
        public const string CTOR_FAILURE_MESSAGE = "GmString was initialized as null, use the default constructor if initialization must be deferred.";

        private string? value;

        public string Value => value ?? throw new UninitializedGmStringException(DEFAULT_CTOR_FAILURE_MESSAGE);

        public GmString() {
            value = null;
        }

        public GmString(string value) {
            this.value = value ?? throw new UninitializedGmStringException(CTOR_FAILURE_MESSAGE);
        }

        public void Serialize(IGmDataSerializer serializer) {
            serializer.Writer.Write(this);
        }

        public void Deserialize(IGmDataDeserializer deserializer) {
            value = deserializer.Reader.ReadGmString().Value;
        }

        public override string ToString() {
            return Value;
        }
    }
}