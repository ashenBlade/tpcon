using System.Reflection.Metadata.Ecma335;
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
        private bool EndOfFile { get; set; }

        public InnerTokenizer(string source, Dictionary<string, Token> reserved)
        {
            _source = source;
            _reserved = reserved;
            _position = -1;
            EndOfFile = false;
        }

        private bool MoveNext()
        {
            if (EndOfFile)
            {
                return false;
            }
            _position++;
            if (_position >= _source.Length)
            {
                EndOfFile = true;
                return false;
            }

            return true;
        }

        public Token? Read()
        {
            while (MoveNext() && SkipWhitespaces() && !EndOfFile)
            {
                return Current switch
                        {
                            _ when char.IsDigit(Current) => ReadNumber(),
                            _ when IsCorrectIdentifierStartLetter(Current) => ReadWord(),
                            _ when IsMathOperator(Current) => ReadOperator(),
                            '\'' or '\"' => ReadStringLiteral(),
                            _ => new Token(Current)
                        };
            }

            return null;
        }

        private Token ReadOperator()
        {
            var current = Current;
            var next = MoveNext()
                           ? Current
                           : '\0';
            switch (current)
            {
                case '+':
                    return next == '+'
                               ? new Word("++", Tags.Increment)
                               : new Token('+');

                case '-':
                    return next == '-'
                               ? new Word("--", Tags.Decrement)
                               : new Token('-');
                case '|':
                    return next == '|'
                               ? new Word("||", Tags.Or)
                               : new Token('|');
                case '&':
                    return next == '&'
                               ? new Word("&&", Tags.And)
                               : new Token('&');
                case '=':
                    if (next == '=')
                    {
                        var nextNext = MoveNext()
                                           ? Current
                                           : '\0';
                        return nextNext == '='
                                   ? new Word("===", Tags.StrongEquality)
                                   : new Word("==", Tags.Equality);
                    }

                    return new Token('=');
                case '<':
                    return next == '='
                               ? new Word("<=", Tags.LessOrEqual)
                               : new Token('<');
                case '>':
                    return next == '='
                               ? new Word(">=", Tags.GreaterOrEqual)
                               : new Token('>');
            }

            return new Token(current);
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

        private bool CanBeUsedInIdentifierWord(char letter)
        {
            return char.IsLetterOrDigit(letter) || letter is '$' or '_';
        }

        private bool IsCorrectIdentifierStartLetter(char letter)
        {
            return char.IsLetter(letter) || letter is '_' or '$';
        }

        private bool IsMathOperator(char letter)
        {
            return letter is '|' or '&' or '+' or '-' or '!' or '=' or '^' or '<' or '>';
        }

        private Token ReadWord()
        {
            var builder = new StringBuilder();
            if (!IsCorrectIdentifierStartLetter(Current))
            {
                throw new UnexpectedTokenException(_source, _position, $"Expected letter. Got: {Current}");
            }

            builder.Append(Current);
            while (MoveNext() && CanBeUsedInIdentifierWord(Current))
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
            var builder = new StringBuilder();
            if (!char.IsDigit(Current))
            {
                throw new UnexpectedTokenException(_source, _position, $"Expected number. Given: {Current}");
            }

            builder.Append(Current);
            while (MoveNext() && char.IsDigit(Current))
            {
                builder.Append(Current);
            }

            
            if (!EndOfFile && Current == '.')
            {
                builder.Append('.');
                while (MoveNext() && char.IsDigit(Current))
                {
                    builder.Append(Current);
                }
            }

            return new NumberLiteral(decimal.Parse(builder.ToString()));
        }
    }

    private static IEnumerable<Word> ReservedWords => new[]
                                                      {
                                                          Keywords.For, Keywords.Break, Keywords.Do, Keywords.While,
                                                          Keywords.Else, Keywords.Function, Keywords.Const,
                                                          Keywords.Let, Keywords.Var, Keywords.Switch, Keywords.Return,
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