using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System.IO;

namespace jwtApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHost = CreateWebHostBuilder(args);

            //var configuration = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //            .AddJsonFile("appsettings.json")
            //            .AddJsonFile($"appsettings.{_environmentName}.json", optional: true, reloadOnChange: true)
            //            .Build();

            //Log.Logger = new LoggerConfiguration()
            //    .ReadFrom.Configuration(configuration)
            //    .CreateLogger();

            webHost.Run();
        }

        public static IWebHost CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders();
                })
                .UseStartup<Startup>()
                .UseSerilog((hostingContext, logging) =>
                {
                    logging.ReadFrom.Configuration(hostingContext.Configuration)
                        .Enrich.FromLogContext();
                })
                .Build();
    }
}
