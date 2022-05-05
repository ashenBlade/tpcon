using System.Runtime.CompilerServices;
using System.Xml;
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
    
    private class InnerVariableExtractor: IDisposable
    {
        private readonly IEnumerator<Token> _enumerator;
        private bool _sequenceEnded;
        private bool SequenceEnded => _sequenceEnded;
        private bool MoveNext() {
            if (_enumerator.MoveNext())
            {
                return true;
            }

            _sequenceEnded = true;
            return false;
        }
        
        private Token Current => _enumerator.Current;

        private Token ReadCurrent()
        {
            var current = Current;
            MoveNext();
            return current;
        }
        
        public InnerVariableExtractor(ITokenizer tokenizer, string source)
        { 
            _enumerator = tokenizer.Tokenize(source)
                                   .GetEnumerator();
            _sequenceEnded = !_enumerator.MoveNext();
        }

        public bool NextVariable(out JsVariable? variable)
        {
            variable = null;
            if (SequenceEnded)
            {
                return false;
            }

            while (Current.Tag != Tags.Var && MoveNext())
            {
                // Skip until sequence end or found 'var' keyword
            }

            if (SequenceEnded)
            {
                return false;
            }
            // Found var tag
            try
            {
                variable = ReadVariable();
                return true;
            }
            // Skip every strange assignment
            catch (UnexpectedTokenException)
            {
                return NextVariable(out variable);
            }
        }

        private JsVariable ReadVariable()
        { 
            ReadTag(Tags.Var);
            var id = ReadIdentifier();
            ReadTag('=');
            var type = ReadType();
            ReadTag(';');
            return new JsVariable(id.Lexeme, type);
        }

        private JsType ReadType()
        {
            return Current switch
                   {
                       NumberLiteral _ => ReadNumber(),
                       StringLiteral _ => ReadString(),
                       BoolLiteral _   => ReadBool(),
                       Word w          => w.Lexeme switch
                                          {
                                              "undefined" => ReadUndefined(),
                                              "null" => ReadNull(),
                                              _ => ReadObjectDeclaration(),
                                          },
                       _ => throw new UnexpectedTokenException("Expected literal or word")
                   };
        }

        private JsUndefined ReadUndefined()
        {
            if (Current is not Word {Lexeme: "undefined"})
            {
                throw new UnexpectedTokenException($"Expected \"undefined\". Got: {Current}");
            }

            MoveNext();
            return JsUndefined.Instance;
        }

        private JsNull ReadNull()
        {
            if (Current is not Word {Lexeme: "null"})
            {
                throw new UnexpectedTokenException($"Expected \"null\". Got: {Current}");
            }

            MoveNext();
            return JsNull.Instance;
        }

        private JsObject ReadObjectDeclaration()
        {
            TryReadToken(Tags.New);
            ReadIdentifier();
            ReadTag('(');
            if (Current.Tag != ')')
            {
                ReadType();
                while (Current.Tag == ',')
                {
                    ReadTag(',');
                    ReadType();
                }
            }
            ReadTag(')');
            return new JsObject();
        }

        private void TryReadToken(int tag)
        {
            if (Current.Tag == tag)
            {
                MoveNext();
            }
        }

        private JsNumber ReadNumber()
        {
            if (Current is NumberLiteral numberLiteral)
            {
                MoveNext();
                return new JsNumber(numberLiteral.Value);
            }

            throw new UnexpectedTokenException($"Expected number. Given: {Current}");
        }

        private JsString ReadString()
        {
            if (Current is StringLiteral stringLiteral)
            {
                MoveNext();
                return new JsString(stringLiteral.Value);
            }

            throw new UnexpectedTokenException($"Expected string. Given: {Current}");
        }

        private JsBool ReadBool()
        {
            if (Current is BoolLiteral boolLiteral)
            {
                MoveNext();
                return new JsBool(boolLiteral.Value);
            }

            throw new UnexpectedTokenException($"Expected bool. Given: {Current}");
        }

        private Identifier ReadIdentifier()
        {
            if (Current.Tag != Tags.Id || Current is not Identifier id)
            {
                throw new UnexpectedTokenException($"Expected identifier. Given: {Current}");
            }

            MoveNext();
            return id;
        }

        private void ReadTag(int expected)
        {
            if (Current.Tag != expected)
            {
                throw new UnexpectedTokenException(Current.Tag, expected);
            }

            MoveNext();
        }

        public void Dispose()
        {
            _enumerator.Dispose();
        }
    }
}