// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.Runtime.InteropServices;

namespace GameBreaker.Core.Abstractions
{
    /// <summary>
    ///     Represents a 24-bit unsigned integer.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 3)]
    public readonly record struct UInt24
    {
        /// <summary>
        ///     Represents the smallest possible value of an <see cref="UInt24"/>. This field is constant.
        /// </summary>
        public const uint MinValue = 0x000000;
        
        /// <summary>
        ///     Represents the largest possible value of an <see cref="UInt24"/>. This field is constant.
        /// </summary>
        public const uint MaxValue = 0xFFFFFF;
        
        /// <summary>
        ///     Represents the smallest possible value of an <see cref="UInt24"/>. This field is readonly.
        /// </summary>
        public static readonly UInt24 MinValue24 = new(MaxValue);
        
        /// <summary>
        ///     Represents the largest possible value of an <see cref="UInt24"/>. This field is readonly.
        /// </summary>
        public static readonly UInt24 MaxValue24 = new(MaxValue);
        
        private readonly byte b1;
        private readonly byte b2;
        private readonly byte b3;

        /// <summary>
        ///     <see cref="UInt24"/> value represented as an <see langword="uint"/> without the fourth byte.
        /// </summary>
        public uint Value {
            get => (uint) (b1 | b2 << 8 | b3 << 16);

            private init {
                b1 = (byte) (value & 0xFF);
                b2 = (byte) ((value >> 8) & 0xFF);
                b3 = (byte) ((value >> 16) & 0xFF);
            }
        }

        /// <summary>
        ///     Constructs a <see cref="UInt24"/> by directly setting the three byte values.
        /// </summary>
        /// <param name="b1">0xXX0000</param>
        /// <param name="b2">0x00XX00</param>
        /// <param name="b3">0x0000XX</param>
        public UInt24(byte b1, byte b2, byte b3) {
            this.b1 = b1;
            this.b2 = b2;
            this.b3 = b3;
        }

        /// <summary>
        ///     Constructs a <see cref="UInt24"/> by directly setting the <see cref="Value"/>. The fourth byte is ignored.
        /// </summary>
        /// <param name="value">The 32-bit unsigned value.</param>
        public UInt24(uint value) : this() {
            Value = value;
        }
    }
}