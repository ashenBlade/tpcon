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
                                Function = new("function", Tags.Function);
}