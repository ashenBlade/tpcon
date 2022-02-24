using System.Linq;
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
    
    [Theory]
    [InlineData("x", 14)]
    [InlineData("y", 0)]
    [InlineData("variable", 0.001)]
    [InlineData("gri123_", 2324343)]
    public void ExtractVariables_WithSequenceWithNumberAssignment_ShouldReturnSingleVariable(string variable, decimal value)
    {
        var expected = new JsVariable(variable, new JsNumber(value));
        var actual = ExtractSingle(Keywords.Var, new Identifier(variable), Token.Equal, new NumberLiteral(value),
                                   Token.Semicolon);
        Assert.Equal(expected, actual);
    }
}