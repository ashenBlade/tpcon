using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using JsTypes;
using JsUtils.Implementation.Tokens;
using Moq;
using Xunit;

namespace JsUtils.Implementation.Tests;

public class ScriptVariableExtractorTests
{
    private ScriptVariableExtractor Extractor => new(new Tokenizer());
    private ScriptVariableExtractor GetExtractor(ITokenizer tokenizer) => new(tokenizer);
    private ITokenizer Tokenizer => new Tokenizer();
    private ITokenizer GetTokenizer(params Token[] tokens)
    {
        var mock = new Mock<ITokenizer>();
        mock.Setup(t => t.Tokenize(It.IsAny<string>()))
            .Returns(tokens);
        return mock.Object;
    }

    private ScriptVariableExtractor GetExtractor(params Token[] tokens) => new(GetTokenizer(tokens));
    
    [Fact]
    public void ExtractVariables_WithEmptySequence_ShouldReturnEmptySequence()
    {
        var extractor = GetExtractor();
        var actual = extractor.ExtractVariables(string.Empty).ToList();
        Assert.Empty(actual);
    }

    private JsVariable ExtractSingle(params Token[] tokens)
    {
        var list = GetExtractor(tokens)
                  .ExtractVariables("")
                  .ToList();
        Assert.Single(list);
        return list[0];
    }

    private const string SampleVariableName = "x";

    private Token[] GetLiteralVariableAssigmentSequence(string variableName, Token assignee)
    {
        return new[] {Keywords.Var, new Identifier(variableName), Token.Equal, assignee, Token.Semicolon};
    }

    [Theory]
    [InlineData("x", 14)]
    [InlineData("y", 0)]
    [InlineData("variable", 0.001)]
    [InlineData("gri123_", 2324343)]
    public void ExtractVariables_WithSequenceWithNumberAssignment_ShouldReturnSingleVariable(string variable, decimal value)
    {
        var expected = new JsVariable(variable, new JsNumber(value));
        var actual = ExtractSingle(GetLiteralVariableAssigmentSequence(variable, new NumberLiteral(value)));
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("x", "this is string")]
    [InlineData("x", "")]
    [InlineData("variable", "dasdfhj")]
    [InlineData("dfdsger4543__", "grger   \t\t\t")]
    [InlineData("$ere11", "grger   \t\n")]
    [InlineData("$__", "ff_ef3r43gwy36 56756bee")]
    public void ExtractVariables_WithStringLiteralAssigment_ShouldReturnVariableWithSameStringValue(string variable, string value)
    {
        var expected = new JsVariable(variable, new JsString(value));
        var actual = ExtractSingle(GetLiteralVariableAssigmentSequence(variable, new StringLiteral(value)));
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ExtractVariables_WithBoolLiteralsAssignment_ShouldReturnVariablesWithSameBoolValues(bool value)
    {
        var expected = new JsVariable(SampleVariableName, new JsBool(value));
        var actual = ExtractSingle(GetLiteralVariableAssigmentSequence(SampleVariableName, new BoolLiteral(value)));
        Assert.Equal(expected, actual);
    }
}