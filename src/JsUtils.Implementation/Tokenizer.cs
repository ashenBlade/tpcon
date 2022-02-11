using System.Collections;
using System.Collections.Immutable;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using JsUtils.Implementation.JsTokens;

namespace JsUtils.Implementation;

public class Tokenizer : ITokenizer
{
    private class InternalTokenizer
    {
        private readonly string _text;
        private int _position;
        private int Position => _position;

        private char Current => _position < _text.Length
                                    ? _text[_position]
                                    : '\0';

        public bool EndOfFile => _text.Length <= _position;
        
        private bool MoveNext()
        {
            if (_position < _text.Length)
            {
                _position++;
                return true;
            }

            return false;
        }

        private bool MoveSteps(int steps)
        {
            _position += steps;
            return _position < _text.Length;
        }

        private char PeekNext()
        {
            return _position + 1 < _text.Length
                       ? _text[_position + 1]
                       : '\0';
        }
        
        public InternalTokenizer(string text)
        {
            _text = text;
            _position = 0;
        }

        public JsToken? ReadToken()
        {
            while (char.IsWhiteSpace(Current) && MoveNext())
            {
                // Move forward
            }

            if (EndOfFile)
            {
                return null;
            }

            if (char.IsDigit(Current))
            {
                return ReadNumber();
            }

            if (char.IsLetter(Current))
            {
                return ReadWord();
            }

            JsToken toReturn;
            switch (Current)
            {
                case '=':
                    toReturn = new JsEquals();
                    MoveNext();
                    break;
                case ';':
                    toReturn = new JsSemicolon();
                    MoveNext();
                    break;
                case '{':
                    toReturn = new JsLeftBrace();
                    MoveNext();
                    break;
                case '(':
                    toReturn = new JsLeftParenthesis();
                    MoveNext();
                    break;
                case '}':
                    toReturn = new JsRightBrace();
                    MoveNext();
                    break;
                case '/':
                    toReturn = ReadRegex();
                    break;
                case ')':
                    toReturn = new JsRightParenthesis();
                    MoveNext();
                    break;
                case ']':
                    toReturn = new JsRightSquareBracket();
                    MoveNext();
                    break;
                case '[':
                    toReturn = new JsLeftSquareBracket();
                    MoveNext();
                    break;
                case '-':
                case '+':
                    toReturn = ReadNumber();
                    break;
                case '\'':
                case '"':
                    toReturn = ReadStringLiteral();
                    break;
                case '.':
                    toReturn = new JsDot();
                    MoveNext();
                    break;
                case ',':
                    toReturn = new JsComma();
                    MoveNext();
                    break;
                default:
                    throw new UnexpectedTokenException(_text, _position, "Unknown token type");
            }

            return toReturn;
        }

        private JsRegex ReadRegex()
        {
            if (Current != '/')
                throw new UnexpectedTokenException(_text, _position, $"Expected '/' for regex literal. Got '{Current}'");
            
            var builder = new StringBuilder();
            var previous = '/';
            while (MoveNext())
            {
                if (Current == '/')
                {
                    if (previous == '\\')
                    {
                        builder.Append(Current);
                    }
                    else
                    {
                        if (PeekNext() == '/')
                        {
                            throw new UnexpectedTokenException(_text, Position, "Unexpected comment declaration");
                        }
                        MoveNext();
                        return new JsRegex(builder.ToString());
                    }
                }
                else
                {
                    builder.Append(Current);
                }

                previous = Current;
            }

            if (EndOfFile)
            {
                throw new UnexpectedTokenException(_text, _position, "Unexpected script end");
            }
            throw new UnexpectedTokenException(_text, _position, $"Expected '/' for regex literal. Got: {Current}");
        }

        private JsStringLiteral ReadStringLiteral()
        {
            while (char.IsWhiteSpace(Current) && MoveNext())
            {
                // Skip
            }

            if (Current is not ('\'' or '"'))
            {
                throw new UnexpectedTokenException(_text, _position, $"Invalid string representation: \" or ' expected");
            }
            
            char opener = Current;
            char previous = '\0';
            var builder = new StringBuilder();
            while (MoveNext())
            {
                if (previous == '\\')
                {
                    builder.Append(Current);
                }
                else
                {
                    if (Current == opener)
                    {
                        MoveNext();
                        return new JsStringLiteral(builder.ToString());
                    }

                    if (Current != '\\')
                    {
                        builder.Append(Current);
                    }
                }

                previous = Current;
            }
            
            throw new UnexpectedTokenException(_text, _position, "Unexpected string literal end");
        }

        private JsToken ReadWord()
        {
            var builder = new StringBuilder();
            do
            {
                builder.Append(Current);
            } while (MoveNext() && char.IsLetter(Current));

            var result = builder.ToString();
            return result switch
                   {
                       "var"   => new JsVar(),
                       "for"   => new JsFor(),
                       "while" => new JsWhile(),
                       "do"    => new JsDo(),
                       "if"    => new JsIf(),
                       "new"   => new JsNew(),
                       _       => new JsIdentifier(result)
                   };
        }

        private JsNumber ReadNumber()
        {
            // var builder = new StringBuilder();
            var regex = new Regex(@"[\+-]?\d+(\.\d*)?");
            var match = regex.Match(_text, _position);
            if (match.Success)
            {
                MoveSteps(match.Length);
                return new JsNumber(decimal.Parse(match.Value));
            }

            throw new UnexpectedTokenException(_text, _position, "Number expected");
        }
    }
    
    public IEnumerable<JsToken> Tokenize(string text)
    {
        var tokenizer = new InternalTokenizer(text);
        while (!tokenizer.EndOfFile)
        {
            var read = tokenizer.ReadToken();
            if (read == null)
            { 
                yield break;
            }

            yield return read;
        }

    }
}