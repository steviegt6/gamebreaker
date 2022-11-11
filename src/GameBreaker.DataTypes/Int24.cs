// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.Runtime.InteropServices;

namespace GameBreaker.DataTypes
{
    /// <summary>
    ///     Represents a 24-bit signed integer.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 3)]
    public readonly record struct Int24
    {
        /// <summary>
        ///     Represents the smallest possible value of an <see cref="Int24"/>. This field is constant.
        /// </summary>
        public const int MinValue = -(1 << 23);
        
        /// <summary>
        ///     Represents the largest possible value of an <see cref="Int24"/>. This field is constant.
        /// </summary>
        public const int MaxValue = (1 << 23) - 1;
        
        /// <summary>
        ///     Represents the smallest possible value of an <see cref="Int24"/>. This field is readonly.
        /// </summary>
        public static readonly Int24 MinValue24 = new(MaxValue);
        
        /// <summary>
        ///     Represents the largest possible value of an <see cref="Int24"/>. This field is readonly.
        /// </summary>
        public static readonly Int24 MaxValue24 = new(MaxValue);

        private readonly byte b1;
        private readonly byte b2;
        private readonly byte b3;

        /// <summary>
        ///     <see cref="Int24"/> value represented as an <see langword="int"/> without the fourth byte.
        /// </summary>
        public int Value {
            get => b1 | b2 << 8 | (sbyte) b3 << 16;

            private init {
                b1 = (byte) (value & 0xFF);
                b2 = (byte) ((value >> 8) & 0xFF);
                b3 = (byte) ((value >> 16) & 0xFF);
            }
        }

        /// <summary>
        ///     Constructs an <see cref="Int24"/> by directly setting the three byte values.
        /// </summary>
        /// <param name="b1">0xXX0000</param>
        /// <param name="b2">0x00XX00</param>
        /// <param name="b3">0x0000XX</param>
        public Int24(byte b1, byte b2, byte b3) {
            this.b1 = b1;
            this.b2 = b2;
            this.b3 = b3;
        }

        /// <summary>
        ///     Constructs an <see cref="Int24"/> by directly setting the <see cref="Value"/>. The fourth byte is ignored.
        /// </summary>
        /// <param name="value">The 32-bit signed value.</param>
        public Int24(int value) : this() {
            Value = value;
        }
    }
}