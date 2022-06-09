namespace Router.Utils.Security;

public class WepSecurity : Domain.Wlan.Security
{
    public override string Name => "WEP";
    
    private readonly WepKey[] _keys;
    public IReadOnlyList<WepKey> Keys => _keys;
    public WepKey Selected { get; }
    public WepType Type { get; }
    public WepKeyFormat Format { get; }

    public WepSecurity(IEnumerable<WepKey> keys,
                       WepKey selected,
                       WepType type, 
                       WepKeyFormat format)
    {
        Selected = selected;
        Type = type;
        Format = format;
        _keys = keys.ToArray();
    }
}