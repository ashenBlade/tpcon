using Router.Domain;
using Router.TPLink.JsParser;
using Router.TPLink.ScriptExtractor;

namespace Router.TPLink;

public abstract class TPLinkRouter : RemoteRouter
{
    protected IJsParser JsParser { get; }
    protected IScriptExtractor ScriptExtractor { get;  }
    protected TPLinkRouter(string username, string password, Uri address, IJsParser jsParser, IScriptExtractor scriptExtractor) 
        : base(username, password, address)
    {
        JsParser = jsParser;
        ScriptExtractor = scriptExtractor;
    }
}