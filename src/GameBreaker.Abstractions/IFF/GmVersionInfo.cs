// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.
// Copyright (c) colinator27. Licensed under the MIT License.
// See the LICENSE-DogScepter file in the repository root for full terms and conditions.

namespace GameBreaker.Abstractions.IFF;

public class GmVersionInfo
{
    /// <summary>
    ///     The major version of the IDE a Gamemaker data file was built with.
    /// </summary>
    /// <remarks>
    ///     This is only an approximation, as some IDE versions do not increase this number.
    /// </remarks>
    public int Major { get; private set; } = 1;

    /// <summary>
    ///     The minor version of the IDE a Gamemaker data file was built with.
    /// </summary>
    /// <remarks>
    ///     This is only an approximation, as some IDE versions do not increase this number.
    /// </remarks>
    public int Minor { get; private set; }

    /// <summary>
    ///     The release version of the IDE a Gamemaker data file was built with.
    /// </summary>
    /// <remarks>
    ///     This is only an approximation, as some IDE versions do not increase this number.
    /// </remarks>
    public int Release { get; private set; }

    /// <summary>
    ///     The build version of the IDE a Gamemaker data file was built with.
    /// </summary>
    /// <remarks>
    ///     This is only an approximation, as some IDE versions do not increase this number.
    /// </remarks>
    public int Build { get; private set; }

    /// <summary>
    ///     Indicates the bytecode format of the GameMaker data file.
    /// </summary>
    /// <remarks>
    ///     This is only an approximation, as some IDE versions do not increase this number. <br />
    ///     For example, this has a value of <c>17</c> from version <c>2.2.1</c> to since at least <c>2022.3</c>.
    /// </remarks>
    public byte FormatID { get; set; } = 0;

    public bool AlignChunksTo16 { get; set; } = true;

    public bool AlignStringsTo4 { get; set; } = true;

    public bool AlignBackgroundsTo8 { get; set; } = true;

    public bool RoomObjectPreCreate { get; set; } = false;

    /// <summary>
    ///     Whether some unknown variables in the <c>VARI</c> chunk have different values. <br />
    ///     Only used if <see cref="FormatID"/> is great than or equal to <c>14</c>.
    /// </summary>
    // /// <remarks><see cref="GMChunkVARI.VarCount1"/> and <see cref="GMChunkVARI.VarCount2"/> seem to be separate
    // /// fields for instance and global variables which when this is <see langword="false"/> end up being the same.</remarks>
    public bool DifferentVarCounts { get; set; } = false;

    /// <summary>
    ///     Whether the GameMaker data file uses option flags in the <c>OPTN</c> chunk.
    /// </summary>
    public bool OptionBitflag { get; set; } = true;

    /// <summary>
    ///     Indicates whether this GameMaker data file was run from the IDE.
    /// </summary>
    public bool RunFromIDE { get; set; } = false;

    /// <summary>
    ///     Whether the VM bytecode short-circuits logical and/or operations.
    /// </summary>
    public bool ShortCircuit { get; set; } = true;

    /// <summary>
    ///     The ID of the main data file's audio group.
    /// </summary>
    public int BuiltinAudioGroupID => Major >= 2 || (Major == 1 && Build is >= 1354 or >= 161 and < 1000) ? 0 : 1;

    /// <summary>
    ///     Only sets the the <see cref="Major"/>, <see cref="Minor"/>, <see cref="Release"/>, and <see cref="Build"/> properties if their respective parameter values are higher.
    /// </summary>
    /// <param name="major"></param>
    /// <param name="minor"></param>
    /// <param name="release"></param>
    /// <param name="build"></param>
    public void SetVersion(int major = 1, int minor = 0, int release = 0, int build = 0) {
        if (Major < major) {
            Major = major;
            Minor = minor;
            Release = release;
            Build = build;
            return;
        }

        if (Major > major) return;

        if (Minor < minor) {
            Minor = minor;
            Release = release;
            Build = build;
            return;
        }

        if (Minor > minor) return;

        if (Release < release) {
            Release = release;
            Build = build;
            return;
        }

        if (Release > release) return;

        if (Build < build) Build = build;
    }

    /// <summary>
    ///     Returns whether the version number is greater than or equal to the specified paramters.
    /// </summary>
    /// <param name="major"></param>
    /// <param name="minor"></param>
    /// <param name="release"></param>
    /// <param name="build"></param>
    /// <returns></returns>
    public bool IsVersionAtLeast(int major = 0, int minor = 0, int release = 0, int build = 0) {
        if (Major != major) return Major > major;
        if (Minor != minor) return Minor > minor;
        if (Release != release) return Release > release;
        if (Build != build) return Build > build;
        return true;
    }
}