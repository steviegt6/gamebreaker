using System.Text;

namespace GameBreaker.Tests.Serial; 

[TestFixture]
public static class CharConversionTests {
    private static readonly Encoding encoding = new UTF8Encoding(false);

    [Test]
    public static void TestCharConversion() {
        for (var i = 0; i < 127; i++) {
            var c = (char) i;
            var b = Convert.ToByte(c);
            var bytes = encoding.GetBytes(c.ToString());
            Assert.That(bytes[0], Is.EqualTo(b));
        }
    }
}
