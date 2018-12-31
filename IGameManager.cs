using System.Threading.Tasks;

namespace Fallout76.Proxy
{
    public interface IGameManager
    {
        string GetCommandLine(string processName);
        Task RestartAsChild();
        Task WaitForProcessAsync();
    }
}
