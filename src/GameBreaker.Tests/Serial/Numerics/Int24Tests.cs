using GameBreaker.Serial.Numerics;

namespace GameBreaker.Tests.Serial.Numerics;

[TestFixture]
public static class Int24Tests {
    [TestCase(int.MinValue, false)]
    [TestCase(int.MaxValue, false)]
    [TestCase(Int24.MinValue, true)]
    [TestCase(Int24.MaxValue, true)]
    [TestCase(0, true)]
    [TestCase(1, true)]
    [TestCase(-1, true)]
    public static void Int24ConversionEqualityTest(int value, bool equal) {
        var int24 = new Int24(value);
        var converted = (int) int24;
        Assert.That(converted == value, Is.EqualTo(equal));
    }

    [TestCase(uint.MinValue, true)]
    [TestCase(uint.MaxValue, false)]
    [TestCase(UInt24.MinValue, true)]
    [TestCase(UInt24.MaxValue, true)]
    [TestCase(0u, true)]
    [TestCase(1u, true)]
    public static void UInt24ConversionEqualityTest(uint value, bool equal) {
        var uint24 = new UInt24(value);
        var converted = (uint) uint24;
        Assert.That(converted == value, Is.EqualTo(equal));
    }
}
