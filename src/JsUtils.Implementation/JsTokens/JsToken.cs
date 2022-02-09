using System.Text.Json;

namespace JsUtils.Implementation.JsTokens;

public abstract class JsToken : IEquatable<JsToken>
{
    public string Name { get; }
    public JsTokenType TokenType { get; }
    protected JsToken(string name, JsTokenType tokenType)
    {
        Name = name;
        TokenType = tokenType;
    }

    public virtual bool Equals(JsToken? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name
            && TokenType == other.TokenType;
    }
    
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals(( JsToken ) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, ( int ) TokenType);
    }
}