// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using GameBreaker.Core.Abstractions.IFF;

namespace GameBreaker.Core.Abstractions.Serialization
{
    public interface IGmDataSerializer : IPositionableWriter
    {
        // void WritePointer(IGmSerializable? ptr);

        void WritePointerString(GmString? ptr);

        // void WriteObjectPointer(IGmSerializable ptr);
    }
}