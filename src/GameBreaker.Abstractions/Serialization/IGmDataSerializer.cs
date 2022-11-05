// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using GameBreaker.Abstractions.IFF.GM;

namespace GameBreaker.Abstractions.Serialization
{
    public interface IGmDataSerializer : IPositionableWriter
    {
        IGameMakerFile GameMakerFile { get; }

        void SerializeData();

        void WritePointer(IGmSerializable? ptr);

        void WritePointerString(GmString? ptr);

        void WriteObjectPointer(IGmSerializable ptr);
    }
}