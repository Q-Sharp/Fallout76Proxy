using System.Threading.Tasks;

namespace Fallout76Proxy
{
    public interface IGameManager
    {
        string GetCommandLine(string processName);
        void RestartAsChild();
        Task WaitForAsync();
    }
}
