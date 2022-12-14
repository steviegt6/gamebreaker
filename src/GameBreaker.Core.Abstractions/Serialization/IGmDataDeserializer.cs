// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using GameBreaker.Core.Abstractions.IFF;

namespace GameBreaker.Core.Abstractions.Serialization
{
    public interface IGmDataDeserializer : IPositionableReader
    {
        IRootedFile? GameMakerFile { get; set; }

        T ReadPointer<T>(int ptr)
            where T : IGmSerializable, new();

        T ReadPointer<T>()
            where T : IGmSerializable, new();

        T ReadPointerObject<T>(int ptr, bool returnAfter = true)
            where T : IGmSerializable, new();
        
        T ReadPointerObject<T>()
            where T : IGmSerializable, new();

        T ReadPointerObjectUnique<T>(int ptr, bool returnAfter = true)
            where T : IGmSerializable, new();

        T ReadPointerObjectUnique<T>()
            where T : IGmSerializable, new();

        GmString ReadStringPointer();

        GmString ReadStringPointerObject();
    }
}