using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JsUtils.Implementation.JsTokens;
using Xunit;

namespace JsUtils.Implementation.Tests;

public class TokenizerTests
{
    private IEnumerable<JsToken> GetTokens(string source)
    {
        return new Tokenizer().Tokenize(source);
    }

    [Theory]
    [InlineData("")]
    [InlineData("\n\n")]
    [InlineData("\t")]
    [InlineData("    ")]
    public void Tokenize_WithWhitespaceString_ShouldReturnEmptySequence(string empty)
    {
        var actual = GetTokens(empty);
        Assert.Empty(actual);
    }

    public static IEnumerable<object[]> VariableAssignment = new[]
                                                             {
                                                                 new object[]
                                                                 {
                                                                     "var x = 10;",
                                                                     new JsToken[]
                                                                     {
                                                                         new JsVar(), 
                                                                         new JsIdentifier("x"),
                                                                         new JsEquals(), 
                                                                         new JsNumber(10),
                                                                         new JsSemicolon()
                                                                     }
                                                                 }
                                                             };
    
    [Theory]
    [MemberData(nameof(VariableAssignment))]
    public void Action_WithPrerequisites_ShouldBehaviour(string script, IEnumerable<JsToken> expected)
    {
        var actual = GetTokens(script).ToList();
        Assert.Equal(expected, actual);
    }
}