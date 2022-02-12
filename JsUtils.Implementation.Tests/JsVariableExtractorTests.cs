using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
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

    
    private static IEnumerable<JsToken> GetObjectDeclarationWithGivenLiteralTypes(string objectIdentifier, params JsType[] parameters)
    {
        yield return new JsVar();
        yield return new JsIdentifier(SampleIdentifierName);
        yield return new JsEquals();
        yield return new JsNew();
        yield return new JsIdentifier(objectIdentifier);
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
    
    public static readonly IEnumerable<object[]> JsArrayElements = new[] {
                                                                             new object[]
                                                                             {
                                                                                 new JsToken[]
                                                                                 {
                                                                                     new JsTokens.JsNumber(12),
                                                                                     new JsStringLiteral("Sample string"),
                                                                                 },
                                                                             },
                                                                             new object[]
                                                                             {
                                                                                 new JsToken[]
                                                                                 {
                                                                                     new JsTokens.JsRegex("dfsdfs"),
                                                                                     new JsTokens.JsNumber(232),
                                                                                     new JsTokens.JsNumber(-2332)
                                                                                 }
                                                                             },
                                                                             new object[]
                                                                             {
                                                                                 new JsToken[]
                                                                                 {
                                                                                     new JsStringLiteral("string")
                                                                                 }
                                                                             }
                                                                         };
    
    [Theory]
    [MemberData(nameof(JsArrayElements))]
    public void ExtractVariables_WithNonEmptyArrayDeclaration_ShouldReturnArrayWithExpectedValues(JsType[] expected)
    {
        var declaration = GetObjectDeclarationWithGivenLiteralTypes("Array", expected);
        var actual = Parse(declaration.ToArray()).ToList();
        Assert.Single(actual);
        var variable = actual[0];
        Assert.True(variable.Value is JsArray);
        var array = ( variable.Value as JsArray)!;
        var arrayValues = array.Values.ToList();
        Assert.True(array.Count == expected.Length);
        for (int i = 0; i < array.Count; i++)
        {
            Assert.True(arrayValues[i].Equals(expected[i].ToJsType()));
        }
    }

    private const string SampleIdentifierName = "x";
    
    public static readonly IEnumerable<object[]> ObjectsEnumerable = new[] 
                                                                     {
                                                                         new object[]
                                                                         {
                                                                             Array.Empty<JsType>()
                                                                         },
                                                                         new object[]
                                                                         {
                                                                             new JsType[]
                                                                             {
                                                                                 new JsTokens.JsNumber(12)
                                                                             }
                                                                         },
                                                                         new object[]
                                                                         {
                                                                             new JsType[]
                                                                             {
                                                                                 new JsStringLiteral("1111")
                                                                             }
                                                                         },
                                                                         new object[]
                                                                         {
                                                                             new JsType[]
                                                                             {
                                                                                 new JsTokens.JsRegex("regex")
                                                                             }
                                                                         },
                                                                         new object[]
                                                                         {
                                                                             new JsType[]
                                                                             {
                                                                                 new JsTokens.JsNumber(123),
                                                                                 new JsTokens.JsNumber(1455),
                                                                                 new JsTokens.JsNumber(-67657)
                                                                             }
                                                                         }
                                                                     };
    
    [Theory]
    [MemberData(nameof(ObjectsEnumerable))]
    public void ExtractVariables_WithEmptyObjectVariableDeclaration_ShouldReturnObjectType(JsType[] types)
    {
        var tokens = GetObjectDeclarationWithGivenLiteralTypes("Object", types).ToArray();
        var actual = Parse(tokens).ToList();
        Assert.Single(actual);
        var first = actual[0].Value;
        Assert.True(first is JsObject);
    }

    private static IEnumerable<JsToken> GetEmptyFunction(int parametersCount)
    {
        yield return new JsTokens.JsFunction();
        yield return new JsIdentifier("SampleFunction");
        yield return new JsLeftParenthesis();
        if (parametersCount > 0)
        {
            yield return new JsIdentifier("sampleParam");
            for (int i = 1; i < parametersCount; i++)
            {
                yield return new JsComma();
                yield return new JsIdentifier("sampleParam" + i);
            }
        }

        yield return new JsRightParenthesis();
        yield return new JsLeftBrace();
        yield return new JsRightBrace();
    }

    private static IEnumerable<JsToken> GetSingleVariableDeclaration(JsType type, string varName)
    {
        yield return new JsVar();
        yield return new JsIdentifier(varName);
        yield return new JsEquals();
        yield return type;
        yield return new JsSemicolon();
    }

    public static readonly IEnumerable<object[]> SingleVariableAndFunction = new[] 
                                                                             {
                                                                                 new object[]
                                                                                 {
                                                                                     new JsTokens.JsNumber(12), 0      
                                                                                 },
                                                                                 new object[]
                                                                                 {
                                                                                     new JsStringLiteral("String"), 1
                                                                                  },
                                                                                 new object[]
                                                                                 {
                                                                                     new JsTokens.JsRegex("1212"), 2
                                                                                 }
                                                                             };
    
    [Theory]
    [MemberData(nameof(SingleVariableAndFunction))]
    public void ExtractVariables_WithSingleVariableDeclarationBeforeFunctionDeclaration_ShouldReturnSingleVariable(JsType type, int functionParametersCount)
    {
        var actual = Parse(GetSingleVariableDeclaration(type, SampleIdentifierName)
                          .Concat(GetEmptyFunction(functionParametersCount))
                          .ToArray())
           .ToList();
        Assert.Single(actual);
        var first = actual[0];
        Assert.True(first.Value.Equals(type.ToJsType()));
    }
}