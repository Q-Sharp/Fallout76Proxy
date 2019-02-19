using System;
using System.Linq;

namespace Fallout76.Proxy
{
    public static class Program
    {
        public static void Main(string[] oArgs)
        {
            try
            {
                var eGameType = oArgs.Any() 
                    ? (BethesdaGameType)Enum.Parse(typeof(BethesdaGameType), oArgs.FirstOrDefault(), true) 
                    : BethesdaGameType.Fallout76;

                new BethesdaLauncher().Launch(eGameType);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}

