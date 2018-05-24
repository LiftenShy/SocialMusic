using System.Net;
using AccountManager.Certificate;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace AccountManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(options =>
                {
                    options.Listen(IPAddress.Loopback, 50608);
                    options.Listen(IPAddress.Any, 80);
                    options.Listen(IPAddress.Loopback, 443, listenOptions =>
                    {
                        listenOptions.UseHttps(Cerftificate.Get());
                    });
                })
                .UseStartup<Startup>()
                .Build();
    }
}
