using GameBreaker.GML.AST;
using GameBreaker.GML.Lexer;

namespace GameBreaker.GML.Parser; 

/// <summary>
///     Parser for GML scripts.
/// </summary>
public static class GmlParser {
    public static GmlAst Parse(string script, ParserOptions options) {
        var lexer = new GmlLexer(script, options);
        return default!;
    }
}
