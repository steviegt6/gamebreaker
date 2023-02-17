using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;

namespace GameBreaker.GML.LexemeGenerator;

public sealed class LexemeList : List<(string Name, string Value)> { }

/// <summary>
///     Generates lexeme definitions and adds them to dictionaries.
/// </summary>
[Generator]
public sealed class Generator : ISourceGenerator {
    public const string NAMESPACE = "GameBreaker.GML.Lexer.Types";
    public const string TYPE_NAME = "Lexeme";

    private static readonly Dictionary<string, LexemeList> lexemes = new() {
        {
            "Reserved", new LexemeList {
                ("VAR", "var"),
                ("IF", "if"),
                ("END", "end"),
                ("ELSE", "else"),
                ("WHILE", "while"),
                ("DO", "do"),
                ("FOR", "for"),
                ("BEGIN", "begin"),
                ("THEN", "then"),
                ("WITH", "with"),
                ("UNTIL", "until"),
                ("REPEAT", "repeat"),
                ("EXIT", "exit"),
                ("RETURN", "return"),
                ("BREAK", "break"),
                ("CONTINUE", "continue"),
                ("SWITCH", "switch"),
                ("CASE", "case"),
                ("DEFAULT", "default"),
                ("AND", "and"),
                ("OR", "or"),
                ("NOT", "not"),
                ("DIV", "div"),
                ("MOD", "mod"),
                ("XOR", "xor"),
                ("GLOBALVAR", "globalvar"),
                ("ENUM", "enum"),
                ("FUNCTION", "function"),
                ("TRY", "try"),
                ("CATCH", "catch"),
                ("FINALLY", "finally"),
                ("THROW", "throw"),
                ("STATIC", "static"),
                ("NEW", "new"),
                ("DELETE", "delete"),
            }
        }, {
            "PreProcessor", new LexemeList {
                ("PRE_MACRO", "#macro"),
                ("PRE_LINE", "#line"),
                ("PRE_REGION", "#region"),
                ("PRE_ENDREGION", "#endregion"),
            }
        },
    };

    void ISourceGenerator.Initialize(GeneratorInitializationContext context) { }

    void ISourceGenerator.Execute(GeneratorExecutionContext context) {
        foreach (var kvp in lexemes) {
            var source = Generate(kvp.Key, kvp.Value);
            context.AddSource($"{TYPE_NAME}.{kvp.Key}.g.cs", source);
        }
    }

    private static string Generate(string cat, LexemeList lexems) {
        var sb = new StringBuilder();

        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine();
        sb.AppendLine($"namespace {NAMESPACE};");
        sb.AppendLine();
        sb.AppendLine($"partial record struct {TYPE_NAME} {{");

        foreach (var (name, value) in lexems)
            sb.AppendLine(
                $"    public static readonly {TYPE_NAME} {name} = new {TYPE_NAME}(\"{value}\");"
            );

        sb.AppendLine();

        sb.AppendLine(
            $"    public static readonly Dictionary<string, Lexeme> {cat} = new() {{"
        );
        foreach (var (name, value) in lexems)
            sb.AppendLine($"        {{ \"{value}\", {name} }},");
        sb.AppendLine("    };");

        sb.AppendLine("}");

        return sb.ToString();
    }
}
