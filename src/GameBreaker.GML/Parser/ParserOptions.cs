namespace GameBreaker.GML.Parser;

/// <summary>
///     Options for <see cref="GmlParser"/>.
/// </summary>
/// <param name="FuncName">The parsed function's name.</param>
/// <param name="Compatible1X">
///     Whether this function being parsed is compatible with 1.x.
/// </param>
/// <param name="IsScript">Whether this function is a script.</param>
/// <param name="ExpandMacros">Whether to expand function macros.</param>
public sealed record ParserOptions(
    string FuncName,
    bool Compatible1X,
    bool IsScript,
    bool ExpandMacros
);
