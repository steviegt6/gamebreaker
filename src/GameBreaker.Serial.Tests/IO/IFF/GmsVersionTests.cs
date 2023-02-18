using GameBreaker.Serial.IO.IFF;

namespace GameBreaker.Serial.Tests.IO.IFF;

[TestFixture]
public static class GmsVersionTests {
    private static readonly GmsVersion[] versions = {
        GmsVersion.UNKNOWN,
        GmsVersion.DEFAULT,
        GmsVersion.GMS_2_2_2_302,
        GmsVersion.GMS_2_3,
        GmsVersion.GMS_2_3_1,
        GmsVersion.GMS_2_3_2,
        GmsVersion.GMS_2_3_6,
        GmsVersion.GMS_2022_1,
        GmsVersion.GMS_2022_2,
        GmsVersion.GMS_2022_3,
        GmsVersion.GMS_2022_5,
        GmsVersion.GMS_2022_6,
        GmsVersion.GMS_2022_8,
        GmsVersion.GMS_2022_9,
        GmsVersion.GMS_2023_1,
    };

    [Test]
    public static void TestEquality() {
        for (var i = 0; i < versions.Length; i++) {
            for (var j = 0; j < versions.Length; j++) {
                Assert.That(versions[i],
                            i == j
                                ? Is.EqualTo(versions[j])
                                : Is.Not.EqualTo(versions[j])
                );
            }
        }
    }

    [Test]
    public static void TestUpdate() {
        var version = GmsVersion.UNKNOWN;

        foreach (var v in versions) {
            version.Update(v);
            Assert.That(version, Is.EqualTo(v));
        }
    }

    [Test]
    public static void TestCompareTo() {
        for (var i = 0; i < versions.Length; i++) {
            for (var j = 0; j < versions.Length; j++) {
                Assert.That(
                    versions[i].CompareTo(versions[j]),
                    Is.EqualTo(i.CompareTo(j))
                );
            }
        }
    }

    [Test]
    public static void TestLessGreaterThan() {
        for (var i = 0; i < versions.Length; i++) {
            for (var j = 0; j < versions.Length; j++) {
                Assert.Multiple(() => {
                    Assert.That(
                        versions[i] < versions[j],
                        Is.EqualTo(i < j)
                    );
                    Assert.That(
                        versions[i] <= versions[j],
                        Is.EqualTo(i <= j)
                    );
                    Assert.That(
                        versions[i] > versions[j],
                        Is.EqualTo(i > j)
                    );
                    Assert.That(
                        versions[i] >= versions[j],
                        Is.EqualTo(i >= j)
                    );
                });
            }
        }
    }
}
