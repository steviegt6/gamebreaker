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

using System;

namespace GameBreaker;

/// <summary>
///     The version of GameMaker Studio that a given IFF file was created in.
///     <br />
///     Rough estimates only, as GameMaker does not actually store this data,
///     and it is instead inferred by changes to the IFF format (new/removed
///     chunks, modified chunks, etc.).
/// </summary>
public sealed record GmVersion(
    int Major = 0,
    int Minor = 0,
    int Release = 0,
    int Patch = 0
) : IComparable,
    IComparable<GmVersion> {
#region Constants
    public static readonly GmVersion UNKNOWN = new();
    public static readonly GmVersion DEFAULT = new(1);

    public static readonly GmVersion GM_2 = new(2);
    public static readonly GmVersion GM_2_2_2_302 = new(2, 2, 2, 302);
    public static readonly GmVersion GM_2_3 = new(2, 3);
    public static readonly GmVersion GM_2_3_1 = new(2, 3, 1);
    public static readonly GmVersion GM_2_3_2 = new(2, 3, 2);
    public static readonly GmVersion GM_2_3_6 = new(2, 3, 6);
    public static readonly GmVersion GM_2022_1 = new(2022, 1);
    public static readonly GmVersion GM_2022_2 = new(2022, 2);
    public static readonly GmVersion GM_2022_3 = new(2022, 3);
    public static readonly GmVersion GM_2022_5 = new(2022, 5);
    public static readonly GmVersion GM_2022_6 = new(2022, 6);
    public static readonly GmVersion GM_2022_8 = new(2022, 8);
    public static readonly GmVersion GM_2022_9 = new(2022, 9);
    public static readonly GmVersion GM_2023_1 = new(2023, 1);
#endregion

    public int Major { get; set; } = Major;

    public int Minor { get; set; } = Minor;

    public int Release { get; set; } = Release;

    public int Patch { get; set; } = Patch;

    /// <summary>
    ///     "Updates" this version to the highest of the two versions.
    /// </summary>
    /// <param name="other"></param>
    public void Update(GmVersion other) {
        if (other.Major > Major) {
            Major = other.Major;
            Minor = other.Minor;
            Release = other.Release;
            Patch = other.Patch;
        }

        if (other.Minor > Minor) {
            Minor = other.Minor;
            Release = other.Release;
            Patch = other.Patch;
        }

        if (other.Release > Release) {
            Release = other.Release;
            Patch = other.Patch;
        }

        if (other.Patch > Patch)
            Patch = other.Patch;
    }

#region Operator Overloads
    public static bool operator >(GmVersion a, GmVersion b) {
        if (a.Major > b.Major)
            return true;

        if (a.Major < b.Major)
            return false;

        if (a.Minor > b.Minor)
            return true;

        if (a.Minor < b.Minor)
            return false;

        if (a.Release > b.Release)
            return true;

        if (a.Release < b.Release)
            return false;

        if (a.Patch > b.Patch)
            return true;

        return false;
    }

    public static bool operator <(GmVersion a, GmVersion b) {
        if (a.Major < b.Major)
            return true;

        if (a.Major > b.Major)
            return false;

        if (a.Minor < b.Minor)
            return true;

        if (a.Minor > b.Minor)
            return false;

        if (a.Release < b.Release)
            return true;

        if (a.Release > b.Release)
            return false;

        if (a.Patch < b.Patch)
            return true;

        return false;
    }

    public static bool operator >=(GmVersion a, GmVersion b) =>
        a == b || a > b;

    public static bool operator <=(GmVersion a, GmVersion b) =>
        a == b || a < b;
#endregion

#region IComparable Impl
    public int CompareTo(object? obj) {
        if (obj is GmVersion other)
            return CompareTo(other);

        return 1;
    }

    public int CompareTo(GmVersion? other) {
        if (ReferenceEquals(this, other))
            return 0;
        if (ReferenceEquals(null, other))
            return 1;

        var majorComparison = Major.CompareTo(other.Major);
        if (majorComparison != 0)
            return majorComparison;

        var minorComparison = Minor.CompareTo(other.Minor);
        if (minorComparison != 0)
            return minorComparison;

        var releaseComparison = Release.CompareTo(other.Release);
        if (releaseComparison != 0)
            return releaseComparison;

        return Patch.CompareTo(other.Patch);
    }
#endregion
}
