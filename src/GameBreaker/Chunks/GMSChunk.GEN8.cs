// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System;
using System.Collections.Generic;
using System.Linq;
using GameBreaker.Core.Abstractions;
using GameBreaker.Core.Abstractions.IFF;
using GameBreaker.Core.Abstractions.IFF.GM;
using GameBreaker.Core.Abstractions.Serialization;
using GameBreaker.Util;

namespace GameBreaker.Chunks;

partial class GMSChunk
{
    public class GEN8 : GMSChunk
    {
        #region Enums

        [Flags]
        public enum InfoFlags : uint
        {
            Fullsceen         = 0x00001,
            SyncVertex1       = 0x00002,
            SyncVertex2       = 0x00004,
            Interpolate       = 0x00008,
            Scale             = 0x00010,
            ShowCursor        = 0x00020,
            Sizeable          = 0x00040,
            ScreenKey         = 0x00080,
            SyncVertex3       = 0x00100,
            StudioVersionB1   = 0x00200,
            StudioVersionB2   = 0x00400,
            StudioVersionB3   = 0x00800,
            StudioVersionMask = 0x00E00,
            SteamOrPlayer     = 0x01000,
            LocalDataEnabled  = 0x02000,
            BorderlessWindow  = 0x04000,
            DefaultCodeKind   = 0x08000,
            LicenseExclusions = 0x10000
        }

        [Flags]
        public enum FunctionClassification : ulong
        {
            None             = 0x0000000000000000,
            Internet         = 0x0000000000000001,
            Joystick         = 0x0000000000000002,
            Gamepad          = 0x0000000000000004,
            ReadScreenPixels = 0x0000000000000010,
            Math             = 0x0000000000000020,
            Action           = 0x0000000000000040,
            D3DState         = 0x0000000000000080,
            D3DPrimitive     = 0x0000000000000100,
            DataStructure    = 0x0000000000000200,
            FileLegacy       = 0x0000000000000400,
            Ini              = 0x0000000000000800,
            Filename         = 0x0000000000001000,
            Directory        = 0x0000000000002000,
            Shell            = 0x0000000000004000,
            Obsolete         = 0x0000000000008000,
            Http             = 0x0000000000010000,
            JsonZip          = 0x0000000000020000,
            Debug            = 0x0000000000040000,
            Motion           = 0x0000000000080000,
            Collision        = 0x0000000000100000,
            Instance         = 0x0000000000200000,
            Room             = 0x0000000000400000,
            Game             = 0x0000000000800000,
            Display          = 0x0000000001000000,
            Device           = 0x0000000002000000,
            Window           = 0x0000000004000000,
            Draw             = 0x0000000008000000,
            Texture          = 0x0000000010000000,
            Graphics         = 0x0000000020000000,
            String           = 0x0000000040000000,
            Tile             = 0x0000000080000000,
            Surface          = 0x0000000100000000,
            Skeleton         = 0x0000000200000000,
            IO               = 0x0000000400000000,
            GMSystem         = 0x0000000800000000,
            Array            = 0x0000001000000000,
            External         = 0x0000002000000000,
            Push             = 0x0000004000000000,
            Date             = 0x0000008000000000,
            Particle         = 0x0000010000000000,
            Resource         = 0x0000020000000000,
            Html5            = 0x0000040000000000,
            Sound            = 0x0000080000000000,
            Audio            = 0x0000100000000000,
            Event            = 0x0000200000000000,
            Script           = 0x0000400000000000,
            Text             = 0x0000800000000000,
            Analytics        = 0x0001000000000000,
            Object           = 0x0002000000000000,
            Asset            = 0x0004000000000000,
            Achievement      = 0x0008000000000000,
            Cloud            = 0x0010000000000000,
            Ads              = 0x0020000000000000,
            Os               = 0x0040000000000000,
            iap              = 0x0080000000000000,
            Facebook         = 0x0100000000000000,
            Physics          = 0x0200000000000000,
            SWF              = 0x0400000000000000,
            PlatformSpecific = 0x0800000000000000,
            Buffer           = 0x1000000000000000,
            Steam            = 0x2000000000000000,
            SteamUGC         = 0x2010000000000000,
            Shader           = 0x4000000000000000,
            Vertex           = 0x8000000000000000
        }

        #endregion

        #region Properties

        public bool DisableDebug { get; set; }

        public byte FormatID { get; set; }

        public short Unknown { get; set; }

        public GmString Filename { get; set; } = null!;

        public GmString Config { get; set; } = null!;
        
        public int LastObjectID { get; set; }
        
        public int LastTileID { get; set; }
        
        public int GameID { get; set; }
        
        public Guid LegacyGUID { get; set; }

        public GmString GameName { get; set; } = null!;
        
        public int Major { get; set; }
        
        public int Minor { get; set; }
        
        public int Release { get; set; }
        
        public int Build { get; set; }

        public int DefaultWindowWidth  { get; set; }
        
        public int DefaultWindowHeight { get; set; }
        
        public InfoFlags Info { get; set; }

        public byte[] LicenseMD5 { get; set; } = Array.Empty<byte>();
        
        public int LicenseCRC32 { get; set; }
        
        public long Timestamp { get; set; }

        public GmString DisplayName { get; set; } = null!;
        
        public long ActiveTargets { get; set; }
        
        public FunctionClassification FunctionClassifications { get; set; }
        
        public int SteamAppID { get; set; }
        
        public int DebuggerPort { get; set; }

        public List<int> RoomOrder { get; set; } = new();

        public List<long> GMS2_RandomUID { get; set; } = new();

        public float GMS2_FPS { get; set; }

        public bool GMS2_AllowStatistics { get; set; }

        public Guid GMS2_GameGUID { get; set; }

        #endregion

        protected override ChunkIdentity ExpectedIdentity => new("GEN8");

        protected override void SerializeChunk(IGmDataSerializer s) {
            if (s.GameMakerFile is not GameMakerFile gmFile) throw new Exception(); // TODO
            
            s.Write(                            DisableDebug);
            s.Write(                            gmFile.VersionInfo.FormatID = FormatID);
            s.Write(                            Unknown);
            s.WritePointerString(               Filename);
            s.WritePointerString(               Config);
            s.Write(                            LastObjectID);
            s.Write(                            LastTileID);
            s.Write(                            GameID);
            s.WriteGuid(                        LegacyGUID);
            s.WritePointerString(               GameName);
            s.Write(                            Major);
            s.Write(                            Minor);
            s.Write(                            Release);
            s.Write(                            Build);
            s.WriteGameMakerVersion(            Major, Minor, Release, Build, gmFile);
            s.Write(                            DefaultWindowWidth);
            s.Write(                            DefaultWindowHeight);
            s.WriteInfoFlags(                   Info);
            s.Write(                            LicenseCRC32);
            s.Write(                            LicenseMD5);
            s.Write(                            Timestamp);
            s.WritePointerString(               DisplayName);
            s.Write(                            ActiveTargets);
            s.WriteFunctionClassification(      FunctionClassifications);
            s.Write(                            SteamAppID);
            if (FormatID >= 14) { s.Write(      DebuggerPort); }
            s.WriteRoomOrder(                   RoomOrder);
            if (Major >= 2) { s.WriteRandomUID( GMS2_RandomUID, this, gmFile);
                              s.Write(          GMS2_FPS);
                              s.Write(          GMS2_AllowStatistics);
                              s.WriteGuid(      GMS2_GameGUID); }
        }

        protected override void DeserializeChunk(IGmDataDeserializer d) {
            /*if (d.GameMakerFile is not GameMakerFile gmFile) throw new Exception(); // TODO
            
            DisableDebug                           = d.ReadByte() != 0; // only time a boolean isn't wide
            gmFile.VersionInfo.FormatID = FormatID = d.ReadByte();
            Unknown                                = d.ReadInt16();
            Filename                               = d.ReadStringPointerObject();
            Config                                 = d.ReadStringPointerObject();
            LastObjectID                           = d.ReadInt32();
            LastTileID                             = d.ReadInt32();
            GameID                                 = d.ReadInt32();
            LegacyGUID                             = d.ReadGuid();
            GameName                               = d.ReadStringPointerObject();
            (Major, Minor, Release, Build)         = d.ReadGameMakerVersion(gmFile);
            DefaultWindowWidth                     = d.ReadInt32();
            DefaultWindowHeight                    = d.ReadInt32();
            Info                                   = d.ReadInfoFlags();
            LicenseCRC32                           = d.ReadInt32();
            LicenseMD5                             = d.ReadBytes(16);
            Timestamp                              = d.ReadInt64();
            DisplayName                            = d.ReadStringPointerObject();
            ActiveTargets                          = d.ReadInt64();
            FunctionClassifications                = d.ReadFunctionClassification();
            SteamAppID                             = d.ReadInt32();
            if (FormatID >= 14) { DebuggerPort     = d.ReadInt32(); }
            RoomOrder                              = d.ReadRoomOrder();
            if (Major >= 2) { GMS2_RandomUID       = d.ReadRandomUID(this, gmFile);
                              GMS2_FPS             = d.ReadSingle();
                              GMS2_AllowStatistics = d.ReadBoolean();
                              GMS2_GameGUID        = d.ReadGuid(); }*/
        }
    }
}