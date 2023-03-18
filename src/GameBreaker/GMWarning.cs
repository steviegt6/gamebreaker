/* MIT License
 * 
 * Copyright (c) 2023 Tomat & GameBreaker Contributors
 * Copyright (c) 2020 colinator27
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

namespace GameBreaker;

//TODO: class should get refactored in the future.
/// <summary>
/// Class used for warnings that happen when dealing with <see cref="GMData"/>.
/// </summary>
public class GMWarning
{
    /// <summary>
    /// A message that describes the current warning.
    /// </summary>
    public readonly string Message;
    //TODO: would be nice to get this readonly as well. Also, nothing ever uses this.
    public string File; // Set when not in main WAD/data file

    /// <summary>
    /// The <see cref="WarningLevel"/> of the current warning.
    /// </summary>
    public readonly WarningLevel Level;
    // TODO: value is assigned but not used.
    public readonly WarningKind Kind;

    /// <summary>
    /// An Enum that describes possible levels of a warning.
    /// </summary>
    public enum WarningLevel
    {
        /// <summary>
        /// The warning describes something informational.
        /// </summary>
        Info,

        /// <summary>
        /// The warning describes something bad.
        /// </summary>
        Bad,

        /// <summary>
        /// The warning describes something severe.
        /// </summary>
        Severe
    }

    /// <summary>
    /// An enum that describes possible kinds of a warning.
    /// </summary>
    public enum WarningKind
    {
        /// <summary>
        /// TODO
        /// </summary>
        Unknown,
        /// <summary>
        /// An unknown chunk was encountered.
        /// </summary>
        UnknownChunk
    }

    /// <summary>
    /// Initializes a new <see cref="GMWarning"/> with a custom message, warning level and warning kind.
    /// </summary>
    /// <param name="message">A message that describes the warning.</param>
    /// <param name="level">The <see cref="WarningLevel"/> of the warning.</param>
    /// <param name="kind">The <see cref="WarningKind"/> of the warning.</param>
    public GMWarning(string message, WarningLevel level = WarningLevel.Bad, WarningKind kind = WarningKind.Unknown)
    {
        Message = message;
        Level = level;
        Kind = kind;
    }
}