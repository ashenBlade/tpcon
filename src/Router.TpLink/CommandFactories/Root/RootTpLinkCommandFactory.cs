using Router.Commands;
using Router.Commands.Exceptions;

namespace Router.TpLink.CommandFactories.Root;

internal class RootTpLinkCommandFactory : CompositeTpLinkCommandFactory
{
    public RootTpLinkCommandFactory(IEnumerable<InternalTpLinkCommandFactory> commands)
        : base(commands, string.Empty)
    { }
}