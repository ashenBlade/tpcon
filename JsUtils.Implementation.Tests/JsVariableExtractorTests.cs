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
        mock.Setup(tokenizer => tokenizer.Tokenize(It.IsAny<string>())).Returns(toReturn);
        return mock.Object;
    }

    private static IEnumerable<JsVariable> Parse(params JsToken[] tokens)
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
        var actual = Parse(new JsVar(), id, new JsEquals(), new JsNew(), new JsIdentifier("Object"), new JsLeftParenthesis(), new JsRightParenthesis(), new JsSemicolon())
           .ToList();
        Assert.Single(actual);
        var variable = actual[0];
        Assert.Equal(id.Name, variable.Name);
        Assert.True(variable.Value is JsObject);
        Assert.Empty((variable.Value as JsObject)!);
    }

    [Fact]
    public void ExtractVariables_WithEmptyArrayAsObjectDeclaration_ShouldReturnEmptyJsArray()
    {
        // Arrange
        var id = new JsIdentifier("array");
        // Act
        var actual = Parse(new JsVar(), id, new JsEquals(), new JsNew(), new JsIdentifier("Array"),
                           new JsLeftParenthesis(), new JsRightParenthesis(), new JsSemicolon()).ToList();
        
        // Assert
        Assert.Single(actual);
        var variable = actual[0];
        Assert.True(variable.Name == id.Name);
        Assert.True(variable.Value is JsArray);
        
        Assert.True((variable.Value as JsArray)!.Count == 0);
    }

    public static readonly IEnumerable<object[]> JsArrayElements = new[] {
                                                                             new object[]
                                                                             {
                                                                                 "x",
                                                                                 new JsToken[]
                                                                                 {
                                                                                     new JsTokens.JsNumber(12),
                                                                                     new JsStringLiteral("Sample string"),
                                                                                 },
                                                                             },
                                                                             new object[]
                                                                             {
                                                                                 "someId2",
                                                                                 new JsToken[]
                                                                                 {
                                                                                     new JsTokens.JsRegex("dfsdfs"),
                                                                                     new JsTokens.JsNumber(232),
                                                                                     new JsTokens.JsNumber(-2332)
                                                                                 }
                                                                             },
                                                                             new object[]
                                                                             {
                                                                                 "another_id",
                                                                                 new JsToken[]
                                                                                 {
                                                                                     new JsStringLiteral("string")
                                                                                 }
                                                                             }
                                                                         };

    private static IEnumerable<JsToken> GetArrayDeclarationWithGivenLiteralTypes(string variableName, JsType[] parameters)
    {
        yield return new JsVar();
        yield return new JsIdentifier(variableName);
        yield return new JsEquals();
        yield return new JsNew();
        yield return new JsIdentifier("Array");
        yield return new JsLeftParenthesis();
        if (parameters.Length > 0)
        {
            yield return parameters[0];
            for (int i = 1; i < parameters.Length; i++)
            {
                yield return new JsComma();
                yield return parameters[i];
            }
        }

        yield return new JsRightParenthesis();
        yield return new JsSemicolon();
    }

    [Theory]
    [MemberData(nameof(JsArrayElements))]
    public void ExtractVariables_WithNonEmptyArrayDeclaration_ShouldReturnArrayWithExpectedValues(string idName, JsType[] expected)
    {
        var declaration = GetArrayDeclarationWithGivenLiteralTypes(idName, expected);
        var actual = Parse(declaration.ToArray()).ToList();
        Assert.Single(actual);
        var variable = actual[0];
        Assert.True(variable.Name == idName);
        Assert.True(variable.Value is JsArray);
        var array = ( variable.Value as JsArray )!;
        var arrayValues = array.Values.ToList();
        Assert.True(array.Count == expected.Length);
        for (int i = 0; i < array.Count; i++)
        {
            Assert.True(arrayValues[i].Equals(expected[i].ToJsType()));
        }
    }
}