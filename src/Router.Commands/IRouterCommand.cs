using System.Threading.Tasks;

namespace Router.Commands;

public interface IRouterCommand
{
    public Task ExecuteAsync();
}