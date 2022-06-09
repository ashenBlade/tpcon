using Router.Domain;

namespace Router.Commands;

public abstract class RouterCommandFactory
{
    protected RouterConnectionParameters ConnectionParameters { get; }

    protected RouterCommandFactory(RouterConnectionParameters connectionParameters)
    {
        ConnectionParameters = connectionParameters;
    }
}