// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System;
using System.Collections.Generic;
using GameBreaker.Core.Abstractions.IFF;
using GameBreaker.Chunks;

namespace GameBreaker.Util;

public static class Constants
{
    public const string FORM = "FORM";
    public const string GEN8 = "GEN8";
    public const string OPTN = "OPTN";
    public const string LANG = "LANG";
    public const string EXTN = "EXTN";
    public const string STRG = "STRG";
    public const string TPAG = "TPAG";
    public const string TXTR = "TXTR";
    public const string AGRP = "AGRP";
    public const string AUDO = "AUDO";
    public const string SOND = "SOND";
    public const string BGND = "BGND";
    public const string PATH = "PATH";
    public const string EMBI = "EMBI";
    public const string DAFL = "DAFL";
    public const string TGIN = "TGIN";
    public const string FONT = "FONT";
    public const string SPRT = "SPRT";
    public const string ACRV = "ACRV";
    public const string FUNC = "FUNC";
    public const string VARI = "VARI";
    public const string SCPT = "SCPT";
    public const string TAGS = "TAGS";
    public const string ROOM = "ROOM";
    public const string OBJT = "OBJT";
    public const string TMLN = "TMLN";
    public const string GLOB = "GLOB";
    public const string GMEN = "GMEN";
    public const string SHDR = "SHDR";
    public const string CODE = "CODE";
    public const string SEQN = "SEQN";
    public const string FEDS = "FEDS";
    public const string FEAT = "FEAT";
    
    public static readonly Dictionary<string, Func<IChunk>> CHUNKS = new()
    {
        {GEN8, () => new GMSChunk.GEN8()},
        // {OPTN, () => new GMSChunk.OPTN()},
        // {LANG, () => new GMSChunk.LANG()},
        // {EXTN, () => new GMSChunk.EXTN()},
        // {STRG, () => new GMSChunk.STRG()},
        // {TPAG, () => new GMSChunk.TPAG()},
        // {TXTR, () => new GMSChunk.TXTR()},
        // {AGRP, () => new GMSChunk.AGRP()},
        // {AUDO, () => new GMSChunk.AUDO()},
        // {SOND, () => new GMSChunk.SOND()},
        // {BGND, () => new GMSChunk.BGND()},
        // {PATH, () => new GMSChunk.PATH()},
        // {EMBI, () => new GMSChunk.EMBI()},
        // {DAFL, () => new GMSChunk.DAFL()},
        // {TGIN, () => new GMSChunk.TGIN()},
        // {FONT, () => new GMSChunk.FONT()},
        // {SPRT, () => new GMSChunk.SPRT()},
        // {ACRV, () => new GMSChunk.ACRV()},
        // {FUNC, () => new GMSChunk.FUNC()},
        // {VARI, () => new GMSChunk.VARI()},
        // {SCPT, () => new GMSChunk.SCPT()},
        // {TAGS, () => new GMSChunk.TAGS()},
        // {ROOM, () => new GMSChunk.ROOM()},
        // {OBJT, () => new GMSChunk.OBJT()},
        // {TMLN, () => new GMSChunk.TMLN()},
        // {GLOB, () => new GMSChunk.GLOB()},
        // {GMEN, () => new GMSChunk.GMEN()},
        // {SHDR, () => new GMSChunk.SHDR()},
        // {CODE, () => new GMSChunk.CODE()},
        // {SEQN, () => new GMSChunk.SEQN()},
        // {FEDS, () => new GMSChunk.FEDS()},
        // {FEAT, () => new GMSChunk.FEAT()}
    };
}