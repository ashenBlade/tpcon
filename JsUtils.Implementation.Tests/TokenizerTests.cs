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
                                                                 },
                                                                 new object[]
                                                                 {
                                                                     "var var var var",
                                                                     new JsToken[]
                                                                     {
                                                                         new JsVar(),
                                                                         new JsVar(),
                                                                         new JsVar(),
                                                                         new JsVar()
                                                                     }
                                                                 },
                                                             };
    
    [Theory]
    [MemberData(nameof(VariableAssignment))]
    public void Tokenize_WithVariableAssignment_ShouldReturnTokensInSameOrderAsTheyAppear(string script, IEnumerable<JsToken> expected)
    {
        var actual = GetTokens(script).ToList();
        Assert.Equal(expected, actual);
    }

    public static IEnumerable<object[]> NonKeywordsList = new[]
                                                          {
                                                              new object[]
                                                              {
                                                                  "hello world word keyword another",
                                                                  new JsToken[]
                                                                  {
                                                                      new JsIdentifier("hello"),
                                                                      new JsIdentifier("world"),
                                                                      new JsIdentifier("word"),
                                                                      new JsIdentifier("keyword"),
                                                                      new JsIdentifier("another"),
                                                                  }
                                                              },
                                                              new object[]
                                                              {
                                                                  "a b c d e f g h i j       k    \n \t \n ddddd",
                                                                  new JsToken[]
                                                                  {
                                                                      new JsIdentifier("a"), new JsIdentifier("b"),
                                                                      new JsIdentifier("c"), new JsIdentifier("d"),
                                                                      new JsIdentifier("e"), new JsIdentifier("f"),
                                                                      new JsIdentifier("g"), new JsIdentifier("h"),
                                                                      new JsIdentifier("i"), new JsIdentifier("j"),
                                                                      new JsIdentifier("k"), new JsIdentifier("ddddd"),
                                                                  }
                                                              }
                                                          };
    
    [Theory]
    [MemberData(nameof(NonKeywordsList))]
    public void Tokenize_WithNonKeywordsList_ShouldReturnIdentifiersAndNotKeywords(string identifiers, IEnumerable<JsToken> expected)
    {
        var actual = GetTokens(identifiers).ToList();
        Assert.Equal(expected, actual);
        Assert.True(actual.All(token => token is JsIdentifier));
    }

    public static IEnumerable<object[]> Integers = new[]
                                                              {
                                                                  new object[]
                                                                  {
                                                                      "123 999 7444 0 22 90",
                                                                      new JsNumber[]
                                                                      {
                                                                          new(123), new(999), new(7444), new(0),
                                                                          new(22), new(90)
                                                                      }
                                                                  },
                                                                  new object[]
                                                                  {
                                                                      "   2933 \t   12367   45635645 \n\n\n\n452467 987654",
                                                                      new JsNumber[]
                                                                      {
                                                                          new (2933), new(12367), new(45635645), 
                                                                          new(452467), new (987654)
                                                                      }
                                                                  },
                                                                  new object[]
                                                                  {
                                                                      " -222 -0 222 35655 -12323 111 -2323",
                                                                       new JsNumber[]
                                                                       {
                                                                           new (-222), new(0), new (222), new (35655),
                                                                           new (-12323), new (111), new (-2323)
                                                                       }
                                                                  }
                                                              };

    [Theory]
    [MemberData(nameof(Integers))]
    public void Tokenize_WithStringWithIntegers_ShouldReturnValidJsNumbers(
        string numbers,
        IEnumerable<JsNumber> expected)
    {
        var actual = GetTokens(numbers);
        Assert.Equal(expected, actual);
    }

    public static IEnumerable<object[]> FloatingPointNumbers = new[]
                                                               {
                                                                   new object[]
                                                                   {
                                                                       "12.999 20323.0 2324.322 12.344 0.0 -232.032",
                                                                       new JsNumber[]
                                                                       {
                                                                           new(12.999M), new(20323), new(2324.322M),
                                                                           new(12.344M), new(0), new(-232.032M)
                                                                       }
                                                                   },
                                                                   new object[]
                                                                   {
                                                                       "90.22 212. 3242342.22 -11.2002 232.00 0.   23123.9232312",
                                                                       new JsNumber[]
                                                                       {
                                                                           new(90.22M), new(212.0M), new (3242342.22M),
                                                                           new (-11.2002M), new(232.00M), 
                                                                           new(0.0M), new(23123.9232312M)
                                                                       }
                                                                   }
                                                               };

    [Theory]
    [MemberData(nameof(FloatingPointNumbers))]
    public void Tokenize_WithFloatingPointNumbers_ShouldReturnFloatingPointNumbersAsInString(string floatingPointNumbers, JsNumber[] expected)
    {
        var actual = GetTokens(floatingPointNumbers).ToList();
        Assert.Equal(expected, actual);
    }
}