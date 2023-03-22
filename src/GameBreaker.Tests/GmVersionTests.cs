namespace GameBreaker.Tests;

[TestFixture]
public static class GmVersionTests {
    private static readonly GmVersion[] versions = {
        GmVersion.UNKNOWN,
        GmVersion.DEFAULT,
        GmVersion.GM_2,
        GmVersion.GM_2_2_2_302,
        GmVersion.GM_2_3,
        GmVersion.GM_2_3_1,
        GmVersion.GM_2_3_2,
        GmVersion.GM_2_3_6,
        GmVersion.GM_2022_1,
        GmVersion.GM_2022_2,
        GmVersion.GM_2022_3,
        GmVersion.GM_2022_5,
        GmVersion.GM_2022_6,
        GmVersion.GM_2022_8,
        GmVersion.GM_2022_9,
        GmVersion.GM_2023_1,
    };

    [Test]
    public static void TestEquality() {
        for (var i = 0; i < versions.Length; i++) {
            for (var j = 0; j < versions.Length; j++) {
                Assert.That(
                    versions[i],
                    i == j
                        ? Is.EqualTo(versions[j])
                        : Is.Not.EqualTo(versions[j])
                );
            }
        }
    }

    [Test]
    public static void TestUpdate() {
        var version = GmVersion.UNKNOWN;

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
