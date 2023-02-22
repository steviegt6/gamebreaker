using System;
using System.Collections.Generic;
using System.Diagnostics;
using GameBreaker.Serial.IO;
using GameBreaker.Serial.IO.IFF;

namespace GameBreaker.Serial.Chunks;

public class Gen8ChunkData : ChunkData {
    public ISerializable<bool> DisableDebug { get; } =
        new SerializableBool();

    public ISerializable<byte> FormatId { get; }
        = new SerializableByte();

    public ISerializable<short> Unknown { get; }
        = new SerializableShort();

    public IPointerSerializable<string>? FileName { get; set; }

    public IPointerSerializable<string>? Config { get; set; }

    public ISerializable<int> LastObjectId { get; }
        = new SerializableInt();

    public ISerializable<int> LastTileId { get; }
        = new SerializableInt();

    public ISerializable<int> GameId { get; }
        = new SerializableInt();

    public ISerializable<Guid> LegacyGuid { get; }
        = new SerializableGuid();

    public IPointerSerializable<string>? GameName { get; set; }

    public ISerializable<int> Major { get; }
        = new SerializableInt();

    public ISerializable<int> Minor { get; }
        = new SerializableInt();

    public ISerializable<int> Release { get; }
        = new SerializableInt();

    public ISerializable<int> Build { get; }
        = new SerializableInt();

    public ISerializable<int> DefaultWindowWidth { get; } =
        new SerializableInt();

    public ISerializable<int> DefaultWindowHeight { get; } =
        new SerializableInt();

    public ISerializable<Gen8InfoFlags> Info { get; } =
        new SerializableGen8InfoFlags();

    public ISerializable<byte[]> LicenseMd5 { get; } =
        new SerializableByteArray(16);

    public ISerializable<int> LicenseCrc32 { get; }
        = new SerializableInt();

    public ISerializable<long> Timestamp { get; }
        = new SerializableLong();

    public IPointerSerializable<string>? DisplayName { get; set; }

    public ISerializable<long> ActiveTargets { get; }
        = new SerializableLong();

    public ISerializable<Gen8FunctionClassification> FunctionClasses { get; }
        = new SerializableGen8FunctionClassification();

    public ISerializable<int> SteamAppId { get; }
        = new SerializableInt();

    public ISerializable<int> DebuggerPort { get; }
        = new SerializableInt();

    public ISerializable<Gen8RoomData> RoomData { get; } =
        new SerializableGen8RoomData();

    public ISerializable<Gen8RandomUid> RandomUid { get; }
        = new SerializableGen8RandomUid();

    public ISerializable<float> Fps { get; }
        = new SerializableFloat();

    public ISerializable<bool> AllowStatistics { get; } =
        new SerializableWideBool();

    public ISerializable<Guid> GameGuid { get; }
        = new SerializableGuid();

    public GmsVersion GmsVersion => new(
        Major.Value,
        Minor.Value,
        Release.Value,
        Minor.Value
    );

    public override void Serialize(IWriter writer, IffFile iffFile) {
        Debug.Assert(FileName is not null);
        Debug.Assert(Config is not null);
        Debug.Assert(GameName is not null);
        Debug.Assert(DisplayName is not null);
        
        DisableDebug.Serialize(writer, this, iffFile);
        FormatId.Serialize(writer, this, iffFile);
        iffFile.Metadata.FormatId = FormatId.Value;
        Unknown.Serialize(writer, this, iffFile);
        writer.WritePointer(FileName);
        writer.WritePointer(Config);
        LastObjectId.Serialize(writer, this, iffFile);
        LastTileId.Serialize(writer, this, iffFile);
        GameId.Serialize(writer, this, iffFile);
        LegacyGuid.Serialize(writer, this, iffFile);
        writer.WritePointer(GameName);
        Major.Serialize(writer, this, iffFile);
        Minor.Serialize(writer, this, iffFile);
        Release.Serialize(writer, this, iffFile);
        Build.Serialize(writer, this, iffFile);

        switch (iffFile.Metadata.VersionInfo.Inference) {
            case VersionInferenceState.NotInferred:
                if (iffFile.Metadata.VersionInfo.Version < GmsVersion)
                    throw new InvalidOperationException("Bad version");

                break;

            case VersionInferenceState.Inferring:
                throw new InvalidOperationException(
                    "Cannot serialize while inferring version"
                );

            case VersionInferenceState.Inferred:
                break;

            default:
                throw new InvalidOperationException("Bad inference state");
        }

        DefaultWindowWidth.Serialize(writer, this, iffFile);
        DefaultWindowHeight.Serialize(writer, this, iffFile);
        Info.Serialize(writer, this, iffFile);
        LicenseCrc32.Serialize(writer, this, iffFile);
        LicenseMd5.Serialize(writer, this, iffFile);
        Timestamp.Serialize(writer, this, iffFile);
        writer.WritePointer(DisplayName);
        ActiveTargets.Serialize(writer, this, iffFile);
        FunctionClasses.Serialize(writer, this, iffFile);
        SteamAppId.Serialize(writer, this, iffFile);

        if (FormatId.Value >= 14)
            DebuggerPort.Serialize(writer, this, iffFile);

        RoomData.Serialize(writer, this, iffFile);

        if (GmsVersion < GmsVersion.GMS_2)
            return;

        RandomUid.Serialize(writer, this, iffFile);
        Fps.Serialize(writer, this, iffFile);
        AllowStatistics.Serialize(writer, this, iffFile);
        GameGuid.Serialize(writer, this, iffFile);
    }

    public override void Deserialize(
        IReader reader,
        IffFile iffFile,
        ChunkPosInfo posInfo
    ) {
        DisableDebug.Deserialize(reader, this, iffFile);
        FormatId.Deserialize(reader, this, iffFile);
        iffFile.Metadata.FormatId = FormatId.Value;
        Unknown.Deserialize(reader, this, iffFile);
        FileName = reader.ReadPointerObject(
            reader.ReadInt32() - 4,
            () => new SerializableGmString()
        );
        Config = reader.ReadPointerObject(
            reader.ReadInt32() - 4,
            () => new SerializableGmString()
        );
        LastObjectId.Deserialize(reader, this, iffFile);
        LastTileId.Deserialize(reader, this, iffFile);
        GameId.Deserialize(reader, this, iffFile);
        LegacyGuid.Deserialize(reader, this, iffFile);
        GameName = reader.ReadPointerObject(
            reader.ReadInt32() - 4,
            () => new SerializableGmString()
        );
        Major.Deserialize(reader, this, iffFile);
        Minor.Deserialize(reader, this, iffFile);
        Release.Deserialize(reader, this, iffFile);
        Build.Deserialize(reader, this, iffFile);
        iffFile.Metadata.VersionInfo.AttemptUpdate(GmsVersion);
        DefaultWindowWidth.Deserialize(reader, this, iffFile);
        DefaultWindowHeight.Deserialize(reader, this, iffFile);
        Info.Deserialize(reader, this, iffFile);
        LicenseCrc32.Deserialize(reader, this, iffFile);
        LicenseMd5.Deserialize(reader, this, iffFile);
        Timestamp.Deserialize(reader, this, iffFile);
        DisplayName = reader.ReadPointerObject(
            reader.ReadInt32() - 4,
            () => new SerializableGmString()
        );
        ActiveTargets.Deserialize(reader, this, iffFile);
        FunctionClasses.Deserialize(reader, this, iffFile);
        SteamAppId.Deserialize(reader, this, iffFile);

        if (FormatId.Value >= 14)
            DebuggerPort.Deserialize(reader, this, iffFile);

        RoomData.Deserialize(reader, this, iffFile);

        if (GmsVersion < GmsVersion.GMS_2)
            return;

        RandomUid.Deserialize(reader, this, iffFile);
        Fps.Deserialize(reader, this, iffFile);
        AllowStatistics.Deserialize(reader, this, iffFile);
        GameGuid.Deserialize(reader, this, iffFile);
    }
}

[Flags]
public enum Gen8InfoFlags : uint {
    /// <summary>
    ///     Start in fullscreen.
    /// </summary>
    Fullscreen        = 0x0001,

    /// <summary>
    ///     Use VSync to avoid tearing issues.
    /// </summary>
    SyncVertex1       = 0x0002,

    /// <summary>
    /// </summary>
    SyncVertex2       = 0x0004,

    /// <summary>
    ///     Interpolate pixels.
    /// </summary>
    Interpolate       = 0x0008,

    /// <summary>
    ///     Maintain aspect ratio for odd resolutions through scaling.
    /// </summary>
    Scale             = 0x0010,

    /// <summary>
    ///     Shows the cursor in the game window.
    /// </summary>
    ShowCursor        = 0x0020,

    /// <summary>
    ///     Allows for window resizing.
    /// </summary>
    AllowResize       = 0x0040,

    /// <summary>
    ///     Allows for toggling fullscreen with a key combination.
    /// </summary>
    FullscreenToggle  = 0x0080,

    /// <summary>
    /// </summary>
    SyncVertex3       = 0x0100,

    /// <summary>
    /// </summary>
    StudioVersionB1   = 0x0200,

    /// <summary>
    /// </summary>
    StudioVersionB2   = 0x0400,

    /// <summary>
    /// </summary>
    StudioVersionB3   = 0x0800,

    /// <summary>
    /// </summary>
    /// <remarks><c>(infoFlags & InfoFlags.StudioVersionMask) >> 9</c></remarks>
    StudioVersionMask = 0x0E00,

    /// <summary>
    ///     Indicates that the game is running under a Steam or YoYo player.
    /// </summary>
    SteamOrPlayer     = 0x1000,

    /// <summary>
    /// </summary>
    LocalDataEnabled  = 0x2000,

    /// <summary>
    ///     Starts the game is borderless window mode.
    /// </summary>
    BorderlessWindow  = 0x4000,  // Borderless Window

    /// <summary>
    ///     Indicates the default code kind for the game (GML or GML Visual).
    /// </summary>
    DefaultCodeKind   = 0x8000,

    /// <summary>
    /// </summary>
    LicenseExclusions = 0x10000,
}

[Flags]
public enum Gen8FunctionClassification : ulong {
    None             = 0x0,
    Internet         = 0x1,
    Joystick         = 0x2,
    Gamepad          = 0x4,
    ReadScreenPixels = 0x10,
    Math             = 0x20,
    Action           = 0x40,
    D3DState         = 0x80,
    D3DPrimitive     = 0x100,
    DataStructure    = 0x200,
    FileLegacy       = 0x400,
    Ini              = 0x800,
    Filename         = 0x1000,
    Directory        = 0x2000,
    Shell            = 0x4000,
    Obsolete         = 0x8000,
    Http             = 0x10000,
    JsonZip          = 0x20000,
    Debug            = 0x40000,
    Motion           = 0x80000,
    Collision        = 0x100000,
    Instance         = 0x200000,
    Room             = 0x400000,
    Game             = 0x800000,
    Display          = 0x1000000,
    Device           = 0x2000000,
    Window           = 0x4000000,
    Draw             = 0x8000000,
    Texture          = 0x10000000,
    Graphics         = 0x20000000,
    String           = 0x40000000,
    Tile             = 0x80000000,
    Surface          = 0x100000000,
    Skeleton         = 0x200000000,
    Io               = 0x400000000,
    GmSystem         = 0x800000000,
    Array            = 0x1000000000,
    External         = 0x2000000000,
    Push             = 0x4000000000,
    Date             = 0x8000000000,
    Particle         = 0x10000000000,
    Resource         = 0x20000000000,
    Html5            = 0x40000000000,
    Sound            = 0x80000000000,
    Audio            = 0x100000000000,
    Event            = 0x200000000000,
    Script           = 0x400000000000,
    Text             = 0x800000000000,
    Analytics        = 0x1000000000000,
    Object           = 0x2000000000000,
    Asset            = 0x4000000000000,
    Achievement      = 0x8000000000000,
    Cloud            = 0x10000000000000,
    Ads              = 0x20000000000000,
    Os               = 0x40000000000000,
    Iap              = 0x80000000000000,
    Facebook         = 0x100000000000000,
    Physics          = 0x200000000000000,
    Swf              = 0x400000000000000,
    PlatformSpecific = 0x800000000000000,
    Buffer           = 0x1000000000000000,
    Steam            = 0x2000000000000000,
    SteamUgc         = 0x2010000000000000,
    Shader           = 0x4000000000000000,
    Vertex           = 0x8000000000000000,
}

public sealed class Gen8RoomData {
    public required List<int> RoomOrder { get; init; }
}

public sealed class Gen8RandomUid {
    public required List<long> RandomUid { get; init; }
}
