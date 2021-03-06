using JsUtils.Implementation.Tokens;

namespace JsUtils.Implementation;

public static class Keywords
{
    public static readonly Word For = new("for", Tags.For),
                                While = new("while", Tags.While),
                                Break = new("break", Tags.Break),
                                Do = new("do", Tags.Do),
                                Case = new("case", Tags.Case),
                                Switch = new("switch", Tags.Switch),
                                Else = new("else", Tags.Else),
                                Function = new("function", Tags.Function),
                                Return = new("return", Tags.Return),
                                Var = new("var", Tags.Var),
                                Let = new("let", Tags.Let),
                                Const = new("const", Tags.Const),
                                New = new("new", Tags.New),
                                Undefined = new("undefined", Tags.Undefined),
                                Null = new("null", Tags.Null),
                                If = new("if", Tags.If);
}