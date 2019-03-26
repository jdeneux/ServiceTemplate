using jwtApi.Presentation.Config;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using System;

namespace jwtApi
{
    public class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                var webHost = CreateWebHostBuilder(args);
                webHost.Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host Terminated unexpectedly");
                return -1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHost CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog((hostingContext, logging) =>
                {
                    logging.ReadFrom.Configuration(hostingContext.Configuration)
                        .Enrich.FromLogContext()
                        .Enrich.WithProperty("Application", AppConfig.Name)
                        .Enrich.WithProperty("ApplicationVersion", AppConfig.GetLongVersion());
                })
                .UseKestrel(k => k.AddServerHeader = false)
                .UseUrls("http://0.0.0.0:5000", "https://0.0.0.0:5001")
                .Build();
    }
}
