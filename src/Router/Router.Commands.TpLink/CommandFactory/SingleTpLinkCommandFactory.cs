namespace Router.Commands.TpLink.CommandFactory;

public abstract class SingleTpLinkCommandFactory : TpLinkCommandFactory
{
    protected SingleTpLinkCommandFactory(string name) 
        : base(name)
    { }
}