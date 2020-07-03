using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;

namespace AccionaCovid.WebApi
{
    /// <summary>
    /// Clase de arranque del web api
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Metodo principal
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .Enrich.WithThreadId()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

            try
            {
                Log.Information("Starting up");
                CreateWebHostBuilder(args).Build().Run();
                Log.Information("Stopping");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostContext, builder) =>
            {
                // Add other providers for JSON, etc.
                if (hostContext.HostingEnvironment.IsEnvironment("Local") ||
                    hostContext.HostingEnvironment.IsDevelopment())
                {
                    builder.AddUserSecrets<Program>();
                }
            })
            .ConfigureKestrel(serverOptions =>
            {
                //serverOptions.Limits.MaxConcurrentConnections = 100;
                //serverOptions.Limits.MaxConcurrentUpgradedConnections = 100;
                //serverOptions.Limits.MaxRequestBodySize = 100000000;
                //serverOptions.Limits.MinRequestBodyDataRate = new MinDataRate(bytesPerSecond: 15, gracePeriod: TimeSpan.FromMinutes(10));
                //serverOptions.Limits.MinResponseDataRate = new MinDataRate(bytesPerSecond: 15, gracePeriod: TimeSpan.FromMinutes(10));
                ////serverOptions.Listen(IPAddress.Loopback, 5000);
                ////serverOptions.Listen(IPAddress.Loopback, 5001,
                ////    listenOptions =>
                ////    {
                ////        listenOptions.UseHttps("testCert.pfx",
                ////            "testPassword");
                ////    });
                //serverOptions.Limits.KeepAliveTimeout =
                //    TimeSpan.FromMinutes(10);
                //serverOptions.Limits.RequestHeadersTimeout =
                //    TimeSpan.FromMinutes(10);

            })
            .UseStartup<Startup>()
            .UseSerilog(SiteConfiguration.ConfigureSerilog);
    }
}
