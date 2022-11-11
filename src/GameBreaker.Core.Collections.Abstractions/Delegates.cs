// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.
// Copyright (c) colinator27. Licensed under the MIT License.
// See the LICENSE-DogScepter file in the repository root for full terms and conditions.

using GameBreaker.Core.Abstractions.Serialization;

namespace GameBreaker.Core.Collections.Abstractions;

public delegate void SerializeCollectionDelegate(IGmDataSerializer serializer, int index, int count);

public delegate void DeserializeCollectionDelegate(IGmDataDeserializer deserializer, int index, int count);

public delegate void SerializeCollectionElementDelegate(IGmDataSerializer serializer, IGmSerializable element);

public delegate IGmSerializable DeserializeCollectionElementDelegate(IGmDataDeserializer deserializer, bool notLast);