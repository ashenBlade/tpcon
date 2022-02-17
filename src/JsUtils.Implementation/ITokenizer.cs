using JsUtils.Implementation.Tokens;

namespace JsUtils.Implementation;

public interface ITokenizer
{
    IEnumerable<Token> Tokenize(string source);
}