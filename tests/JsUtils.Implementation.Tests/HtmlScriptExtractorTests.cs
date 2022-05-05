using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace JsUtils.Implementation.Tests;

public class HtmlScriptExtractorTests
{
    [Fact]
    public void CanInstantiate_WithoutProblems()
    {
        var extractor = new HtmlScriptExtractor();
        Assert.True(true);
    }

    private IEnumerable<string> Extract(string html)
    {
        return new HtmlScriptExtractor()
              .ExtractScripts(html);
    }
    
    [Fact]
    public void ExtractScript_WithEmptyScriptTag_ShouldReturnEmptyString()
    {
        const string emptyScriptTag = "<script></script>";
        var actual = Extract(emptyScriptTag);
        Assert.Equal(new [] {string.Empty}, actual);
    }

    [Fact]
    public void ExtractScript_WithNoScriptTags_ShouldReturnEmptySequence()
    {
        const string noScriptTagsHtml =
            "<html><head><meta charset=\"utf-8\"></head><body><p>Hello, world</p></body></html>";
        var actual = Extract(noScriptTagsHtml);
        Assert.Equal(Array.Empty<string>(), actual);
    }

    public static IEnumerable<object[]> SingleScriptHtmls = new[]
                                                            {
                                                                new object[]
                                                                {
                                                                    "<script>var x = 10;</script>", 
                                                                    new[]
                                                                    {
                                                                        "var x = 10;"
                                                                    }
                                                                },
                                                                new object[]
                                                                {
                                                                    "<script>function DoSomething()\n {\n\tconsole.log(\"Hello, world\");\n }\n</script>",
                                                                    new []
                                                                    {
                                                                        "function DoSomething()\n {\n\tconsole.log(\"Hello, world\");\n }\n"
                                                                    }
                                                                },
                                                                new object[]
                                                                {
                                                                    "<script>console.log(\"Hello, world\");</script>",
                                                                    new[]
                                                                    {
                                                                        "console.log(\"Hello, world\");"
                                                                    }
                                                                }
                                                            };
    
    [Theory]
    [MemberData(nameof(SingleScriptHtmls))]
    public void ExtractScript_WithNonEmptySingleScriptTag_ShouldReturnSameAsInTheString(string html, string[] expected)
    {
        var actual = Extract(html);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("<html>\n<head>\n</head>\n<body>\n<p>Hello, world</p>\n</body>\n</html>")]
    [InlineData("<p>Hello, world for the second time</p>")]
    public void ExtractScript_WithMultipleNestedTagsAndNoScriptTag_ShouldReturnEmptyString(string html)
    {
        var scripts = Extract(html);
        Assert.Empty(scripts);
    }

    public static IEnumerable<object[]> MultipleNestedTagsWithWhitespaceScripts = new[]
                                                                                  {
                                                                                      new object[]
                                                                                      {
                                                                                          "<html>\n<head>\n<script></script><script></script></head>\n<body>\n<p>Hello, world</p>\n</body>\n</html>",
                                                                                          new[] {"", ""}
                                                                                      },
                                                                                      new object[]
                                                                                      {
                                                                                          "<html>\n<head>\n<script>\n\n</script></head>\n<body>\n<p>Hello, world</p>\n</body>\n</html>",
                                                                                          new[] {"\n\n"}
                                                                                      }
                                                                                  };

    [Theory]
    [MemberData(nameof(MultipleNestedTagsWithWhitespaceScripts))]
    public void ExtractScript_WithMultipleNestedTagsAndSomeWhiteSpacedOrEmptyScriptTags_ShouldReturnSequenceWithWhitespaceStrings(string html, string[] expected)
    {
        var actual = Extract(html);
        Assert.Equal(expected, actual);
    }

    public static IEnumerable<object[]> NonEmptyScriptTagsHtmls = new[]
                                                                  {
                                                                      new object[]
                                                                      {
                                                                          "<html>\n<head>\n<script>var x = 10;\nconsole.log(\"Hello, world\");\n</script></head>\n<body>\n<p>Hello, world</p>\n</body>\n</html>",
                                                                          new []
                                                                          {
                                                                              "var x = 10;\nconsole.log(\"Hello, world\");\n"
                                                                          }
                                                                      },
                                                                      new object[]
                                                                      {
                                                                          "<html>\n<head><script>\nfunction Plus(x, y) {\n return x + y; \n};\n</script></head>\n<body></body>\n</html>",
                                                                          new []
                                                                          {
                                                                              "\nfunction Plus(x, y) {\n return x + y; \n};\n"
                                                                          }
                                                                      }
                                                                  };

    [Theory]
    [MemberData(nameof(NonEmptyScriptTagsHtmls))]
    public void ExtractScript_WithMultipleNestedTagsAndOneNonEmptyScriptTag_ShouldReturnInnerContentOfScriptTag(
        string html,
        string[] expected)
    {
        var actual = Extract(html).ToHashSet();
        Assert.Equal(expected, actual);
    }
    
    public static IEnumerable<object[]> HtmlWithEmptyAndNonEmptyScriptTags = new[]
                                                                             {
                                                                                 new object[]
                                                                                 {
                                                                                     "", 
                                                                                     Array.Empty<string>()
                                                                                 },
                                                                                 new object[]
                                                                                 {
                                                                                     "<script></script><script></script>",
                                                                                     new[]
                                                                                     {
                                                                                         "", 
                                                                                         ""
                                                                                     }
                                                                                 },
                                                                                 new object[]
                                                                                 {
                                                                                     "<script></script><script>let date = Date.now();\nconsole.log(date);\n</script><script></script>",
                                                                                     new[]
                                                                                     {
                                                                                         "",
                                                                                         "let date = Date.now();\nconsole.log(date);\n",
                                                                                         ""
                                                                                     }
                                                                                 }
                                                                             };

    [Theory]
    [MemberData(nameof(HtmlWithEmptyAndNonEmptyScriptTags))]
    public void ExtractScript_WithEmptyScriptTags_ShouldNotIncludeThemInResult(string html, string[] expected)
    {
        var actual = Extract(html);
        Assert.Equal(expected, actual);
    }

    public static IEnumerable<object[]> UpperCaseWithParametersScripts = new[]
                                                                         {
                                                                             new object[]
                                                                             {
                                                                                 "<SCRIPT language=\"javascript\" type=\"text/javascript\">var statistList = new Array( 188, 205002, 2, 2454, 0,0 );</SCRIPT>",
                                                                                 new []
                                                                                 {
                                                                                     "var statistList = new Array( 188, 205002, 2, 2454, 0,0 );"
                                                                                 }
                                                                             },
                                                                             new object[]
                                                                             {
                                                                                 "<SCRIPT language=\"javascript\" type=\"text/javascript\"> var wanPara = new Array( 4, \"A0-F3-C1-F9-DD-E5\", \"178.204.181.223\", 3, \"255.255.255.255\", 0, 0, \"178.204.181.223\", 0, 1, 0, \"89.232.109.74 , 217.23.177.252\", \"0 day(s) 01:41:41\", 1, 1, \"0.0.0.0\", \"0.0.0.0\", 0, 1, 0, 2, 0, 0,0 ); </SCRIPT>",
                                                                                 new []
                                                                                 {
                                                                                     " var wanPara = new Array( 4, \"A0-F3-C1-F9-DD-E5\", \"178.204.181.223\", 3, \"255.255.255.255\", 0, 0, \"178.204.181.223\", 0, 1, 0, \"89.232.109.74 , 217.23.177.252\", \"0 day(s) 01:41:41\", 1, 1, \"0.0.0.0\", \"0.0.0.0\", 0, 1, 0, 2, 0, 0,0 ); "
                                                                                 }
                                                                             }
                                                                         };
    
    [Theory]
    [MemberData(nameof(UpperCaseWithParametersScripts))]
    public void ExtractScript_WithScriptTagInUpperCaseWithParameters_ShouldReturnContentOfScriptTags(string script, string[] expected)
    {
        var actual = Extract(script);
        Assert.Equal(expected, actual);
    }

    public static IEnumerable<object[]> RawScripts => new[]
                                                      {
                                                          new object[]
                                                          {
                                                              " var wanPara = new Array( 4, \"A0-F3-C1-F9-DD-E5\", \"178.204.181.223\", 3, \"255.255.255.255\", 0, 0, \"178.204.181.223\", 0, 1, 0, \"89.232.109.74 , 217.23.177.252\", \"0 day(s) 01:41:41\", 1, 1, \"0.0.0.0\", \"0.0.0.0\", 0, 1, 0, 2, 0, 0,0 ); ",
                                                          },
                                                          new object[]
                                                          {
                                                              "var statistList = new Array( 188, 205002, 2, 2454, 0,0 );"
                                                          }
                                                      };
}