using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Linq;

namespace Fallout76.Proxy
{
    public class BethesdaLauncher : IBethesdaLauncher
    {
        public bool IsInstalled => Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes\BethesdaNet\Shell\Open\Command") != null;
        public bool IsActive => Process.GetProcessesByName("BethesdaNetLauncher").Any();

        public void Start(BethesdaGameType GameIdx) => Process.Start($"bethesdanet://run/{(int)GameIdx}");
        public void Stop() => Process.GetProcessesByName("BethesdaNetLauncher")?.ToList().ForEach(x => x.Kill());

        public void Launch(BethesdaGameType eGameType)
        {
            if (!IsInstalled)
                throw new Exception(Fallout76ProxyResource.Reinstall);

            Console.WriteLine(Fallout76ProxyResource.Starting);
            Start(eGameType);

            Console.WriteLine(Fallout76ProxyResource.Wait);
            var oGameManager = new GameManager();
            oGameManager.GetProcess(eGameType.ToString());

            Console.WriteLine(Fallout76ProxyResource.Restart);
            oGameManager.RestartAsChild(eGameType.ToString());

            Console.WriteLine(Fallout76ProxyResource.Close);
            Stop();
        }
    }
}
