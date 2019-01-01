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
        private volatile Process oProcess;
        private readonly Func<string, Process> GetProcess = (string pn) => Process.GetProcesses().FirstOrDefault(y => y.ProcessName == pn);

        public GameManager()
        {
        }

        public async Task WaitForProcessAsync(string sProcessName)
        {
            oProcess = await Task.Run(() =>
            {
                SpinWait.SpinUntil(() => GetProcess(sProcessName) != null, TimeSpan.FromMinutes(5));
                return GetProcess(sProcessName);
            });
        }

        public Task RestartAsChild(string sProcessName)
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

        public string GetCommandLine(string sProcessName)
        {
            var mngmtClass = new ManagementClass("Win32_Process");
            foreach(var o in mngmtClass.GetInstances())
            {
                if(o["Name"].Equals(sProcessName))
                    return (string)o["CommandLine"];
            }

            throw new SystemException(string.Format(Fallout76ProxyResource.Arguments, sProcessName));
        }

        public static bool Fallout76Exists() => Process.GetProcessesByName("Fallout76").Count() > 0;
    }
}
