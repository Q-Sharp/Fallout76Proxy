using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text.RegularExpressions;
using System.Threading;

namespace Fallout76.Proxy
{
    public class GameManager : IGameManager
    {
        private volatile Process oProcess;

        public void GetProcess(string sProcessName)
        {
            SpinWait.SpinUntil(() => Process.GetProcessesByName(sProcessName).Any(), TimeSpan.FromMinutes(5));
            oProcess = Process.GetProcessesByName(sProcessName).FirstOrDefault();
        }

        public void RestartAsChild(string sProcessName)
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
        }

        public string GetCommandLine(string sProcessName)
        {
            var mngmtClass = new ManagementClass("Win32_Process");
            foreach (var o in mngmtClass.GetInstances())
            {
                if (o["Name"].Equals(sProcessName))
                    return (string)o["CommandLine"];
            }

            throw new SystemException(string.Format(Fallout76ProxyResource.Arguments, sProcessName));
        }
    }
}
