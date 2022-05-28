namespace Router.TpLink.TLWR741ND.Commands;

public class TpLinkSetWlanPasswordCommand : TpLinkBaseCommand
{
    private readonly string _password;

    public TpLinkSetWlanPasswordCommand(TpLinkRouter router, string password) : base(router)
    {
        _password = password;
    }

    public override Task ExecuteAsync()
    {
        return Router.Wlan.SetPasswordAsync(_password);
    }
}