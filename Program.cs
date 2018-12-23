using System;
using System.Threading.Tasks;

namespace Fallout76Proxy
{
    public static class Program
    {
        public static async Task Launch()
        {
            if(!BethesdaLauncher.Default.IsInstalled)
                throw new Exception("Try to reinstall bethesda launcher.");

            Console.WriteLine("Starting Fallout76 from BethesdaLauncher.");

            BethesdaLauncher.Default.Start(BethesdaGameType.Fallout76);
            Console.WriteLine("Waiting for game started.");

            var fallout76 = new GameManager("Fallout76");
            await fallout76.WaitForAsync();

            Console.WriteLine("Restarting Fallout 76 as child process.");
            fallout76.RestartAsChild();

            Console.WriteLine("Closing BethesdaLauncher.");
            BethesdaLauncher.Default.Stop();
        }

        public static async Task Main(string[] args)
        {
            try
            {
                await Launch();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine("\nPress any key to exit...");
                Console.Read();
            }
        }
    }
}

