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

    [Theory]
    
    [InlineData("asdf 123", 2)]
    [InlineData("asdf 123 dfdf", 3)]
    [InlineData("&& && &&", 3)]
    [InlineData("&& x 1", 3)]
    [InlineData("&& * | ()", 5)]
    [InlineData("&& * || ()", 5)]
    [InlineData("&& * -97343 ()", 6)]
    [InlineData("&& * - -", 4)]
    [InlineData("/ + -", 3)]
    [InlineData("++ ++ __variable ++", 4)]
    [InlineData("++ $ __variable |", 4)]
    [InlineData("++ var let const __variable |", 6)]
    [InlineData("( ) ( ) ( ) ( ) { } { }", 12)]
    [InlineData("( - ) ( + ) ( / ) ( . ) { . } { . }", 18)]
    [InlineData("( - ) ( ( + ) ( function ) ) ( identifier_name ) { 776545678.2323 } { ++ }", 20)]
    [InlineData("function printHelloWorld() { console.log(\"Hello, world\"); } ", 13)]
    public void Tokenize_WithStringWithMultipleWords_ShouldReturnTokenSequenceWithExpectedCount(string input, int expectedCount)
    {
        var list = Tokenize(input);
        Assert.Equal(expectedCount, list.Count);
    }

    public static readonly IEnumerable<object[]> StringAndTokenSequence = new[]
                                                                                     {
                                                                                         new object[]
                                                                                         {
                                                                                             "12 12 12",
                                                                                             new List<Token>()
                                                                                             {
                                                                                                 new NumberLiteral(12),
                                                                                                 new NumberLiteral(12),
                                                                                                 new NumberLiteral(12)
                                                                                             }
                                                                                         },
                                                                                         new object[]
                                                                                         {
                                                                                             "x y $ const",
                                                                                             new List<Token>()
                                                                                             {
                                                                                                 new Identifier("x"),
                                                                                                 new Identifier("y"),
                                                                                                 new Identifier("$"),
                                                                                                 Keywords.Const
                                                                                             }
                                                                                         },
                                                                                         new object[]
                                                                                         {
                                                                                             "123.323 , function variable var",
                                                                                             new List<Token>()
                                                                                             {
                                                                                                 new NumberLiteral(123.323M),
                                                                                                 new Token(','),
                                                                                                 Keywords.Function,
                                                                                                 new Identifier("variable"),
                                                                                                 Keywords.Var
                                                                                             }
                                                                                         },
                                                                                         new object[]
                                                                                         {
                                                                                             "\"this is a string\" 'another string' _this_is_variable$$ {}",
                                                                                             new List<Token>()
                                                                                             {
                                                                                                 new StringLiteral("this is a string"),
                                                                                                 new StringLiteral("another string"),
                                                                                                 new Identifier("_this_is_variable$$"),
                                                                                                 new Token('{'),
                                                                                                 new Token('}')
                                                                                             }
                                                                                         },
                                                                                         new object[]
                                                                                         {
                                                                                             "new Date(0);",
                                                                                             new List<Token>()
                                                                                             {
                                                                                                 Keywords.New,
                                                                                                 new Identifier("Date"),
                                                                                                 Token.LeftParenthesis,
                                                                                                 new NumberLiteral(0),
                                                                                                 Token.RightParenthesis,
                                                                                                 Token.Semicolon
                                                                                             }
                                                                                         },
                                                                                         new object[]
                                                                                         {
                                                                                             "function doSubmit(){\nreturn 0;\n}\n",
                                                                                             new List<Token>()
                                                                                             {
                                                                                                 Keywords.Function, new Identifier("doSubmit"), Token.LeftParenthesis, Token.RightParenthesis,
                                                                                                 Token.LeftBrace, 
                                                                                                 Keywords.Return, new NumberLiteral(0), Token.Semicolon, 
                                                                                                 Token.RightBrace
                                                                                             }
                                                                                         },
                                                                                         new object[]
                                                                                         {
                                                                                             "var x = [0,\t 1,\t 2,\n 3\n, 4, 5, 6, 7];",
                                                                                             new List<Token>()
                                                                                             {
                                                                                                 Keywords.Var, new Identifier("x"), Token.Equal, Token.LeftBracket,
                                                                                                 new NumberLiteral(0), Token.Comma,
                                                                                                 new NumberLiteral(1), Token.Comma,
                                                                                                 new NumberLiteral(2), Token.Comma,
                                                                                                 new NumberLiteral(3), Token.Comma,
                                                                                                 new NumberLiteral(4), Token.Comma,
                                                                                                 new NumberLiteral(5), Token.Comma,
                                                                                                 new NumberLiteral(6), Token.Comma,
                                                                                                 new NumberLiteral(7), Token.RightBracket, Token.Semicolon
                                                                                             }
                                                                                         },
                                                                                         new object[]
                                                                                         {
                                                                                             "function x() {\n\tconsole.log(\"Hello, world\");\n\treturn undefined; \n}\n",
                                                                                             new List<Token>()
                                                                                             {
                                                                                                 Keywords.Function, new Identifier("x"), Token.LeftParenthesis, Token.RightParenthesis, Token.LeftBrace,
                                                                                                 new Identifier("console"), Token.Dot, new Identifier("log"), Token.LeftParenthesis, new StringLiteral("Hello, world"), Token.RightParenthesis, Token.Semicolon,
                                                                                                 Keywords.Return, Keywords.Undefined, Token.Semicolon,
                                                                                                 Token.RightBrace
                                                                                             }
                                                                                         },
                                                                                         new object[]
                                                                                         {
                                                                                             "var array = new Array(\"string\", 0.0, 111, \"router param\", \"\\\"\", 23232, null);",
                                                                                             new List<Token>()
                                                                                             {
                                                                                                 Keywords.Var, new Identifier("array"), Token.Equal, 
                                                                                                 Keywords.New, new Identifier("Array"), Token.LeftParenthesis,
                                                                                                 new StringLiteral("string"), Token.Comma, 
                                                                                                 new NumberLiteral(0.0M), Token.Comma, 
                                                                                                 new NumberLiteral(111), Token.Comma, 
                                                                                                 new StringLiteral("router param"), Token.Comma, 
                                                                                                 new StringLiteral("\""), Token.Comma, 
                                                                                                 new NumberLiteral(23232), Token.Comma, 
                                                                                                 Keywords.Null, Token.RightParenthesis, Token.Semicolon
                                                                                             }
                                                                                         }
                                                                                     };
    

    [Theory]
    [MemberData(nameof(StringAndTokenSequence))]
    public void Tokenize_WithStringWithMultipleDifferentTokens_ShouldReturnExpectedTokenSequence(string input, List<Token> expected)
    {
        var actual = Tokenize(input);
        Assert.Equal(actual, expected);
    }
}