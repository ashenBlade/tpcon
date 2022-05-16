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

    private List<JsVariable> ExtractVariables(IEnumerable<Token> seq) => ExtractVariables(seq.ToArray());
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

    private JsVariable ExtractSingle(IEnumerable<Token> sequence) => ExtractSingle(sequence.ToArray());

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

    private static IEnumerable<Token> CreateFunction(string name, IEnumerable<string> idNames, IEnumerable<Token> body)
    {
        var list = new List<Token> {Keywords.Function, new Identifier(name), Token.LeftParenthesis};
        var ids = idNames
                 .SelectMany(n => new[] {new Identifier(n), Token.Comma})
                 .ToList();
        if(ids.Count > 1) ids.RemoveAt(ids.Count - 1);
        list.AddRange(ids);
        list.Add(Token.RightParenthesis);
        list.Add(Token.LeftBrace);
        list.AddRange(body);
        list.Add(Token.RightBrace);
        return list;
    }

    private static IEnumerable<Token> ConsoleLog(string log) => new[]
                                                                {
                                                                    new Identifier("console"), Token.Dot, new Identifier("log"),
                                                                    Token.LeftParenthesis, new StringLiteral(log), Token.RightParenthesis,
                                                                    Token.Semicolon
                                                                };

    private static IEnumerable<Token> DocumentGetElementById(string id) => new[]
                                                                           {
                                                                               new Identifier("document"), Token.Dot,
                                                                               new Identifier("getElementById"),
                                                                               Token.LeftParenthesis, new StringLiteral(id),
                                                                               Token.RightParenthesis
                                                                           };

    private static IEnumerable<Token> If(IEnumerable<Token> condition, IEnumerable<Token> body)
        => new[] {Keywords.If, Token.LeftParenthesis}
          .Concat(condition)
          .Concat(new[] {Token.RightParenthesis, Token.LeftBrace})
          .Concat(body)
          .Concat(new[]{Token.RightBrace});

    private static IEnumerable<Token> IfElse(IEnumerable<Token> condition, IEnumerable<Token> ifBody, IEnumerable<Token> elseBody) =>
        If(condition, ifBody)
           .Concat(new[] {Keywords.Else, Token.LeftBrace})
           .Concat(elseBody)
           .Concat(new[] {Token.RightBrace});


    private static IEnumerable<Token> VariableAssignment(string name, params Token[] value)
    {
        yield return Keywords.Var;
        yield return new Identifier(name);
        yield return Token.Equal;
        foreach (var token in value)
        {
            yield return token;
        }

        yield return Token.Semicolon;
    }

    private static IEnumerable<Token> NumberAssignment(string name, decimal value) =>
        VariableAssignment(name, new NumberLiteral(value));

    private static IEnumerable<Token> StringAssignment(string name, string value) =>
        VariableAssignment(name, new StringLiteral(value));

    private static IEnumerable<Token> BoolAssignment(string name, bool value) => 
        VariableAssignment(name, value 
                                     ? BoolLiteral .True 
                                     : BoolLiteral .False);

    private static IEnumerable<Token> ObjectAssignment(string name, string obj, params Literal[] literalTypes)
    {
        var list = new List<Token> {Keywords.New, new Identifier(obj), Token.LeftParenthesis};
        foreach (var literal in literalTypes)
        {
            list.Add(literal);
            list.Add(Token.Comma);
        }

        if (list.Count > 3)
        {
            list.RemoveAt(list.Count - 1);
        }
        list.Add(Token.RightParenthesis);

        return VariableAssignment(name, list.ToArray());
    }

    private static IEnumerable<Token> ArrayAssignment(string name, params Literal[] literalTypes) =>
        ObjectAssignment(name, "Array", literalTypes);


    public static IEnumerable<object[]> SingleFunctionDeclarationsWithoutArguments
        => new[]
           {
               new object[] {CreateFunction("doReturn", Array.Empty<string>(), Array.Empty<Token>()),},
               new object[] 
               {
                   CreateFunction("doRefresh", 
                                  ArraySegment<string>.Empty, 
                                  ConsoleLog("1"))
               },
               
               new object[] 
               {
                   CreateFunction("doRefresh", 
                                  ArraySegment<string>.Empty, 
                                  IfElse(new Token[]
                                         {
                                             new NumberLiteral(1), Word.Equality, new Identifier("i")
                                         }, 
                                         ConsoleLog("1"), 
                                         ConsoleLog("2"))
                                 )
               },
               new object[] 
               {
                   CreateFunction("showModal", 
                                  ArraySegment<string>.Empty, 
                                  DocumentGetElementById("modal").Concat(new []{Token.Dot, new Identifier("style"), Token.Dot, new Identifier("margin"), Token.Equal, new NumberLiteral(100), Token.Semicolon}))
               },
           };

    
    [Theory(DisplayName = "Function declaration; Without arguments and inner assignments; Return empty sequence")]
    [MemberData(nameof(SingleFunctionDeclarationsWithoutArguments))]
    public void ExtractVariables_WithFunctionDeclarationAndEmptyArguments_ShouldNotReturnThem(Token[] sequence)
    {
        var actual = ExtractVariables(sequence);
        Assert.Empty(actual);
    }

    

    public static IEnumerable<object[]> FunctionDeclarationsWithArgumentsWithoutAssignments
        => new[]
           {
               new object[]
               {
                   CreateFunction("checkExists", 
                                  new[]{"name", "age"}, 
                                  
                                  IfElse(new []
                                         {
                                             new Identifier("age"), Token.Less, new NumberLiteral(13), Word.And, 
                                             new Identifier("name"), Token.Equal, new StringLiteral("Bob")
                                         }, 
                                         new[]
                                         {
                                             Keywords.Return, BoolLiteral.True, Token.Semicolon
                                         },
                                         ConsoleLog("user not exists").Concat(new[]
                                                                              {
                                                                                  Keywords.Return, BoolLiteral.False, Token.Semicolon
                                                                              }))
                                 )
               }
           };

    [Theory(DisplayName = "Function declaration; With arguments; Without inner assignment; Return empty sequence")]
    [MemberData(nameof(FunctionDeclarationsWithArgumentsWithoutAssignments))]
    public void FunctionDeclaration_WithArgumentsAndNoAssignments_ReturnEmptySequence(Token[] sequence)
    {
        var actual = ExtractVariables(sequence);
        Assert.Empty(actual);
    }


    public static IEnumerable<object[]> FunctionDeclarationsWithInnerVariablesAssignments =>
        new[]
        {
            new object[]
            {
                CreateFunction("hereAssignment", Array.Empty<string>(),
                               StringAssignment("str", "value").Concat(ConsoleLog("12342")) ),
            },
            new object[]
            {
                CreateFunction("doubleAssignment", Array.Empty<string>(),
                               ConsoleLog("log")
                                  .Concat(StringAssignment("str", "value"))
                                  .Concat(NumberAssignment("num", 1000))),
            },
            new object[]
            {
                CreateFunction("triple", new[]{"someId", "anotherId"},
                               StringAssignment("str", "value")
                                  .Concat(DocumentGetElementById("id"))
                                  .Concat(NumberAssignment("i", 1))
                                  .Concat(ArrayAssignment("array", new NumberLiteral(123), new NumberLiteral(0)))),
            },
        };

    [Theory(DisplayName = "Function declaration; With single variable assignment in it; Returns empty sequence")]
    [MemberData(nameof(FunctionDeclarationsWithInnerVariablesAssignments))]
    public void FunctionDeclaration_SingleInnerAssignment(Token[] sequence)
    {
        var actual = ExtractVariables(sequence);
        Assert.Empty(actual);
    }


    public static IEnumerable<object[]> FunctionDeclarationSingleOuterSeveralInnerDeclarations =>
        new[]
        {
            new object[]
            {
                CreateFunction("someFunction", new[]{"id", "i"}, NumberAssignment("age", 10).Concat(StringAssignment("name", "Bob")))
                   .Concat(NumberAssignment("mustBeReturned", 10)),
                new[]
                {
                    new JsVariable("mustBeReturned", new JsNumber(10))
                }
            },
            new object[]
            {
                NumberAssignment("i", 1)
                   .Concat(NumberAssignment("j", 1))
                   .Concat(CreateFunction("sum", new[]{"a", "b"},
                                          VariableAssignment("s", new Identifier("a"), Token.Plus, new Identifier("b"))
                                             .Concat(new[]{Keywords.Return, new Identifier("s"), Token.Semicolon})
                                         )),
                new JsVariable[]
                {
                    new ("i", new JsNumber(1)),
                    new("j", new JsNumber(1))
                }
            },
            new object[]
            {
                StringAssignment("age", "18")
                   .Concat(CreateFunction("checkAge", new[]{"age"},
                                          ArrayAssignment("arr", new StringLiteral("permitted"), BoolLiteral.False, new NumberLiteral(10))
                                             .Concat(new[]{Keywords.Return, new Identifier("arr"), Token.LeftBracket, new Identifier("age"), Token.RightBracket})
                                         ))
                   .Concat(NumberAssignment("x", 1000)),
                new JsVariable[]
                {
                    new("age", new JsString("18")),
                    new("x", new JsNumber(1000))
                }
            },
        };

    [Theory(DisplayName = "Function declaration; With outer and inner variable declarations; Returns only outer declarations;")]
    [MemberData(nameof(FunctionDeclarationSingleOuterSeveralInnerDeclarations))]
    public void FunctionDeclaration_WithInnerAndSingleOuterDeclarations_ShouldReturnOnlySingleOuterDeclaration(IEnumerable<Token> sequence, IEnumerable<JsVariable> outer)
    {
        var actual = ExtractVariables(sequence.ToArray()).ToHashSet();
        var expected = outer.ToHashSet();
        Assert.True(actual.Count == expected.Count);
        Assert.All(actual, v => expected.Contains(v));
    }

    public static IEnumerable<object[]> DeclareSameNameVariableInFunction =>
        new[]
        {
            new object[]
            {
                NumberAssignment("name", 10)
                   .Concat(CreateFunction("overrider", new[] {"some"}, StringAssignment("name", "not 10"))),
                new JsVariable("name", new JsNumber(10))
            },
            new object[]
            {
                ArrayAssignment("array", new NumberLiteral(10), new StringLiteral("xxx"))
                   .Concat(CreateFunction("overrider", new[] {"some"}, 
                                          NumberAssignment("num", 11)
                                             .Concat(DocumentGetElementById("id").Concat(new[]{Token.Semicolon}))
                                             .Concat(StringAssignment("array", "array")))),
                new JsVariable("array", new JsArray(new JsType[]{new JsNumber(10), new JsString("xxx")}))
            },
        };

    [Theory(DisplayName = "When declaring variable with same name in function; Should not override value")]
    [MemberData(nameof(DeclareSameNameVariableInFunction))]
    public void WhenDeclaringSameVariableInFunction_NotOverride(IEnumerable<Token> sequence, JsVariable expected)
    {
        var actual = ExtractSingle(sequence);
        Assert.Equal(expected, actual);
    }

    private static IEnumerable<Token> SingleStatementIf(IEnumerable<Token> condition, IEnumerable<Token> body)
    {
        yield return Keywords.If;
        yield return Token.LeftParenthesis;
        foreach (var token in condition)
        {
            yield return token;
        }

        yield return Token.RightParenthesis;
        foreach (var token in body)
        {
            yield return token;
        }
    }

    private static IEnumerable<Token> SingleStatementIfElse(IEnumerable<Token> condition,
                                                            IEnumerable<Token> @if,
                                                            IEnumerable<Token> @else)
    {
        yield return Keywords.If;
        yield return Token.LeftParenthesis;
        foreach (var token in condition)
        {
            yield return token;
        }

        yield return Token.RightParenthesis;
        foreach (var token in @if)
        {
            yield return token;
        }

        yield return Keywords.Else;
        foreach (var token in @else)
        {
            yield return token;
        }
    }

    public static IEnumerable<object[]> IfElseInnerNoOuter =>
        new[]
        {
            new object[]
            {
                If(new[] {new NumberLiteral(10), Token.Less, new NumberLiteral(12)},
                   NumberAssignment("x", 1000))
            },
            new object[]
            {
                If(new[] {BoolLiteral.True},
                   NumberAssignment("x", 1000))
            },
            new object[]
            {
                SingleStatementIf(new[] {new NumberLiteral(10), Token.Less, new NumberLiteral(12)},
                                  NumberAssignment("x", 1000))
            },
            new object[]
            {
                SingleStatementIfElse(new[] {new NumberLiteral(10), Token.Less, new NumberLiteral(12)},
                                      NumberAssignment("x", 1000),
                                      StringAssignment("str", "simple string"))
            },
            new object[]
            {
                IfElse(new[] {new NumberLiteral(10), Token.Less, new NumberLiteral(12)},
                       NumberAssignment("x", 1000),
                       StringAssignment("str", "SomeString").Concat(ConsoleLog("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")))
            },
        };

    [Theory(DisplayName = "If/else; With inner assignments; Without outer assignments; Should return empty sequence")]
    [MemberData(nameof(IfElseInnerNoOuter))]
    public void IfElse_WithInnerAssignments_WithoutOuterAssignments(IEnumerable<Token> sequence)
    {
        var actual = ExtractVariables(sequence);
        Assert.Empty(actual);
    }

    public static IEnumerable<object[]> IfElseInnerAndOuter =>
        new[]
        {
            new object[]
            {
                NumberAssignment("x", 10)
                   .Concat(If(new[]{BoolLiteral.True}, NumberAssignment("x", 12))),
                new[]{new JsVariable("x", new JsNumber(10))}
            },
            new object[]
            {
                BoolAssignment("shouldParse", true)
                   .Concat(SingleStatementIf(new[]{BoolLiteral.True}, NumberAssignment("x", 12)))
                   .Concat(ArrayAssignment("dates", new NumberLiteral(10), new NumberLiteral(20), new NumberLiteral(30))),
                new[]
                {
                    new JsVariable("shouldParse", JsBool.True), 
                    new JsVariable("dates", new JsArray(new JsNumber(10), new JsNumber(20), new JsNumber(30)))
                }
            },
            new object[]
            {
                StringAssignment("name", "Tom")
                   .Concat(DocumentGetElementById("id")).Concat(new[]{Token.Semicolon})
                   .Concat(IfElse(new[]{BoolLiteral.True}, NumberAssignment("x", 12), ObjectAssignment("obj", "Person", new StringLiteral("Tom")))),
                new[]{new JsVariable("name", new JsString("Tom"))}
            },
            new object[]
            {
                NumberAssignment("x", 10)
                   .Concat(SingleStatementIfElse(new[]{new Identifier("x"), Token.Greater, new NumberLiteral(10)}, NumberAssignment("x", 12), ConsoleLog("false"))),
                new[]{new JsVariable("x", new JsNumber(10))}
            },
        };

    [Theory(DisplayName = "If/else; With inner and outer assignments; Should return only outer")]
    [MemberData(nameof(IfElseInnerAndOuter))]
    public void IfElse_WithInnerAndOuterAssignments_ShouldReturnOnlyOuter(IEnumerable<Token> seq, IEnumerable<JsVariable> vars)
    {
        var actual = ExtractVariables(seq).ToDictionary(v => v.Name, v => v.Value);
        var expected = vars.ToDictionary(v => v.Name, v => v.Value);
        Assert.Equal(expected.Count, actual.Count);
        foreach (var variable in actual)
        {
            Assert.True(expected.ContainsKey(variable.Key));
            Assert.True(expected[variable.Key].Equals(variable.Value));
        }
    }
}