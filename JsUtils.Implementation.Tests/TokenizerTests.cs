using System.Collections.Generic;
using System.Linq;
using JsUtils.Implementation.Tokens;
using Xunit;

namespace JsUtils.Implementation.Tests;

public class TokenizerTests
{
    private Tokenizer Tokenizer => new Tokenizer();

    private List<Token> Tokenize(string value) => Tokenizer.Tokenize(value).ToList();
    
    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData("  \n  ")]
    [InlineData("  \t\t  ")]
    [InlineData("  \t\n  ")]
    [InlineData("\t\n\t")]
    [InlineData("\n\n\n\n\n")]
    [InlineData("\n    \t\t")]
    public void Tokenize_WithEmptyString_ShouldReturnEmptySequence(string empty)
    {
        var actual = Tokenize(empty);
        Assert.Empty(actual);
    }
}