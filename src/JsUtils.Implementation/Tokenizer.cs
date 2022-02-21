using System.Diagnostics;
using System.Net;
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
        private bool EndOfFile => _position >= _source.Length;

        public InnerTokenizer(string source, Dictionary<string, Token> reserved)
        {
            _source = source;
            _reserved = reserved;
            _position = 0;
        }

        private bool MoveNext()
        {
            if (EndOfFile)
            {
                return false;
            }
            _position++;
            return !EndOfFile;
        }

        public Token? Read()
        {
            while (SkipWhitespaces() && !EndOfFile)
            {
                return Current switch
                        {
                            _ when char.IsDigit(Current) => ReadNumber(),
                            _ when IsCorrectIdentifierStartLetter(Current) => ReadWord(),
                            '\'' or '\"' => ReadStringLiteral(),
                            _ when IsMathOperator(Current) => ReadOperator(),
                            _ => ReadAsSingleToken()
                        };
            }

            return null;
        }

        private Token ReadAsSingleToken()
        {
            var token = new Token(Current);
            MoveNext();
            return token;
        }

        private Token ReadAhead(params KeyValuePair<char, Token>[] sequence)
        {
            var toReturn = new Token(Current);
            for (int i = 0; i < sequence.Length; i++)
            {
                if (!MoveNext())
                {
                    break;
                }
                var (ch, token) = sequence[i];
                if (Current == ch)
                {
                    toReturn = token;
                }
                else
                {
                    break;
                }
            }

            MoveNext();

            return toReturn;
        }

        private Token ReadTwoAhead(char next, Token second, char nextNext, Token third)
        {
            var current = Current;
            if (MoveNext() && Current == next)
            {
                if (MoveNext() && Current == nextNext)
                {
                    MoveNext();
                    return third;
                }

                return second;
            }

            return new Token(current);
        }
        
        private Token ReadOneAhead(char after, Token toReturn)
        {
            var current = Current;
            if (MoveNext() && Current  == after)
            {
                MoveNext();
                return toReturn;
            }

            return new Token(current);
        }

        private Token ReadSingle()
        {
            var current = Current;
            MoveNext();
            return new Token(current);
        }

        private Token ReadOperator()
        {
            return Current switch
                   {
                       '+' => ReadOneAhead('+', Word.Increment),
                       '-' => ReadOneAhead('-', Word.Decrement),
                       '|' => ReadOneAhead('|', Word.Or),
                       '&' => ReadOneAhead('&', Word.And),
                       '=' => ReadTwoAhead('=', Word.Equality, '=', Word.StrongEquality),
                       '<' => ReadOneAhead('=', new Word("<=", Tags.LessOrEqual)),
                       '>' => ReadOneAhead('=', new Word(">=", Tags.GreaterOrEqual)),
                       _   => ReadSingle()
                   };
        }


        private bool SkipWhitespaces()
        {
            bool ShouldSkip(char ch) => ch is ' ' or '\t' or '\n';
            while (!EndOfFile)
            {
                if (!ShouldSkip(Current))
                {
                    break;
                }

                MoveNext();
            }

            return !EndOfFile;
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
                        previous = Current;
                        continue;
                    }

                    MoveNext();
                    break;
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