namespace GameBreaker.GML.Lexer;

public readonly record struct LexerPosition {
    private readonly StringPair? stringRef;

    public string Name => stringRef?.StringOne ?? string.Empty;

    public string Name2 => stringRef?.StringTwo ?? string.Empty;

    public int Line { get; }

    public int Column { get; }

    public int Index { get; }

    public LexerPosition(StringPair? stringRef, int line, int column, int index) {
        this.stringRef = stringRef;
        Line = line;
        Column = column;
        Index = index;
    }
}
