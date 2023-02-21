using System;

namespace GameBreaker.Serial.IO.IFF;

/// <summary>
///     The version of GameMaker Studio that a given IFF file was created in.
///     <br />
///     Rough estimates only, as GameMaker does not actually store this data,
///     and it is instead inferred by changes to the IFF format (new/removed
///     chunks, modified chunks, etc.).
/// </summary>
public record GmsVersion(
    int Major = 0,
    int Minor = 0,
    int Release = 0,
    int Patch = 0
) : IComparable,
    IComparable<GmsVersion> {
#region Constants
    public static readonly GmsVersion UNKNOWN = new();
    public static readonly GmsVersion DEFAULT = new(1);

    public static readonly GmsVersion GMS_2 = new(2);
    public static readonly GmsVersion GMS_2_2_2_302 = new(2, 2, 2, 302);
    public static readonly GmsVersion GMS_2_3 = new(2, 3);
    public static readonly GmsVersion GMS_2_3_1 = new(2, 3, 1);
    public static readonly GmsVersion GMS_2_3_2 = new(2, 3, 2);
    public static readonly GmsVersion GMS_2_3_6 = new(2, 3, 6);
    public static readonly GmsVersion GMS_2022_1 = new(2022, 1);
    public static readonly GmsVersion GMS_2022_2 = new(2022, 2);
    public static readonly GmsVersion GMS_2022_3 = new(2022, 3);
    public static readonly GmsVersion GMS_2022_5 = new(2022, 5);
    public static readonly GmsVersion GMS_2022_6 = new(2022, 6);
    public static readonly GmsVersion GMS_2022_8 = new(2022, 8);
    public static readonly GmsVersion GMS_2022_9 = new(2022, 9);
    public static readonly GmsVersion GMS_2023_1 = new(2023, 1);
#endregion

    public int Major { get; set; } = Major;

    public int Minor { get; set; } = Minor;

    public int Release { get; set; } = Release;

    public int Patch { get; set; } = Patch;

    /// <summary>
    ///     "Updates" this version to the highest of the two versions.
    /// </summary>
    /// <param name="other"></param>
    public void Update(GmsVersion other) {
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
    public static bool operator >(GmsVersion a, GmsVersion b) {
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

    public static bool operator <(GmsVersion a, GmsVersion b) {
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

    public static bool operator >=(GmsVersion a, GmsVersion b) =>
        a == b || a > b;

    public static bool operator <=(GmsVersion a, GmsVersion b) =>
        a == b || a < b;
#endregion

#region IComparable Impl
    public int CompareTo(object? obj) {
        if (obj is GmsVersion other)
            return CompareTo(other);

        return 1;
    }

    public int CompareTo(GmsVersion? other) {
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
