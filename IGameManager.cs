using System.Threading.Tasks;

namespace Fallout76.Proxy
{
    public interface IGameManager
    {
        string GetCommandLine(string sProcessName);
        void RestartAsChild(string sProcessName);
        void GetProcess(string sProcessName);
    }
}
