// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

namespace GameBreaker.Abstractions.Serialization
{
    public interface IGmDataSerializer
    {
        IGmData Data { get; }

        IPositionableWriter Writer { get; }

        void SerializeData();

        void WritePointer(IGmSerializable? ptr);

        void WritePointerString(GmString? ptr);

        void WriteObjectPointer(IGmSerializable ptr);
    }
}