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
}