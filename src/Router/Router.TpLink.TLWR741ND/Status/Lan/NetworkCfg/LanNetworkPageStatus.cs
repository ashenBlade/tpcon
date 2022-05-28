using JsTypes;

namespace Router.TpLink.TLWR741ND.Status.Lan.NetworkCfg;

public class LanNetworkPageStatus : LanPageStatus
{
    public LanNetworkPageStatus(JsArray lanPara)
    {
        ArgumentNullException.ThrowIfNull(lanPara);
        LanPara = lanPara;
    }

    public JsArray LanPara { get; }
}