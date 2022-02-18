using System.Text;
using System.Xml.Serialization;
using JsUtils.Implementation.Tokens;

namespace JsUtils.Implementation;

public class Tokenizer : ITokenizer
{
    private class InnerTokenizer
    {
        private readonly string _source;
        private int _position;
        private readonly Dictionary<string, Token> _reserved;
        private int Position => _position;
        private char Current => _source[_position];
        public InnerTokenizer(string source, Dictionary<string, Token> reserved)
        {
            _source = source;
            _reserved = reserved;
            _position = -1;
        }

        private bool MoveNext()
        {
            _position++;
            if (_position >= _source.Length)
            {
                _position--;
                return false;
            }

            return true;
        }
        
        public Token? Read()
        {
            while (MoveNext() && SkipWhitespaces())
            {
                return Current switch
                       {
                           _ when char.IsDigit(Current) => ReadNumber(),
                           _ when char.IsLetter(Current) => ReadWord(),
                           '\'' or '\"' => ReadStringLiteral(),
                           _ => throw new UnexpectedTokenException(_source, _position, "Unknown token")
                       };
            }

            return null;
        }

        private bool SkipWhitespaces()
        {
            bool IsWhitespace(char ch) => ch is ' ' or '\t' or '\n';
            while (true)
            {
                if (!IsWhitespace(Current))
                {
                    break;
                }

                if (!MoveNext())
                {
                    return false;
                }
            }

            return true;
        }

        private Token ReadWord()
        {
            var builder = new StringBuilder();
            if (!char.IsLetter(Current))
            {
                throw new UnexpectedTokenException(_source, _position, $"Expected letter. Got: {Current}");
            }

            builder.Append(Current);
            while (MoveNext() && char.IsLetterOrDigit(Current))
            {
                builder.Append(Current);
            }

            var result = builder.ToString();
            return LookupWords(result);
        }

        private Token LookupWords(string lexeme)
        {
            if (TryGetReserved(lexeme, out var reserved))
            {
                return reserved!;
            }

            return new Identifier(lexeme);
        }

        private bool TryGetReserved(string lexeme, out Token? token)
        {
            token = null;
            return _reserved.TryGetValue(lexeme, out token);
        }

        private StringLiteral ReadStringLiteral()
        {
            var opener = Current;
            if (opener is not ('\'' or '"'))
            {
                throw new UnexpectedTokenException(_source, _position,
                                                   $"Expected \" or \' at start of string literal. Got: {Current}");
            }
            var builder = new StringBuilder();
            var previous = '\0';
            while (MoveNext())
            {
                if (Current == opener)
                {
                    if (previous == '\\')
                    {
                        builder.Append(opener);
                    }
                    else
                    {
                        break;
                    }
                }

                if (Current == '\\')
                {
                    previous = Current;
                    continue;
                }

                previous = Current;
                builder.Append(Current);
            }

            return new StringLiteral(builder.ToString());
        }

        private Token ReadNumber()
        {
            var start = _position;
            var end = _position;
            while (MoveNext() && char.IsDigit(Current))
            {
                end++;
            }

            if (Current == '.')
            {
                end++;
                while (MoveNext() && char.IsDigit(Current))
                {
                    end++;
                }
            }

            return new NumberLiteral(decimal.Parse(_source[start..(end+1)]));
        }
    }

    private static IEnumerable<Word> ReservedWords => new[] {
                                                                Keywords.For,
                                                                Keywords.Break, 
                                                                Keywords.Do, 
                                                                Keywords.While, 
                                                                Keywords.Else,
                                                                Keywords.Function, 
                                                                Keywords.Const, 
                                                                Keywords.Let,
                                                                Keywords.Var, 
                                                                Keywords.Switch, 
                                                                Keywords.Return,
                                                                Keywords.Case
                                                            };

    private Dictionary<string, Token> GetReservedWords()
    {
        var dict = ReservedWords.ToDictionary<Word?, string, Token>(word => word.Lexeme, word => word);
        dict.Add("false", BoolLiteral.False);
        dict.Add("true", BoolLiteral.True);
        return dict;
    }

    public IEnumerable<Token> Tokenize(string source)
    {
        var tokenizer = new InnerTokenizer(source, GetReservedWords());
        while (true)
        {
            var token = tokenizer.Read();
            if (token is null)
            {
                yield break;
            }

            yield return token;
        }
    }
}