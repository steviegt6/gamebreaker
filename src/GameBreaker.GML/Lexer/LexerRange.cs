namespace GameBreaker.GML.Lexer; 

/// <summary>
///     Describes a range within a script that is lexed.
/// </summary>
public readonly record struct LexerRange(
    LexerPosition Start,
    LexerPosition End
    ) {
    /// <summary>
    ///     Determines whether the given <paramref name="position"/> is located
    ///     within this <see cref="LexerRange"/>.
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns>Whether the position is within this range.</returns>
    public bool Inside(LexerPosition position) {
        // TODO: < or <= for end - oversight or intentional?
        return Start.Index <= position.Index && position.Index < End.Index;
    }
}
