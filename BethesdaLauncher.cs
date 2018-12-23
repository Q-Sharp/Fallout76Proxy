using Microsoft.Win32;
using System.Diagnostics;
using System.Linq;

namespace Fallout76Proxy
{
    public class BethesdaLauncher : IBethesdaLauncher
    {
        public static BethesdaLauncher Default { get; set; } = new BethesdaLauncher();

        public bool IsInstalled => Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes\BethesdaNet\Shell\Open\Command") != null;
        public bool IsActive => Process.GetProcessesByName("BethesdaNetLauncher").Any();

        public void Start(BethesdaGameType GameIdx) => Process.Start($"bethesdanet://run/{(int)GameIdx}");
        public void Stop() => Process.GetProcessesByName("BethesdaNetLauncher").FirstOrDefault()?.Kill();
    }
}
