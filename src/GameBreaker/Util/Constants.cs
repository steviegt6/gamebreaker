// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System;
using System.Collections.Generic;
using System.Linq;

namespace GameBreaker.Util
{
    public static class Constants
    {
        public const uint DEADGAME = 0xDEAD6A3E;

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
        
        public static readonly char[] FORM_C = FORM.ToCharArray();
        public static readonly char[] GEN8_C = GEN8.ToCharArray();
        public static readonly char[] OPTN_C = OPTN.ToCharArray();
        public static readonly char[] LANG_C = LANG.ToCharArray();
        public static readonly char[] EXTN_C = EXTN.ToCharArray();
        public static readonly char[] STRG_C = STRG.ToCharArray();
        public static readonly char[] TPAG_C = TPAG.ToCharArray();
        public static readonly char[] TXTR_C = TXTR.ToCharArray();
        public static readonly char[] AGRP_C = AGRP.ToCharArray();
        public static readonly char[] AUDO_C = AUDO.ToCharArray();
        public static readonly char[] SOND_C = SOND.ToCharArray();
        public static readonly char[] BGND_C = BGND.ToCharArray();
        public static readonly char[] PATH_C = PATH.ToCharArray();
        public static readonly char[] EMBI_C = EMBI.ToCharArray();
        public static readonly char[] DAFL_C = DAFL.ToCharArray();
        public static readonly char[] TGIN_C = TGIN.ToCharArray();
        public static readonly char[] FONT_C = FONT.ToCharArray();
        public static readonly char[] SPRT_C = SPRT.ToCharArray();
        public static readonly char[] ACRV_C = ACRV.ToCharArray();
        public static readonly char[] FUNC_C = FUNC.ToCharArray();
        public static readonly char[] VARI_C = VARI.ToCharArray();
        public static readonly char[] SCPT_C = SCPT.ToCharArray();
        public static readonly char[] TAGS_C = TAGS.ToCharArray();
        public static readonly char[] ROOM_C = ROOM.ToCharArray();
        public static readonly char[] OBJT_C = OBJT.ToCharArray();
        public static readonly char[] TMLN_C = TMLN.ToCharArray();
        public static readonly char[] GLOB_C = GLOB.ToCharArray();
        public static readonly char[] GMEN_C = GMEN.ToCharArray();
        public static readonly char[] SHDR_C = SHDR.ToCharArray();
        public static readonly char[] CODE_C = CODE.ToCharArray();
        public static readonly char[] SEQN_C = SEQN.ToCharArray();
        public static readonly char[] FEDS_C = FEDS.ToCharArray();
        public static readonly char[] FEAT_C = FEAT.ToCharArray();
        
        /*public static readonly Dictionary<string, Type> CHUNKS = new()
        {
            {GEN8, typeof(GmChunk.GEN8)},
            {OPTN, typeof(GmChunk.OPTN)},
            {LANG, typeof(GmChunk.LANG)},
            {EXTN, typeof(GmChunk.EXTN)},
            {STRG, typeof(GmChunk.STRG)},
            {TPAG, typeof(GmChunk.TPAG)},
            {TXTR, typeof(GmChunk.TXTR)},
            {AGRP, typeof(GmChunk.AGRP)},
            {AUDO, typeof(GmChunk.AUDO)},
            {SOND, typeof(GmChunk.SOND)},
            {BGND, typeof(GmChunk.BGND)},
            {PATH, typeof(GmChunk.PATH)},
            {EMBI, typeof(GmChunk.EMBI)},
            {DAFL, typeof(GmChunk.DAFL)},
            {TGIN, typeof(GmChunk.TGIN)},
            {FONT, typeof(GmChunk.FONT)},
            {SPRT, typeof(GmChunk.SPRT)},
            {ACRV, typeof(GmChunk.ACRV)},
            {FUNC, typeof(GmChunk.FUNC)},
            {VARI, typeof(GmChunk.VARI)},
            {SCPT, typeof(GmChunk.SCPT)},
            {TAGS, typeof(GmChunk.TAGS)},
            {ROOM, typeof(GmChunk.ROOM)},
            {OBJT, typeof(GmChunk.OBJT)},
            {TMLN, typeof(GmChunk.TMLN)},
            {GLOB, typeof(GmChunk.GLOB)},
            {GMEN, typeof(GmChunk.GMEN)},
            {SHDR, typeof(GmChunk.SHDR)},
            {CODE, typeof(GmChunk.CODE)},
            {SEQN, typeof(GmChunk.SEQN)},
            {FEDS, typeof(GmChunk.FEDS)},
            {FEAT, typeof(GmChunk.FEAT)}
        };*/

        // public static readonly Dictionary<Type, string> CHUNKS_R = CHUNKS.ToDictionary(x => x.Value, x => x.Key);
    }
}