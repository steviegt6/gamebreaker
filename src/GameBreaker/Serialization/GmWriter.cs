// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.
// Copyright (c) colinator27. Licensed under the MIT License.
// See the LICENSE-DogScepter file in the repository root for full terms and conditions.

namespace GameBreaker.Serialization
{
    public class GmWriter : BufferedWriter
    {
        public override void Write(bool value) {
            // Int32
            Write(value ? 1 : 0);
        }
    }
}