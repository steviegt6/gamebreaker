// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

namespace GameBreaker.Core.Abstractions.Serialization
{
    public interface IGmSerializable
    {
        void Serialize(IGmDataSerializer serializer);

        void Deserialize(IGmDataDeserializer deserializer);
    }
}