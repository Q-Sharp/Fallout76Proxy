using System.Threading.Tasks;

namespace Fallout76.Proxy
{
    public interface IGameManager
    {
        string GetCommandLine(string sProcessName);
        Task RestartAsChild(string sProcessName);
        Task WaitForProcessAsync(string sProcessName);
    }
}
