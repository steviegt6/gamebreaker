using GameBreaker.GML.Parser;

namespace GameBreaker.GML.Lexer; 

/// <summary>
///     Lexer for GML scripts.
/// </summary>
public sealed class GmlLexer {
    private char[] script;
    private int currIndex;
    private char currChar;
    private char nextChar;
    
    public GmlLexer(string script, ParserOptions options) {
        this.script = script.ToCharArray();
        currIndex = 0;

        currChar = '\0';
        nextChar = '\0';
    }
}
