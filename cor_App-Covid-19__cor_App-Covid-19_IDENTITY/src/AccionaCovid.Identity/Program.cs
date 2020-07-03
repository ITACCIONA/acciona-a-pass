// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Collections.ObjectModel;
using System.Data;

namespace AccionaCovid.Identity
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console(theme: AnsiConsoleTheme.Literate)
                .WriteTo.Debug()
                .CreateLogger();

            try
            {
                Log.Information("Starting host...");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostContext, builder) =>
                {
                    // Add other providers for JSON, etc.
                    if (hostContext.HostingEnvironment.IsEnvironment("Local") ||
                        hostContext.HostingEnvironment.IsDevelopment())
                    {
                        builder.AddUserSecrets<Program>();
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((context, builder) =>
                    {
                        builder.AddJsonFile("identity.json")
                            .AddJsonFile($"identity.{context.HostingEnvironment.EnvironmentName}.json")
                            .AddEnvironmentVariables();
                    })
                    .UseStartup<Startup>()
                    .UseSerilog((WebHostBuilderContext hostingContext, LoggerConfiguration config) =>
                    {
                        string connection = hostingContext.Configuration.GetConnectionString("AccionaCovidConnection");

                        var configSection = hostingContext.Configuration.GetSection("Logging");

                        LogEventLevel sqlLevel = Enum.Parse<LogEventLevel>(configSection.GetValue<string>("SQLLogLevel"));

                        var columnOptions = new ColumnOptions
                        {
                            AdditionalColumns = new Collection<SqlColumn>
                            {
                                new SqlColumn(new DataColumn {DataType = typeof(string), ColumnName = "RequestId"}),
                                new SqlColumn(new DataColumn {DataType = typeof(string), ColumnName = "UserName"})
                            }
                        };

                        config.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning);
                        config.MinimumLevel.Override("Microsoft", LogEventLevel.Warning);
                        //config.Enrich.WithHttpRequestId();
                        //config.Enrich.WithUserName();
                        config.Enrich.FromLogContext();
                        config.WriteTo.MSSqlServer(connection, "Logs", restrictedToMinimumLevel: sqlLevel, autoCreateSqlTable: true, columnOptions: columnOptions);

                        // Para ver los log en el app service de azure
                        config.WriteTo.File($@"D:\home\LogFiles\AccionaCovid.Identity.txt",
                          restrictedToMinimumLevel: sqlLevel,
                          rollingInterval: RollingInterval.Day,
                          rollOnFileSizeLimit: true,
                          shared: true,
                          flushToDiskInterval: TimeSpan.FromSeconds(1),
                          outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [RequestId: {RequestId}] [{Level:u3}] {Message:lj} [UserName: {UserName}] {NewLine}{Exception}");

                        if (hostingContext.HostingEnvironment.IsDevelopment())
                        {
                            config.WriteTo.Console(sqlLevel);
                            //config.WriteTo.File("D:/Proyectos/log.ndjson", restrictedToMinimumLevel: LogEventLevel.Information, rollingInterval: RollingInterval.Day, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [RequestId: {RequestId}] [{Level:u3}] {Message:lj} [UserName: {UserName}] {NewLine}{Exception}");
                        }
                        else
                        {
                            config.WriteTo.Console(sqlLevel);
                        }
                    });
                });
    }
}