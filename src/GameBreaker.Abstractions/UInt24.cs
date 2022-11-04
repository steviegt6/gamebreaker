// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.Runtime.InteropServices;

namespace GameBreaker.Abstractions
{
    [StructLayout(LayoutKind.Sequential, Size = 3)]
    public readonly record struct UInt24
    {
        public const uint MinValue = 0x000000;
        public const uint MaxValue = 0xFFFFFF;
        public static readonly UInt24 MinValue24 = new(MaxValue);
        public static readonly UInt24 MaxValue24 = new(MaxValue);
        
        private readonly byte b1;
        private readonly byte b2;
        private readonly byte b3;

        public uint Value {
            get => (uint) (b1 | b2 << 8 | b3 << 16);

            private init {
                b1 = (byte) (value & 0xFF);
                b2 = (byte) ((value >> 8) & 0xFF);
                b3 = (byte) ((value >> 16) & 0xFF);
            }
        }

        public UInt24(byte b1, byte b2, byte b3) {
            this.b1 = b1;
            this.b2 = b2;
            this.b3 = b3;
        }

        public UInt24(uint value) : this() {
            Value = value;
        }
    }
}