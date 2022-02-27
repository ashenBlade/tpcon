using System;
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

    private List<JsVariable> ExtractVariables(Token[] sequence) => GetExtractor(sequence)
                                                                  .ExtractVariables("")
                                                                  .ToList();
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


    private Token[] GetVariableAssignmentTokenSequence(string variable, params Token[] assignment)
    {
        var list = new List<Token> {Keywords.Var, new Identifier(variable), Token.Equal};
        list.AddRange(assignment);
        list.Add(Token.Semicolon);
        return list.ToArray();
    }

    private Token[] GetCustomObjectAssignmentTokenSequence(string variable, string objectId)
    {
        return GetVariableAssignmentTokenSequence(variable, Keywords.New, new Identifier(objectId),
                                                  Token.LeftParenthesis, Token.RightParenthesis);
    }
    
    [Fact]
    public void ExtractVariables_WithObjectAssignment_ShouldReturnVariableWithObjectTypeValue()
    {
        var expected = new JsVariable(SampleVariableName, new JsObject());
        var actual = ExtractSingle(GetCustomObjectAssignmentTokenSequence(SampleVariableName, "Object"));
        Assert.Equal(expected, actual);
    }

    private (JsType, Token) GetRandomJsNumber()
    {
        var value = Random.Shared.Next(0, int.MaxValue);
        return ( new JsNumber(value), new NumberLiteral(value) );
    }

    private (JsType, Token) GetRandomJsString()
    {
        var str = "string";
        return ( new JsString(str), new StringLiteral(str) );
    }

    private (JsType, Token) GetRandomJsBool()
    {
        return Random.Shared.Next(0, 1) == 0
                   ? ( JsBool.True, BoolLiteral.True )
                   : ( JsBool.False, BoolLiteral.False );
    }

    
    private (JsType JsType, Token Token) GetRandomLiteralType()
    {
        return Random.Shared.Next(0, 3) switch
               {
                   0 => GetRandomJsBool(),
                   1 => GetRandomJsNumber(),
                   2 => GetRandomJsString(),
                   _ => GetRandomLiteralType() // Just in case
               };
    }

    private Token[] GetObjectWithConstructorParamsAssignmentSequence(string variable, params Token[] literals)
    {
        var list = new List<Token>() {Keywords.Var, new Identifier(variable), Token.Equal, Keywords.New, new Identifier("Object"), Token.LeftParenthesis};
        if (literals.Length > 0)
        {
            list.Add(literals[0]);
            for (var i = 1; i < literals.Length; i++)
            {
                list.Add(Token.Comma);
                list.Add(literals[i]);
            }
        }
        list.Add(Token.RightParenthesis);
        list.Add(Token.Semicolon);
        return list.ToArray();
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void ExtractVariables_WithObjectWithSeveralLiteralConstructorParamsAssignment_ShouldReturnObject(int paramsCount)
    {
        var expected = new JsVariable(SampleVariableName, new JsObject());
        var ctorParams = Enumerable.Range(0, paramsCount)
                                   .Select(_ => GetRandomLiteralType().Token)
                                   .ToArray();

        var sequence = GetObjectWithConstructorParamsAssignmentSequence(SampleVariableName, ctorParams);
        var actual = ExtractSingle(sequence);
        Assert.Equal(expected, actual);
    }

    private Token[] GetSequenceOfObjectAssignmentToVariable(string variableName, Token[][] ctorParams)
    {
        var sequence = new List<Token>()
                       {
                           Keywords.Var,
                           new Identifier(variableName),
                           Token.Equal,
                           Keywords.New,
                           new Identifier("Object"),
                           Token.LeftParenthesis
                       };
        if (ctorParams.Length > 0)
        {
            sequence.AddRange(ctorParams[0]);
            for (var i = 1; i < ctorParams.Length; i++)
            {
                sequence.Add(Token.Comma);
                sequence.AddRange(ctorParams[i]);
            }
        }
        sequence.Add(Token.RightParenthesis);
        sequence.Add(Token.Semicolon);
        return sequence.ToArray();
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public void ExtractVariables_WithObjectWithSeveralObjectConstructorParamsAssignment_ShouldReadObjectDeclarationAsParameter(int paramsCount)
    {
        var expected = new JsVariable(SampleVariableName, new JsObject());
        var ctorParams = Enumerable.Range(0, paramsCount)
                                   .Select(_ => GetRandomObjectDeclaration())
                                   .ToArray();
        var sequence = GetSequenceOfObjectAssignmentToVariable(SampleVariableName, ctorParams);
        var actual = ExtractSingle(sequence);
        Assert.Equal(expected, actual);
    }

    private Token[] GetRandomObjectDeclaration()
    {
        var paramsCount = Random.Shared.Next(7);
        var literals = Enumerable.Range(0, paramsCount)
                                 .Select(_ => GetRandomLiteralType().Token)
                                 .ToArray();
        var list = new List<Token>() {Keywords.New, new Identifier("Object"), Token.LeftParenthesis};
        if (paramsCount > 0)
        {
            list.Add(literals[0]);
            for (int i = 1; i < literals.Length; i++)
            {
                list.Add(Token.Comma);
                list.Add(literals[i]);
            }
        }
        list.Add(Token.RightParenthesis);
        return list.ToArray();
    }

    public static readonly IEnumerable<object[]> SequencesWithoutVariableAssignments = new[]
                                                                                       {
                                                                                           new object[]
                                                                                           {
                                                                                               new[]
                                                                                               {
                                                                                                   Keywords.Function,
                                                                                                   new
                                                                                                       Identifier("print"),
                                                                                                   Token
                                                                                                      .LeftParenthesis,
                                                                                                   new
                                                                                                       Identifier("msg"),
                                                                                                   Token
                                                                                                      .RightParenthesis,
                                                                                                   Token.LeftBrace, new
                                                                                                       Identifier("console"),
                                                                                                   Token.Dot,
                                                                                                   new
                                                                                                       Identifier("log"),
                                                                                                   Token
                                                                                                      .LeftParenthesis,
                                                                                                   new
                                                                                                       Identifier("msg"),
                                                                                                   Token
                                                                                                      .RightParenthesis,
                                                                                                   Token.Semicolon,
                                                                                                   Token.RightBrace
                                                                                               }
                                                                                           },
                                                                                           new object[]
                                                                                           {
                                                                                               new[]
                                                                                               {
                                                                                                   new Identifier("document"), Token.Dot, new Identifier("getElementById"), Token.LeftParenthesis, new StringLiteral("root"), Token.RightParenthesis, 
                                                                                                   Token.Dot, new Identifier("addEventListener"), Token.LeftParenthesis, new StringLiteral("click"), Token.Comma, new Identifier("e"), Token.Equal, Token.Greater,
                                                                                                   new Identifier("console"), Token.Dot, new Identifier("log"), Token.LeftParenthesis, new StringLiteral("clicked"), Token.RightParenthesis, Token.RightParenthesis, Token.Semicolon
                                                                                               }
                                                                                           },
                                                                                           new object[]
                                                                                           {
                                                                                               new[]
                                                                                               {
                                                                                                   Token.Semicolon, 
                                                                                                   Token.Semicolon, 
                                                                                                   Token.Semicolon, 
                                                                                                   Token.Semicolon, 
                                                                                                   Token.Semicolon
                                                                                               }
                                                                                           },
                                                                                       };

    [Theory]
    [MemberData(nameof(SequencesWithoutVariableAssignments))]
    public void ExtractVariables_WithSequenceWithoutVariableAssignment_ShouldReturnEmptySequence(Token[] sequence)
    {
        var actual = ExtractVariables(sequence);
        Assert.Empty(actual);
    }
}