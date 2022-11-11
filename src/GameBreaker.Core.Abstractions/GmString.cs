// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using GameBreaker.Core.Abstractions.Exceptions;
using GameBreaker.Core.Abstractions.Serialization;

namespace GameBreaker.Core.Abstractions
{
    /// <summary>
    ///     Encapsulates a <see langword="string"/> that be be serialized/deserialized with a <see cref="IGmDataSerializer"/>,<see cref="IGmDataDeserializer"/>.
    /// </summary>
    public record GmString
    {
        public const string CTOR_FAILURE_MESSAGE = "GmString was initialized as null.";

        private string? value;

        /// <summary>
        ///     The non-nullable value encapsulated by this <see cref="GmString"/>.
        /// </summary>
        /// <exception cref="UninitializedGmStringException"></exception>
        public string Value => value ?? throw new UninitializedGmStringException(CTOR_FAILURE_MESSAGE);
        
        /// <summary>
        ///     Initializes a new instance of the <see cref="GmString"/> class with the given <paramref name="value"/>, which must not be <see langword="null"/>.
        /// </summary>
        /// <param name="value">The <see langword="string"/> value.</param>
        /// <exception cref="UninitializedGmStringException">If the passed <paramref name="value"/> is <see langword="null"/>.</exception>
        public GmString(string value) {
            this.value = value ?? throw new UninitializedGmStringException(CTOR_FAILURE_MESSAGE);
        }

        /// <summary>
        ///     Serializes a the <see cref="Value"/> in the <see cref="GmString"/> format.
        /// </summary>
        /// <param name="serializer"></param>
        public void Serialize(IGmDataSerializer serializer) {
            serializer.Write(this);
        }

        /// <summary>
        ///     Deserializes a <see cref="GmString"/>
        /// </summary>
        /// <param name="deserializer"></param>
        public void Deserialize(IGmDataDeserializer deserializer) {
            value = deserializer.ReadGmString().Value;
        }

        public override string ToString() {
            return Value;
        }
    }
}