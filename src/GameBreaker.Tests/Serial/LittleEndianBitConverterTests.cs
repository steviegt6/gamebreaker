/*
 * Copyright (c) 2023 Tomat & GameBreaker Contributors
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using GameBreaker.Serial.Numerics;
using static GameBreaker.Serial.LittleEndianBitConverter;

namespace GameBreaker.Tests.Serial;

[TestFixture]
public static class LittleEndianBitConverterTests {
    private static readonly byte[] test_bytes = {
        0x01,
        0x02,
        0x03,
        0x04,
        0x05,
        0x06,
        0x07,
        0x08,
    };

    [SetUp]
    public static void SetUp() {
        if (!BitConverter.IsLittleEndian)
            throw new Exception("Test only works on little endian systems.");
    }

    [Test]
    public static void TestToInt16() {
        const short expected = 0x0201;

        IsLittleEndian = true;
        var actualLittle = ToInt16(test_bytes, 0);
        IsLittleEndian = false;
        var actualBig = ToInt16(test_bytes, 0);

        Assert.Multiple(() => {
            Assert.That(actualLittle, Is.EqualTo(expected));
            Assert.That(actualBig, Is.EqualTo(expected));
        });
    }

    [Test]
    public static void TestToUInt16() {
        const ushort expected = 0x0201;

        IsLittleEndian = true;
        var actualLittle = ToUInt16(test_bytes, 0);
        IsLittleEndian = false;
        var actualBig = ToUInt16(test_bytes, 0);
        
        Assert.Multiple(() => {
            Assert.That(actualLittle, Is.EqualTo(expected));
            Assert.That(actualBig, Is.EqualTo(expected));
        });
    }

    [Test]
    public static void TestToInt24() {
        const int expected = 0x030201;

        IsLittleEndian = true;
        var actualLittle = ToInt24(test_bytes, 0);
        IsLittleEndian = false;
        var actualBig = ToInt24(test_bytes, 0);
        
        Assert.Multiple(() => {
            Assert.That(actualLittle, Is.EqualTo(new Int24(expected)));
            Assert.That(actualBig, Is.EqualTo(new Int24(expected)));
        });
    }

    [Test]
    public static void TestToUInt24() {
        const uint expected = 0x030201;

        IsLittleEndian = true;
        var actualLittle = ToUInt24(test_bytes, 0);
        IsLittleEndian = false;
        var actualBig = ToUInt24(test_bytes, 0);
        
        Assert.Multiple(() => {
            Assert.That(actualLittle, Is.EqualTo(new UInt24(expected)));
            Assert.That(actualBig, Is.EqualTo(new UInt24(expected)));
        });
    }

    [Test]
    public static void TestToInt32() {
        const int expected = 0x04030201;

        IsLittleEndian = true;
        var actualLittle = ToInt32(test_bytes, 0);
        IsLittleEndian = false;
        var actualBig = ToInt32(test_bytes, 0);
        
        Assert.Multiple(() => {
            Assert.That(actualLittle, Is.EqualTo(expected));
            Assert.That(actualBig, Is.EqualTo(expected));
        });
    }

    [Test]
    public static void TestToUInt32() {
        const uint expected = 0x04030201;

        IsLittleEndian = true;
        var actualLittle = ToUInt32(test_bytes, 0);
        IsLittleEndian = false;
        var actualBig = ToUInt32(test_bytes, 0);
        
        Assert.Multiple(() => {
            Assert.That(actualLittle, Is.EqualTo(expected));
            Assert.That(actualBig, Is.EqualTo(expected));
        });
    }

    [Test]
    public static void TestToInt64() {
        const long expected = 0x0807060504030201;

        IsLittleEndian = true;
        var actualLittle = ToInt64(test_bytes, 0);
        IsLittleEndian = false;
        var actualBig = ToInt64(test_bytes, 0);
        
        Assert.Multiple(() => {
            Assert.That(actualLittle, Is.EqualTo(expected));
            Assert.That(actualBig, Is.EqualTo(expected));
        });
    }

    [Test]
    public static void TestToUInt64() {
        const ulong expected = 0x0807060504030201;

        IsLittleEndian = true;
        var actualLittle = ToUInt64(test_bytes, 0);
        IsLittleEndian = false;
        var actualBig = ToUInt64(test_bytes, 0);
        
        Assert.Multiple(() => {
            Assert.That(actualLittle, Is.EqualTo(expected));
            Assert.That(actualBig, Is.EqualTo(expected));
        });
    }

    [Test]
    public static void TestToSingle() {
        const float expected = 0x04030201;

        IsLittleEndian = true;
        var actualLittle = ToSingle(test_bytes, 0);
        IsLittleEndian = false;
        var actualBig = ToSingle(test_bytes, 0);
        
        Assert.Multiple(() => {
            Assert.That(actualLittle, Is.EqualTo(expected));
            Assert.That(actualBig, Is.EqualTo(expected));
        });
    }

    [Test]
    public static void TestToDouble() {
        const double expected = 0x0807060504030201;

        IsLittleEndian = true;
        var actualLittle = ToDouble(test_bytes, 0);
        IsLittleEndian = false;
        var actualBig = ToDouble(test_bytes, 0);
        
        Assert.Multiple(() => {
            Assert.That(actualLittle, Is.EqualTo(expected));
            Assert.That(actualBig, Is.EqualTo(expected));
        });
    }
}
