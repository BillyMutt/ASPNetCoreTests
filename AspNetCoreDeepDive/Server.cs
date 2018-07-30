using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;

namespace AspNetCoreDeepDive
{
    public static class Program
    {
        internal static DataModel Model = new DataModel();

        static void Main(string[] args)
        {
            //  Build our ASP .NET Core host.
            IWebHost host = WebHost.CreateDefaultBuilder(args)
               .UseUrls("http://localhost:9000")
               .UseStartup<Startup>()
               .Build();

            //  Run the host.
            host.Start();
            Console.WriteLine("Host started - press ENTER to quit");
            Console.ReadLine();
        }
    }
}
