using System.Collections;
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
            JsToken toReturn = null;
            while (char.IsWhiteSpace(Current) && MoveNext())
            {
                // Move forward
            }

            if (EndOfFile)
            {
                return toReturn;
            }

            if (char.IsDigit(Current))
            {
                toReturn = ReadNumber();
            }

            else if (char.IsLetter(Current))
            {
                toReturn = ReadWord();
            }
            else
            {
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
                    case '-':
                    case '+':
                        toReturn = ReadNumber();
                        break;
                    case '\'':
                    case '"':
                        toReturn = ReadStringLiteral();
                        break;
                    default:
                        throw new UnexpectedTokenException(_text, _position, "Unknown token type");
                }
            }

            return toReturn;
        }

        private JsStringLiteral ReadStringLiteral()
        {
            var regex = new Regex(@"('(?<Content>(\s|.)*?)'|""(?<Content>(\s|.)*?)"")");
            var match = regex.Match(_text, Position);
            if (match.Success)
            {
                MoveSteps(match.Length);
                var content = match.Value;
                return new JsStringLiteral(content);
            }

            throw new UnexpectedTokenException(_text, _position, "Invalid string literal representation");
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
                       "var" => new JsVar(),
                       _     => new JsIdentifier(result)
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