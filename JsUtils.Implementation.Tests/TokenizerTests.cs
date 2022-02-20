using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
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

    [Theory]
    [InlineData(@"", "")]
    [InlineData(@"value", "value")]
    [InlineData(@"\""", "\"")]
    [InlineData(@"\\", "\\")]
    [InlineData(@"//", "//")]
    [InlineData(@"aaaaaaaa", "aaaaaaaa")]
    [InlineData(@"a   aa   aaa   aa", "a   aa   aaa   aa")]
    [InlineData(@"a2222 43445.. //  aa", "a2222 43445.. //  aa")]
    [InlineData(@"\\\\", @"\\")]
    public void Tokenize_WithStringLiteral_ShouldReturnStringLiteralWithValueEqualsStringInQuotes(string literal, string value)
    {
        AssertStringLiteral(@$"""{literal}""", value);
        AssertStringLiteral($@"'{literal}'", value);
    }

    private void AssertStringLiteral(string literal, string value)
    {
        var list = Tokenize(literal);
        Assert.Single(list);
        var actual = list[0];
        Assert.IsType<StringLiteral>(actual);
        var expected = new StringLiteral(value);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("true", true)]
    [InlineData("false", false)]
    public void Tokenize_WithBoolLiteral_ShouldReturnBoolLiteral(string literal, bool value)
    {
        var actual = TokenizeSingle<BoolLiteral>(literal);
        var expected = new BoolLiteral(value);
        Assert.Equal(expected, actual);
    }

    private T TokenizeSingle<T>(string source) where T : Token
    {
        var list = Tokenize(source);
        Assert.Single(list);
        var first = list[0];
        Assert.IsType<T>(first);
        return (T)first;
    }

    public static readonly IEnumerable<object[]> KeywordsSequence = new[]
                                                                    {
                                                                        new object[] { Keywords.For.Lexeme, Keywords.For },
                                                                        new object[] { Keywords.While.Lexeme, Keywords.While },
                                                                        new object[] { Keywords.Do.Lexeme, Keywords.Do },
                                                                        new object[] { Keywords.Else.Lexeme, Keywords.Else },
                                                                        new object[] { Keywords.Case.Lexeme, Keywords.Case },
                                                                        new object[] { Keywords.Switch.Lexeme, Keywords.Switch },
                                                                        new object[] { Keywords.Break.Lexeme, Keywords.Break },
                                                                        new object[] { Keywords.Function.Lexeme, Keywords.Function },
                                                                        new object[] { Keywords.Return.Lexeme, Keywords.Return }
                                                                    };

    [Theory]
    [MemberData(nameof(KeywordsSequence))]
    public void Tokenize_WithJavascriptKeywords_ShouldReturnTokenWithSpecialTags(string source, Word expected)
    {
        var actual = TokenizeSingle<Word>(source);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("a")]
    [InlineData("x")]
    [InlineData("data")]
    [InlineData("d23243")]
    [InlineData("Var")]
    [InlineData("Let")]
    [InlineData("someId")]
    [InlineData("date")]
    [InlineData("identi1")]
    [InlineData("ii")]
    [InlineData("i1")]
    [InlineData("j")]
    [InlineData("token")]
    [InlineData("func")]
    [InlineData("case_empty")]
    [InlineData("not_equal")]
    [InlineData("$value")]
    [InlineData("_privateFiled")]
    [InlineData("_")]
    [InlineData("aaaa1$")]
    [InlineData("adf23203_____43243dfdf")]
    public void Tokenize_WithSingleValidNonReservedWord_ShouldReturnIdentifier(string id)
    {
        var actual = TokenizeSingle<Identifier>(id);
        var expected = new Identifier(id);
        Assert.Equal(expected, actual);
    }
    

    [Theory]
    [InlineData(">=", Tags.GreaterOrEqual)]
    [InlineData("<=", Tags.LessOrEqual)]
    [InlineData(">", '>')]
    [InlineData("<", '<')]
    [InlineData("=", '=')]
    [InlineData("==", Tags.Equality)]
    [InlineData("===", Tags.StrongEquality)]
    [InlineData("&&", Tags.And)]
    [InlineData("&", '&')]
    [InlineData("||", Tags.Or)]
    [InlineData("|", '|')]
    [InlineData("+", '+')]
    [InlineData("++", Tags.Increment)]
    [InlineData("-", '-')]
    [InlineData("--", Tags.Decrement)]
    [InlineData("!", '!')]
    [InlineData("^", '^')]
    public void Tokenize_WithMathOperationsSigns_ShouldReturnCorrectWords(string @operator, int expectedTag)
    {
        var expected = new Word(@operator, expectedTag);
        var list = Tokenize(@operator);
        Assert.Single(list);
        var actual = list[0];
        Assert.Equal(expected, actual);
    }
}