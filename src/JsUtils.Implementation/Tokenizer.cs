using System.Text;
using JsUtils.Implementation.Tokens;

namespace JsUtils.Implementation;

public class Tokenizer : ITokenizer
{
    private class InnerTokenizer
    {
        private readonly string _source;
        private int _position;
        private int Position => _position;
        private char Current => _source[_position];
        public InnerTokenizer(string source)
        {
            _source = source;
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
            while (MoveNext())
            {
                if (char.IsDigit(Current))
                {
                    return ReadNumber();
                }

                switch (Current)
                {
                    case '\'':
                    case '"':
                        return ReadStringLiteral();    
                }
            }

            return null;
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
    public IEnumerable<Token> Tokenize(string source)
    {
        var tokenizer = new InnerTokenizer(source);
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