using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Fallout76.Proxy
{
    public class GameManager : IGameManager
    {
        private readonly string sProcessName;
        private volatile Process oProcess;

        public GameManager(string sProcessName)
        {
            this.sProcessName = sProcessName;
        }

        public async Task WaitForProcessAsync()
        {
            oProcess = await Task.Run(() =>
            {
                Process oFound = null;
                SpinWait.SpinUntil(() =>
                {
                    oFound = Process.GetProcesses().FirstOrDefault(x => x.ProcessName == sProcessName);
                    return oFound != null;
                }, TimeSpan.FromMinutes(5));

                return oFound;
            });
        }

        public Task RestartAsChild()
        {
            return Task.Run(() =>
            {
                var regex = new Regex("\"(.+?)\"\\s(.+)");
                var match = regex.Match(GetCommandLine($"{sProcessName}.exe"));

                oProcess?.Kill();
                var TargetPath = match.Groups[1].Value;
                var TargetArguments = match.Groups[2].Value;

                Directory.SetCurrentDirectory(Path.GetDirectoryName(TargetPath));

                oProcess = new Process();
                oProcess.StartInfo.FileName = TargetPath;
                oProcess.StartInfo.Arguments = TargetArguments;
                oProcess.Start();
            });
        }

        public string GetCommandLine(string processName)
        {
            var mngmtClass = new ManagementClass("Win32_Process");
            foreach(var o in mngmtClass.GetInstances())
            {
                if(o["Name"].Equals(processName))
                    return (string)o["CommandLine"];
            }

            throw new SystemException(string.Format(Fallout76ProxyResource.Arguments, processName));
        }

        public static bool Fallout76Exists() => Process.GetProcessesByName("Fallout76").Count() > 0;
    }
}
