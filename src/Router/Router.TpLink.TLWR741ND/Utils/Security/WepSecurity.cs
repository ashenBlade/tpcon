using Router.TpLink.TLWR741ND.Commands.DisplayStatus;

namespace Router.TpLink.TLWR741ND.Utils.Security;

public class WepSecurity : CustomSecurity
{
    private readonly WepKey[] _keys;
    public IReadOnlyList<WepKey> Keys => _keys;
    public WepKey Selected { get; }
    public WepType Type { get; }
    public WepKeyFormat Format { get; }
    public override string Name => "WEP";

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
    
    public override SecurityDisplayStatus ToDisplayStatus()
    {
        return new WepDisplayStatus(this);
    }
}