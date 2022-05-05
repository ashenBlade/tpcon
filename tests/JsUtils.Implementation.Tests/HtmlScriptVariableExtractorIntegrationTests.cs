using System.Collections.Generic;
using System.IO;
using System.Linq;
using JsTypes;
using Xunit;

namespace JsUtils.Implementation.Tests;

public class HtmlScriptVariableExtractorIntegrationTests
{
    private static JsVariable[] ExtractVariables(string html)
    {
        return new HtmlScriptVariableExtractor(new HtmlScriptExtractor(), new ScriptVariableExtractor(new Tokenizer()))
              .ExtractVariables(html)
              .ToArray();
    }

    [Fact]
    public void ExtractVariables_WithEmptyString_ShouldReturnEmptyArray()
    {
        var actual = ExtractVariables(string.Empty);
        Assert.Empty(actual);
    }

    public static IEnumerable<object[]> HtmlsWithoutScripts => new[]
                                                               {
                                                                   new object[]
                                                                   {
                                                                       "<html><head><meta charset=\"utf-8\"></head><body><p>Hello, world</p></body></html>",
                                                                   },
                                                                   new object[]
                                                                   {
                                                                       "<html>\n<head>\n</head>\n<body>\n<p>Hello, world</p>\n</body>\n</html>"
                                                                   },
                                                                   new object[]
                                                                   {
                                                                       "<!DOCTYPE html><html>\n<head><title>This is sample title</title>\n</head>\n<body>\n<p>Hello, world</p>\n</body>\n</html>"
                                                                   },
                                                               };

    [Theory]
    [MemberData(nameof(HtmlsWithoutScripts))]
    public void ExtractVariables_WithoutScriptsInHtml_ShouldReturnEmptyArray(string html)
    {
        var actual = ExtractVariables(html);
        Assert.Empty(actual);
    }

    public static IEnumerable<object[]> ScriptsWithoutAssignements => new[]
                                                                      {
                                                                          new object[]
                                                                          {
                                                                              "\nfunction Plus(x, y) {\n return x + y; \n};\n",
                                                                          },
                                                                          new object[]
                                                                          {
                                                                              "\n\n",
                                                                          },
                                                                          new object[]
                                                                          {
                                                                              "\nconsole.log(date);\n",
                                                                          },
                                                                          new object[]
                                                                          {
                                                                              "return false;",
                                                                          },
                                                                          new object[]
                                                                          {
                                                                              "if (window.location.href === '/') { \nthrow new Error();\n}\n",
                                                                          },
                                                                      };

    [Theory]
    [MemberData(nameof(ScriptsWithoutAssignements))]
    public void ExtractVariables_WithSingleScriptWithoutAssignements_ShouldReturnEmptyArray(string script)
    {
        var html = $"<html><head><script>{script}</script></head><body></body></html>";
        var actual = ExtractVariables(html);
        Assert.Empty(actual);
    } 
}