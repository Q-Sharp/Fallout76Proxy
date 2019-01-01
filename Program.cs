using System;
using System.Threading.Tasks;

namespace Fallout76.Proxy
{
    public static class Program
    {
        private static async Task Launch(BethesdaGameType eGameType)
        {
            if(!BethesdaLauncher.Default.IsInstalled)
                throw new Exception(Fallout76ProxyResource.Reinstall);

            Console.WriteLine(Fallout76ProxyResource.Starting);

            BethesdaLauncher.Default.Start(eGameType);
            Console.WriteLine(Fallout76ProxyResource.Wait);

            var fallout76 = new GameManager();
            await fallout76.WaitForProcessAsync(eGameType.ToString());

            Console.WriteLine(Fallout76ProxyResource.Restart);
            await fallout76.RestartAsChild(eGameType.ToString());

            Console.WriteLine(Fallout76ProxyResource.Close);
            BethesdaLauncher.Default.Stop();
        }

        public static async Task Main(string[] args)
        {
            try
            {
                await Launch(BethesdaGameType.Fallout76);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                Console.WriteLine();
                Console.WriteLine(Fallout76ProxyResource.Exit);
                Console.Read();
            }
        }
    }
}

