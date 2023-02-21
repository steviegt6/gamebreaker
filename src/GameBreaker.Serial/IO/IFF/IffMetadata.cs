using System;
using ViState = GameBreaker.Serial.IO.IFF.VersionInferenceState;

namespace GameBreaker.Serial.IO.IFF;

/// <summary>
///     Describes elements of an IFF file that are either inferred or manually
///     specified, such as the GameMaker version to deserialize based on.
/// </summary>
public class IffMetadata {
    public VersionInfo VersionInfo { get; init; } = new();

    public bool RunFromIde { get; set; }

    public byte FormatId { get; set; }
}

/// <summary>
///     Version metadata used within <see cref="IffMetadata"/>.
/// </summary>
public class VersionInfo {
    /// <summary>
    ///     The GameMaker version. <br />
    ///     This is <see cref="GmsVersion.UNKNOWN"/> by default, which will
    ///     error during deserialization if <see cref="Inference"/> is not
    ///     <see cref="ViState.Inferring"/> and during
    ///     serialization if <see cref="Inference"/> is not
    ///     <see cref="ViState.Inferred"/>.
    /// </summary>
    /// <remarks>
    ///     More specifically, if <see cref="Inference"/> is
    ///     <see cref="ViState.NotInferred"/>, then
    ///     <see cref="Version"/> cannot be <see cref="GmsVersion.UNKNOWN"/>.
    /// </remarks>
    public GmsVersion Version { get; init; } = GmsVersion.UNKNOWN;

    /// <summary>
    ///     The inference state of the <see cref="Version"/>.
    /// </summary>
    public ViState Inference { get; set; } =
        ViState.Inferring;
}

/// <summary>
///     Describes the inference state of the <see cref="GmsVersion"/> in
///     <see cref="VersionInfo"/>.
/// </summary>
public enum VersionInferenceState {
    NotInferred,
    Inferring,
    Inferred,
}

public static class VersionInfoExtensions {
    public static bool IsUnknown(this VersionInfo versionInfo) {
        return versionInfo.Version == GmsVersion.UNKNOWN;
    }

    public static bool IsInferring(this VersionInfo versionInfo) {
        return versionInfo.Inference == ViState.Inferring;
    }

    public static void MarkInferred(this VersionInfo versionInfo) {
        versionInfo.Inference = ViState.Inferred;
    }

    public static bool ValidateInference(this VersionInfo vi) {
        return vi.Inference switch {
            ViState.Inferred => !vi.IsUnknown(),
            ViState.Inferring => true,
            ViState.NotInferred => !vi.IsUnknown(),
            _ => throw new ArgumentOutOfRangeException(nameof(vi)),
        };
    }
}
