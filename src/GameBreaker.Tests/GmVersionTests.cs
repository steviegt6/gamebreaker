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
