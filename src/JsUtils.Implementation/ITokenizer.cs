using JsUtils.Implementation.JsTokens;

namespace JsUtils.Implementation;

public interface ITokenizer
{
    public IEnumerable<JsToken> Tokenize(string text);
}