﻿// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.
// Copyright (c) colinator27. Licensed under the MIT License.
// See the LICENSE-DogScepter file in the repository root for full terms and conditions.

using System.Diagnostics;

namespace GameBreaker.Serialization
{
    public class GmReader : BufferedReader
    {
        public override bool ReadBoolean() {
            Debug.Assert(Position + 1 <= Length, "ReadBoolean: Read out of bounds.");
            int val = ReadInt32();
            Debug.Assert(val is 0 or 1, $"ReadBoolean: Value was not 0 or 1 ({val}).");
            return val != 0;
        }
    }
}