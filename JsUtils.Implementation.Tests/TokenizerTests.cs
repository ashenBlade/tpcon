using System;
using System.Collections.Generic;
using System.Globalization;
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

    [Fact]
    public void Tokenize_WithStringWithOnlyPositiveIntegers_ShouldReturnNumberTokenWithSameValue()
    {
        for (int i = 0; i < 100; i++)
        {
            var number = Random.Shared.Next();
            var expected = new NumberLiteral(number);
            var list = Tokenize(number.ToString());
            Assert.Single(list);
            var actual = list[0];
            Assert.IsType<NumberLiteral>(actual);
            Assert.Equal(expected, (NumberLiteral) actual);
        }
    }

    [Fact]
    public void Tokenize_WithStringWithFloatingPointNumbers_ShouldReturnNumberTokenWithSameValue()
    {
        for (int i = 0; i < 100; i++)
        {
            var floating = Random.Shared.NextDouble() + Random.Shared.Next();
            var expected = new NumberLiteral((decimal) floating);
            var list = Tokenize(floating.ToString(CultureInfo.InvariantCulture));
            Assert.Single(list);
            var actual = list[0];
            Assert.IsType<NumberLiteral>(actual);
            Assert.Equal(expected, actual);
        }
    }
}