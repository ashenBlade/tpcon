using System.Collections.Generic;
using System.Linq;
using JsTypes;
using Xunit;

namespace JsUtils.Implementation.Tests;

public class JsVariableExtractorTests
{
    private readonly JsVariableExtractor _jsVariableExtractor = new(new Tokenizer());
    private List<JsVariable> Parse(string script) => _jsVariableExtractor.ExtractVariables(script).ToList();

    [Theory]
    [InlineData("")]
    [InlineData("\t\t")]
    [InlineData("\n\n\t")]
    [InlineData("    ")]
    public void ParseScript_WithEmptyOrWhiteSpacedString_ShouldReturnEmptyScript(string whitespace)
    {
        var result = Parse(whitespace);
        Assert.Empty(result);
    }

    [Theory]
    [InlineData("var x = 10;", "x")]
    [InlineData("var date = Date.now();", "date")]
    [InlineData("var value = \"Hello, world!\"", "value")]
    public void ParseScript_WithOnlyOneVariableDeclaration_ShouldReturnScriptWithOnlyOneVariable(string singleVariableDeclarationScriptString, string variableName)
    {
        var variables = Parse(singleVariableDeclarationScriptString);
        Assert.Contains(variables, variable => variable.Name == variableName);
    }
}