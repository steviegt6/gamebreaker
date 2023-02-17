namespace GameBreaker.GML.Lexer.Types;

/// <summary>
///     A lexeme lexed by the lexer.
/// </summary>
/// <param name="Token">
///     The actual string-represented token (lexeme value).
/// </param>
public readonly partial record struct Lexeme(string Token);
