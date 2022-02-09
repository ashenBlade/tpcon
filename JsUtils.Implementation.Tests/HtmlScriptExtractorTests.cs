using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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

    private string Extract(string html)
    {
        return new HtmlScriptExtractor().ExtractScript(html);
    }
    
    [Fact]
    public void ExtractScript_WithEmptyScriptTag_ShouldReturnEmptyString()
    {
        // Arrange
        const string emptyScriptTag = "<script></script>";
        
        // Act
        var result = Extract(emptyScriptTag);
        
        // Assert
        Assert.NotNull(result);
        Assert.True(string.IsNullOrWhiteSpace(result));
    }

    [Fact]
    public void ExtractScript_WithNoScriptTags_ShouldReturnEmptyString()
    {
        const string noScriptTagsHtml =
            "<html><head><meta charset=\"utf-8\"></head><body><p>Hello, world</p></body></html>";
        var result = Extract(noScriptTagsHtml);
        Assert.NotNull(result);
        Assert.True(string.IsNullOrWhiteSpace(result));
    }

    public static IEnumerable<object[]> SingleScriptHtmls = new[]
                                                            {
                                                                new object[]
                                                                {
                                                                    "<script>var x = 10;</script>", 
                                                                    "var x = 10;"
                                                                },
                                                                new object[]
                                                                {
                                                                    "<script>function DoSomething()\n {\n\tconsole.log(\"Hello, world\");\n }\n</script>",
                                                                    "function DoSomething()\n {\n\tconsole.log(\"Hello, world\");\n }\n"
                                                                },
                                                                new object[]
                                                                {
                                                                    "<script>console.log(\"Hello, world\");</script>",
                                                                    "console.log(\"Hello, world\");"
                                                                }
                                                            };
    
    [Theory]
    [MemberData(nameof(SingleScriptHtmls))]
    public void ExtractScript_WithNonEmptySingleScriptTag_ShouldReturnSameAsInTheString(string html, string expected)
    {
        var actual = Extract(html);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("<html>\n<head>\n</head>\n<body>\n<p>Hello, world</p>\n</body>\n</html>")]
    [InlineData("<p>Hello, world for the second time</p>")]
    public void ExtractScript_WithMultipleNestedTagsAndNoScriptTag_ShouldReturnEmptyString(string html)
    {
        var result = Extract(html);
        Assert.NotNull(result);
        Assert.True(string.IsNullOrWhiteSpace(result));
    }

    [Theory]
    [InlineData("<html>\n<head>\n<script></script><script></script></head>\n<body>\n<p>Hello, world</p>\n</body>\n</html>")]
    [InlineData("<html>\n<head>\n<script>\n\n</script></head>\n<body>\n<p>Hello, world</p>\n</body>\n</html>")]
    public void ExtractScript_WithMultipleNestedTagsAndSomeEmptyScriptTags_ShouldReturnEmptyString(string html)
    {
        var result = Extract(html);
        Assert.NotNull(result);
        Assert.True(string.IsNullOrWhiteSpace(result));
    }

    public static IEnumerable<object[]> NonEmptyScriptTagsHtmls = new[]
                                                                  {
                                                                      new[]
                                                                      {
                                                                          "<html>\n<head>\n<script>var x = 10;\nconsole.log(\"Hello, world\");\n</script></head>\n<body>\n<p>Hello, world</p>\n</body>\n</html>",
                                                                          "var x = 10;\nconsole.log(\"Hello, world\");\n"
                                                                      },
                                                                      new[]
                                                                      {
                                                                          "<html>\n<head><script>\nfunction Plus(x, y) {\n return x + y; \n};\n</script></head>\n<body></body>\n</html>",
                                                                          "\nfunction Plus(x, y) {\n return x + y; \n};\n"
                                                                      }
                                                                  };

    [Theory]
    [MemberData(nameof(NonEmptyScriptTagsHtmls))]
    public void ExtractScript_WithMultipleNestedTagsAndOneNonEmptyScriptTag_ShouldReturnInnerContentOfScriptTag(string html, string expected)
    {
        var actual = Extract(html);
        Assert.NotNull(actual);
        Assert.Equal(expected, actual);
        Assert.Contains(expected, actual);
    }

    public static readonly IEnumerable<object[]> SeparatedScriptsHtmls = new[]
                                                                {
                                                                    new[]
                                                                    {
                                                                        "<html>\n<head><script>\nfunction Plus(x, y) {\n return x + y; \n};\n</script><script>var x = 10;</script></head>\n<body></body>\n</html>",
                                                                        "\nfunction Plus(x, y) {\n return x + y; \n};\n\nvar x = 10;"
                                                                    },
                                                                    new[]
                                                                    {
                                                                        "<html>\n<head><script>console.log('Initialization started');</script></head>\n<body><script>document.write('<p>Hello, world</p>');</script></body></html>",
                                                                        "console.log('Initialization started');\ndocument.write('<p>Hello, world</p>');"
                                                                    }
                                                                };

    [Theory]
    [MemberData(nameof(SeparatedScriptsHtmls))]
    public void ExtractScript_WithMultipleNonEmptyScriptTags_ShouldBeSeparatedInResultStringByNewLine(string html, string expected)
    {
        var actual = Extract(html);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("<script></script><script></script>", "")]
    [InlineData("<script></script><script>let date = Date.now();\nconsole.log(date);\n</script><script></script>", "let date = Date.now();\nconsole.log(date);\n")]
    public void ExtractScript_WithEmptyScriptTags_ShouldNotIncludeThemInResult(string html, string expected)
    {
        var actual = Extract(html);
        Assert.Equal(expected, actual);
    }
}