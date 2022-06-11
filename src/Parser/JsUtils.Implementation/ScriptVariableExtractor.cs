using JsTypes;
using JsUtils.Implementation.Tokens;

namespace JsUtils.Implementation;

public class ScriptVariableExtractor : IJsVariableExtractor
{
    private readonly ITokenizer _tokenizer;

    public ScriptVariableExtractor(ITokenizer tokenizer)
    {
        _tokenizer = tokenizer;
    }

    public IEnumerable<JsVariable> ExtractVariables(string script)
    {
        using var inner = new InnerVariableExtractor(_tokenizer, script);
        while (inner.NextVariable(out var variable))
        {
            yield return variable!;
        }
    }

    private class InnerVariableExtractor : IDisposable
    {
        private readonly IEnumerator<Token> _enumerator;
        private bool SequenceEnded { get; set; }

        private Token Current => _enumerator.Current;

        private bool MoveNext()
        {
            if (SequenceEnded)
            {
                return false;
            }

            SequenceEnded = !_enumerator.MoveNext();
            return !SequenceEnded;
        }

        public InnerVariableExtractor(ITokenizer tokenizer, string source)
        {
            _enumerator = tokenizer.Tokenize(source)
                                   .GetEnumerator();
            Initialize();
        }

        private void Initialize()
        {
            SequenceEnded = !_enumerator.MoveNext();
        }

        public bool NextVariable(out JsVariable? variable)
        {
            variable = null;
            if (SequenceEnded)
            {
                return false;
            }

            var exception = false;
            try
            {
                while (TryReadStatement(out variable)
                    && variable is null)
                {
                }
            }
            catch (UnexpectedTokenException)
            {
                exception = true;
            }

            if (exception)
            {
                return NextVariable(out variable);
            }
            // return NextVariable(out variable);

            return variable is not null;
        }

        private bool TryReadStatement(out JsVariable? variable)
        {
            variable = null;
            if (SequenceEnded)
            {
                return false;
            }

            variable = Current switch
                       {
                           Word w => w.Lexeme switch
                                     {
                                         "var"      => ReadVariableAssignment(),
                                         "function" => ReadFunctionDeclaration(),
                                         "if"       => ReadIfElse(),
                                         _          => ReadSingleStatement(),
                                     },
                           _ => SkipOne()
                       };
            return !SequenceEnded;
        }

        private JsVariable? SkipOne()
        {
            MoveNext();
            return null;
        }

        private JsVariable? ReadSingleStatement()
        {
            while (Current is not {Tag: ';'} && MoveNext())
            {
            }

            if (!SequenceEnded)
            {
                MoveNext();
            }

            return null;
        }


        private JsVariable? ReadFunctionDeclaration()
        {
            ReadWord(Keywords.Function);
            TryReadIdentifier(out _);
            ReadFunctionDeclarationArguments();
            ReadBlock();
            return null;
        }

        private bool TryReadIdentifier(out Identifier? identifier)
        {
            identifier = null;
            if (Current is Identifier id)
            {
                MoveNext();
                identifier = id;
            }

            return identifier is not null;
        }

        private void ReadFunctionDeclarationArguments()
        {
            ReadToken('(');
            while (Current is not {Tag: ')'})
            {
                ReadIdentifier();
                if (Current is {Tag: ','})
                {
                    ReadToken(',');
                }
            }

            ReadToken(')');
        }

        private void ReadBlock()
        {
            if (Current is {Tag: '{'})
            {
                var innerBlocksCount = 1;
                while (MoveNext())
                {
                    switch (Current.Tag)
                    {
                        case '{':
                            innerBlocksCount++;
                            break;
                        case '}':
                            innerBlocksCount--;
                            break;
                    }

                    if (innerBlocksCount == 0)
                    {
                        MoveNext();
                        break;
                    }
                }

                return;
            }

            while (MoveNext())
            {
                if (Current is {Tag: ';'})
                {
                    MoveNext();
                    break;
                }
            }
        }

        private JsVariable? ReadVariableAssignment()
        {
            ReadWord(Keywords.Var);
            var id = ReadIdentifier();
            ReadToken(Token.Equal);
            var value = ReadType();
            ReadToken(';');
            return new JsVariable(id.Lexeme, value);
        }

        private JsType ReadType()
        {
            var type = Current switch
                       {
                           StringLiteral => ReadString(),
                           NumberLiteral => ReadNumber(),
                           BoolLiteral   => ReadBool(),
                           RegexLiteral  => ReadRegex(),
                           Word w => w.Lexeme switch
                                     {
                                         "null"      => ReadNull(),
                                         "undefined" => ReadUndefined(),
                                         "new"       => ReadNewObjectDeclaration(),
                                         "if"        => ReadIfElse(),
                                         _           => ReadIdentifierOrFunctionCall()
                                     },
                           _ => null
                       };
            if (type is null)
            {
                throw new UnexpectedTokenException($"Expected type, but given {Current}", new Token(Tags.Type),
                                                   Current);
            }

            return type;
        }

        private JsRegex ReadRegex()
        {
            if (Current is not RegexLiteral regex)
            {
                throw new UnexpectedTokenException($"Expected regex. Given: {Current}", RegexLiteral.Token, Current);
            }

            return new JsRegex(regex.Value);
        }

        private JsVariable? ReadIfElse()
        {
            ReadWord(Keywords.If);
            ReadBlock();
            if (TryReadWord(Keywords.Else))
            {
                ReadBlock();
            }

            return null;
        }

        private bool TryReadToken(int tag)
        {
            if (Current.Tag == tag)
            {
                MoveNext();
                return true;
            }

            return false;
        }

        private bool TryReadToken(Token token) => TryReadToken(token.Tag);

        private bool TryReadWord(Word w)
        {
            if (!SequenceEnded && Current is Word word && word.Lexeme == w.Lexeme)
            {
                MoveNext();
                return true;
            }

            return false;
        }

        private JsType ReadIdentifierOrFunctionCall()
        {
            var id = ReadIdentifier();
            if (Current is {Tag: '('})
            {
                ReadFunctionArguments();
            }

            // For our purposes we do not want to execute and see what's in it
            // Just read
            return new JsVariable(id.Lexeme, JsUndefined.Instance);
        }

        private JsObject ReadNewObjectDeclaration()
        {
            ReadWord(Keywords.New);
            var id = ReadIdentifier();
            var arguments = ReadFunctionArguments();
            return id.Lexeme is "Array"
                       ? new JsArray(arguments)
                       : new JsObject();
        }

        private List<JsType> ReadFunctionArguments()
        {
            var list = new List<JsType>();
            ReadToken('(');
            while (Current is not {Tag: ')'})
            {
                var type = ReadType();
                if (Current is {Tag: ','})
                {
                    ReadToken(',');
                }

                list.Add(type);
            }

            ReadToken(')');
            return list;
        }

        private JsUndefined ReadUndefined()
        {
            if (Current is Word {Lexeme: "undefined"})
            {
                MoveNext();
                return JsUndefined.Instance;
            }

            throw new UnexpectedTokenException($"Expected \"undefined\", but given: {Current}", Token.Undefined,
                                               Current);
        }

        private JsNull ReadNull()
        {
            if (Current is Word {Lexeme: "null"})
            {
                MoveNext();
                return JsNull.Instance;
            }

            throw new UnexpectedTokenException($"Expected \"null\", but given {Current}", Token.Null, Current);
        }

        private JsBool ReadBool()
        {
            if (Current is BoolLiteral literal)
            {
                MoveNext();
                return literal.Value
                           ? JsBool.True
                           : JsBool.False;
            }

            throw new UnexpectedTokenException($"Expected bool literal, given: {Current}", BoolLiteral.Token, Current);
        }

        private JsNumber ReadNumber()
        {
            if (Current is NumberLiteral literal)
            {
                MoveNext();
                return new JsNumber(literal.Value);
            }

            throw new UnexpectedTokenException($"Expected number literal, given: {Current}", NumberLiteral.Token,
                                               Current);
        }

        private JsString ReadString()
        {
            if (Current is StringLiteral literal)
            {
                MoveNext();
                return new JsString(literal.Value);
            }

            throw new UnexpectedTokenException($"Expected string literal, given: {Current}", StringLiteral.Token,
                                               Current);
        }

        private Token ReadToken(int tag)
        {
            if (Current is { } token && token.Tag == tag)
            {
                MoveNext();
                return token;
            }

            throw new UnexpectedTokenException(new Token(tag), Current);
        }

        private Token ReadToken(Token token)
        {
            if (Current is { } t && t.Tag == token.Tag)
            {
                MoveNext();
                return t;
            }

            throw new UnexpectedTokenException(token, Current);
        }

        private Identifier ReadIdentifier()
        {
            if (Current is Word word)
            {
                MoveNext();
                return new Identifier(word.Lexeme);
            }

            throw new UnexpectedTokenException($"Expected identifier, given: {Current.Tag}", Identifier.Token, Current);
        }

        private Word ReadWord(Word w)
        {
            if (Current is Word word && word.Lexeme == w.Lexeme)
            {
                MoveNext();
                return word;
            }

            throw new UnexpectedTokenException($"Expected \"{w.Lexeme}\", but given {Current}", w, Current);
        }

        public void Dispose()
        {
            _enumerator?.Dispose();
        }
    }
}