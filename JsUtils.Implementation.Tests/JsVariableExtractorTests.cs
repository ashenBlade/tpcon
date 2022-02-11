using System;
using System.Collections.Generic;
using System.Linq;
using JsTypes;
using JsUtils.Implementation.JsTokens;
using Moq;
using Xunit;
using JsType = JsUtils.Implementation.JsTokens.JsType;

namespace JsUtils.Implementation.Tests;

public class JsVariableExtractorTests
{
    private static ITokenizer GetTokenizer(params JsToken[] toReturn)
    {
        var mock = new Mock<ITokenizer>();
        mock.Setup(tokenizer => tokenizer.Tokenize(Moq.It.IsAny<string>())).Returns(toReturn);
        return mock.Object;
    }

    private static IEnumerable<JsVariable> Parse(JsToken[] tokens)
    {
        return new JsVariableExtractor(GetTokenizer(tokens)).ExtractVariables("");
    }

    [Fact]
    public void ParseScript_WithNoTokens_ShouldReturnEmptyScript()
    {
        var result = Parse(Array.Empty<JsToken>());
        Assert.Empty(result);
    }


    public static readonly IEnumerable<object[]> SingleVariableAssignments = new[]
                                                                             {
                                                                                 new JsToken[]
                                                                                 {
                                                                                     new JsIdentifier("x"),
                                                                                     new JsTokens.JsNumber(10)
                                                                                 },
                                                                                 new JsToken[]
                                                                                 {
                                                                                     new JsIdentifier("variable1"),
                                                                                     new JsStringLiteral("a Some string   aa")
                                                                                 },
                                                                                 new JsToken[]
                                                                                 {
                                                                                     new JsIdentifier("Nopen"),
                                                                                     new JsTokens.JsRegex("regex")
                                                                                 },
                                                                                 new JsToken[]
                                                                                 {
                                                                                     new JsIdentifier("anotherId"),
                                                                                     new JsTokens.JsNumber(123123)
                                                                                 }
                                                                             };

    [Theory]
    [MemberData(nameof(SingleVariableAssignments))]
    public void ExtractVariables_WithSingleVariableDeclarationAndAssignmentOfBasicLiteralType_ShouldReturnOneVariable(JsIdentifier identifier, JsType type)
    {
        var actual = Parse(new JsToken[] {new JsVar(), identifier, new JsEquals(), type, new JsSemicolon()}).ToList();
        Assert.Single(actual);
        var variable = actual[0];
        Assert.Equal(identifier.Name, variable.Name);
        Assert.Equal(type.ToJsType(), variable.Value);
    }

    [Fact]
    public void ExtractVariables_WithVariableDeclarationToNewObjectWithoutConstructorParameters_ShouldReturnSingleVariableWithEmptyObject()
    {
        var id = new JsIdentifier("name");
        var actual = Parse(new JsToken[]
                           {
                               new JsVar(), id, new JsEquals(), 
                               new JsNew(), new JsIdentifier("Object"),
                               new JsLeftParenthesis(), new JsRightParenthesis(), 
                               new JsSemicolon()
                           })
           .ToList();
        Assert.Single(actual);
        var variable = actual[0];
        Assert.Equal(id.Name, variable.Name);
        Assert.True(variable.Value is JsObject);
        Assert.Empty((variable.Value as JsObject)!);
    }
}