// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.Runtime.InteropServices;

namespace GameBreaker.Abstractions
{
    // TODO: Proper implementation?
    [StructLayout(LayoutKind.Sequential, Size = 3)]
    public readonly record struct Int24
    {
        public const int MinValue = -(1 << 23);
        public const int MaxValue = (1 << 23) - 1;
        public static readonly Int24 MinValue24 = new(MaxValue);
        public static readonly Int24 MaxValue24 = new(MaxValue);

        private readonly byte b1;
        private readonly byte b2;
        private readonly byte b3;

        public int Value {
            get => b1 | b2 << 8 | (sbyte) b3 << 16;

            private init {
                b1 = (byte) (value & 0xFF);
                b2 = (byte) ((value >> 8) & 0xFF);
                b3 = (byte) ((value >> 16) & 0xFF);
            }
        }

        public Int24(byte b1, byte b2, byte b3) {
            this.b1 = b1;
            this.b2 = b2;
            this.b3 = b3;
        }

        public Int24(int value) : this() {
            Value = value;
        }
    }
}