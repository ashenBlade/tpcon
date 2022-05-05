using Router.Domain;

namespace Router.Commands.Commands;

public abstract class GetWlanStatusCommand : BaseRouterCommand
{
    public TextWriter Output { get; }

    public GetWlanStatusCommand(RouterParameters routerParameters, TextWriter output)
        : base(routerParameters)
    {
        Output = output;
    }
}