// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System;
using GameBreaker.Abstractions.Exceptions;
using GameBreaker.Abstractions.Serialization;

namespace GameBreaker.Abstractions
{
    public record GmString : IGmSerializable
    {
        public const string INIT_FAILURE_MESSAGE = "GmString value was null, default constructor should only be used during deserialization.";

        private string? value;

        public string Value => value ?? throw new UninitializedGmStringExceptionException(INIT_FAILURE_MESSAGE);

        public GmString() {
            value = null;
        }

        public GmString(string value) {
            this.value = value ?? throw new NullReferenceException("Tried to assign null value to GmString.");
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