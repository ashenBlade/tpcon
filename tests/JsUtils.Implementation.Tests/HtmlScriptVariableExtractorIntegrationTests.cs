using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        var html = DecorateWithStubHtml(script);
        var actual = ExtractVariables(html);
        Assert.Empty(actual);
    }

    private static string DecorateWithStubHtml(params string[] scripts)
    {
        var builder = new StringBuilder();
        builder.Append("<html><head>");
        foreach (var script in scripts)
        {
            builder.Append("<SCRIPT language=\"javascript\" type=\"text/javascript\">");
            builder.Append(script);
            builder.Append("</SCRIPT>");
        }
        builder.Append("</head><body></body></html>");
        return builder.ToString();
    }

    public static IEnumerable<object[]> ScriptsWithSingleVariableAssignment => new[]
                                                                               {
                                                                                   new object[]
                                                                                   {
                                                                                       "var y = null;",
                                                                                       new JsVariable("y",
                                                                                                      JsNull.Instance)
                                                                                   },
                                                                                   new object[]
                                                                                   {
                                                                                       "var x = 10;",
                                                                                       new JsVariable("x",
                                                                                                      new JsNumber(10))
                                                                                   },
                                                                                   new object[]
                                                                                   {
                                                                                       "var someVar = undefined;",
                                                                                       new JsVariable("someVar",
                                                                                                      JsUndefined.Instance)
                                                                                   },
                                                                                   new object[]
                                                                                   {
                                                                                       "var array = new Array(0, 0, 0);",
                                                                                       new JsVariable("array",
                                                                                                      new JsArray(new[]{new JsNumber(0), new JsNumber(0), new JsNumber(0)}))
                                                                                   },
                                                                                   new object[]
                                                                                   {
                                                                                       "var hello = \"world\";",
                                                                                       new JsVariable("hello",
                                                                                                      new JsString("world"))
                                                                                   },
                                                                                   new object[]
                                                                                   {
                                                                                       "var _private = true;",
                                                                                       new JsVariable("_private",
                                                                                                      JsBool.True)
                                                                                   },
                                                                                   new object[]
                                                                                   {
                                                                                       "var obj = new Object();",
                                                                                       new JsVariable("obj",
                                                                                                      new JsObject())
                                                                                   },
                                                                               };

    [Theory]
    [MemberData(nameof(ScriptsWithSingleVariableAssignment))]
    public void ExtractVariables_WithSingleVariableExtractor_ShouldReturnSingleVariable(string script, JsVariable expected)
    {
        var actual = ExtractVariables(DecorateWithStubHtml(script));
        Assert.Single(actual);
        var variable = actual[0];
        Assert.Equal(expected.Name, variable.Name);
        Assert.Equal(expected.Value, variable.Value);
    }


    public static IEnumerable<object[]> ScriptsWithMultipleAssignments => new[]
                                                                           {
                                                                               new object[]
                                                                               {
                                                                                   "var x = 10;var y = 11;",
                                                                                   new JsVariable[]
                                                                                   {
                                                                                       new("x", new JsNumber(10)),
                                                                                       new("y", new JsNumber(11))
                                                                                   }
                                                                               },
                                                                               new object[]
                                                                               {
                                                                                   "var ages = new Array(17, 20, 11, 14); var name = \"Tom or Bob\";",
                                                                                   new JsVariable[]
                                                                                   {
                                                                                       new("ages",
                                                                                           new JsArray(new[]
                                                                                                       {
                                                                                                           new
                                                                                                               JsNumber(17),
                                                                                                           new
                                                                                                               JsNumber(20),
                                                                                                           new
                                                                                                               JsNumber(11),
                                                                                                           new
                                                                                                               JsNumber(14)
                                                                                                       })),
                                                                                       new("name",
                                                                                           new JsString("Tom or Bob"))
                                                                                   }
                                                                               },
                                                                               new object[]
                                                                               {
                                                                                   "var myNull = null; var thisIsNotNull = new Object();",
                                                                                   new JsVariable[]
                                                                                   {
                                                                                       new("myNull", JsNull.Instance),
                                                                                       new("thisIsNotNull",
                                                                                           new JsObject())
                                                                                   }
                                                                               },
                                                                               new object[]
                                                                               {
                                                                                   "var first = 1; var second = 2 ; var third = 3;",
                                                                                   new JsVariable[]
                                                                                   {
                                                                                       new("first", new JsNumber(1)),
                                                                                       new("second", new JsNumber(2)),
                                                                                       new("third", new JsNumber(3)),
                                                                                   }
                                                                               },
                                                                               new object[]
                                                                               {
                                                                                   "var prev = undefined; var next = true; var empty = ''; var exact = true;",
                                                                                   new JsVariable[]
                                                                                   {
                                                                                       new("prev", JsUndefined.Instance),
                                                                                       new("next", JsBool.True),
                                                                                       new("empty", new JsString("")),
                                                                                       new("exact", JsBool.True),
                                                                                   }
                                                                               },
                                                                               new object[]
                                                                               {
                                                                                   "var firstVar_ = 1; console.log('123'); var secVa$ = null;",
                                                                                   new JsVariable[]
                                                                                   {
                                                                                       new("firstVar_", new JsNumber(1)),
                                                                                       new("secVa$", JsNull.Instance),
                                                                                   }
                                                                               },
                                                                               new object[]
                                                                               {
                                                                                   "(function() {\n this.name = \"Some test name\";\n this.age = 20;\n return this;\n})().name; var u = 12; console.log({age: 20}); var status = new Array(1, 0, \"192.168.0.1\");",
                                                                                   new JsVariable[]
                                                                                   {
                                                                                       new("u", new JsNumber(12)),
                                                                                       new("status", new JsArray(new JsType[]{new JsNumber(1), new JsNumber(0), new JsString("192.168.0.1")})),
                                                                                   }
                                                                               },
                                                                           };

    [Theory]
    [MemberData(nameof(ScriptsWithMultipleAssignments))]
    public void ExtractVariables_WithMultipleVariableAssignments_ShouldReturnRightVariables(string script, JsVariable[] expected)
    {
        var html = DecorateWithStubHtml(script);
        var actual = ExtractVariables(html);
        Assert.Equal(expected, actual);
    }

    public static IEnumerable<object[]> MultipleSingleAssignmentScripts => new[]
                                                                           {
                                                                               new object[]
                                                                               {
                                                                                   new[] {"var x = 10;", "var y = 15;"},
                                                                                   new JsVariable[]
                                                                                   {
                                                                                       new("x", new JsNumber(10)),
                                                                                       new("y", new JsNumber(15)),
                                                                                   }
                                                                               },
                                                                               new object[]
                                                                               {
                                                                                   new[] {"var x = new Array(new Array(), 80, 90, 0, 0, 0);", "var sec = null;", "console.log('done');"},
                                                                                   new JsVariable[]
                                                                                   {
                                                                                       new("x", new JsArray(new JsType[]{new JsArray(), new JsNumber(80), new JsNumber(90), new JsNumber(0), new JsNumber(0), new JsNumber(0)})),
                                                                                       new("sec", JsNull.Instance),
                                                                                   }
                                                                               },
                                                                               new object[]
                                                                               {
                                                                                   new[] {"var wlanPara = new Array(90, 0, 1, \"192.168.0.1\", 11, null);", "var delay = 150000;\n function doReboot() \n{ \nsend(); \n}"},
                                                                                   new JsVariable[]
                                                                                   {
                                                                                       new("wlanPara", new JsArray(new JsNumber(90), new JsNumber(0), new JsNumber(1), new JsString("192.168.0.1"), new JsNumber(11), JsNull.Instance)),
                                                                                       new("delay", new JsNumber(150000)),
                                                                                   }
                                                                               },
                                                                               new object[]
                                                                               {
                                                                                   new[] {"console.log(\"Hello\");", "console.log(\"world\");",  "console.log(\"world\");",  "console.error(\"world\");"},
                                                                                   Array.Empty<JsVariable>()
                                                                               },
                                                                               new object[]
                                                                               {
                                                                                   new[] {"var undef = undefined; sendStatus();", "doUpdate(); var global = new Object();"},
                                                                                   new JsVariable[]
                                                                                   {
                                                                                       new("undef", JsUndefined.Instance),
                                                                                       new("global", new JsObject()),
                                                                                   }
                                                                               },
                                                                           };

    [Theory]
    [MemberData(nameof(MultipleSingleAssignmentScripts))]
    public void ExtractVariables_WithMultipleSingleVariableAssignments_ShouldReturnRightVariables(string[] scripts, JsVariable[] expected)
    {
        var html = DecorateWithStubHtml(scripts);
        var actual = ExtractVariables(html);
        Assert.Equal(expected, actual);
    }
}