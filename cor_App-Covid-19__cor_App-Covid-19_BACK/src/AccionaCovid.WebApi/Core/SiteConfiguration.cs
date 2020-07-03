using AccionaCovid.Crosscutting;
using AccionaCovid.Data;
using AccionaCovid.Data.Seed;
using AccionaCovid.WebApi.Core;
using AccionaCovid.WebApi.HostedServices;
using AccionaCovid.WebApi.Utils;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Text;
using WebApiContrib.Core.Formatter.Csv;

namespace AccionaCovid.WebApi
{
    /// <summary>
    /// Configuracion del site
    /// </summary>
    public static class SiteConfiguration
    {
        /// <summary>
        /// Metodo que registra la conexión a la base de datos en el contenedor de dependencias.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="config"></param>
        public static void RegisterDataBaseContext(this IServiceCollection service, IConfiguration config)
        {
            string sqlConnection = config.GetConnectionString("AccionaCovidConnection");
            service.AddDbContext<AccionaCovidContext>(options => options.UseSqlServer(sqlConnection), ServiceLifetime.Transient);
        }

        /// <summary>
        /// Custom configuration for Responses
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureResponses(this IServiceCollection services)
        {
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });

            services.AddResponseCompression(options =>
            {
                IEnumerable<string> MimeTypes = new[]
                {
                     // General
                     "text/plain",
                     "text/html",
                     "text/css",
                     "font/woff2",
                     "application/javascript",
                     "image/x-icon",
                     "image/png",
                     "application/json",
                     "text/json",
                     "text/csv"
                 };

                options.EnableForHttps = true;
                options.MimeTypes = MimeTypes;
                options.Providers.Add<GzipCompressionProvider>();
                options.Providers.Add<BrotliCompressionProvider>();
            });

            return services;
        }

        /// <summary>
        /// Añade la configuración del API.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureMvc(this IServiceCollection services)
        {
            //services.AddCors(corsOptions =>
            //{
            //    corsOptions.AddDefaultPolicy(builder =>
            //    {
            //        builder
            //        .AllowAnyOrigin()
            //        .AllowAnyHeader()
            //        .AllowAnyMethod()
            //        .AllowCredentials();
            //    });
            //});

            // añadimos los ficheros de recursos para la localizacion
            services.AddLocalization(options => options.ResourcesPath = "ValidatorsMessages");
            services.AddLocalization(options => options.ResourcesPath = "ValidatorFields");

            services.AddMvc()
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
            .AddMvcOptions(options =>
                {
                    options.Filters.Add(typeof(ExceptionFilter));
                    options.FormatterMappings.SetMediaTypeMappingForFormat("csv", Microsoft.Net.Http.Headers.MediaTypeHeaderValue.Parse("text/csv"));
                    options.OutputFormatters.Add(new CsvOutputFormatter(new CsvFormatterOptions()
                    {
                        CsvDelimiter = ";",
                        Encoding = Encoding.Default,
                        UseSingleLineHeaderInCsv = true
                    }));
                });

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
            });

            // TODO: Mock hasta que se active la autenticacion y autorizacion
            services.AddScoped<IUserInfoAccesor, UserInfoAccesor>();
            //services.AddScoped<IUserInfoAccesor>(ctx => new UserInfoAccesor() { IdUser = 1 });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
            });

            return services;
        }

        /// <summary>
        /// Custom configuration for Autofac
        /// Advanced Dependency Injection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="environment"></param>
        /// <param name="configuration"></param>
        /// <param name="logicalRemove"></param>
        /// <returns></returns>
        public static IServiceProvider ConfigureAutofac(this IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration, bool logicalRemove)
        {
            IContainer container = AutofacConfiguration.ConfigureAutofac(services, environment, configuration, logicalRemove);
            return new AutofacServiceProvider(container);
        }

        /// <summary>
        /// Configura serilog
        /// </summary>
        /// <param name="hostingContext"></param>
        /// <param name="config"></param>
        public static void ConfigureSerilog(WebHostBuilderContext hostingContext, LoggerConfiguration config)
        {
            //https://github.com/serilog/serilog/wiki/Enrichment

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
            config.Enrich.WithHttpRequestId();
            config.Enrich.WithUserName();
            config.Enrich.FromLogContext();
            config.WriteTo.MSSqlServer(connection, "Logs", restrictedToMinimumLevel: sqlLevel, autoCreateSqlTable: true, columnOptions: columnOptions);

            // Para ver los log en el app service de azure
            config.WriteTo.File($@"D:\home\LogFiles\AccionaMov.WebApi.txt",
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
        }

        /// <summary>
        /// Metodo que ejecuta la migracion y seed en BBDD si hubiera y si el parametro enable del fichero de configuracion lo permite
        /// </summary>
        /// <param name="app"></param>
        /// <param name="hostingContext"></param>
        /// <param name="container"></param>
        /// <param name="environment"></param>
        public static void ConfigureMigration(this IApplicationBuilder app, WebHostBuilderContext hostingContext, IServiceProvider container, IWebHostEnvironment environment)
        {
            var configSectionMigration = hostingContext.Configuration.GetSection("Migration");
            var configSectionSeed = hostingContext.Configuration.GetSection("Seed");

            bool migrationEnable = configSectionMigration.GetValue<bool>("Enable");
            bool seedEnable = configSectionSeed.GetValue<bool>("Enable");

            if (migrationEnable)
            {
                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    AccionaCovidContext database = serviceScope.ServiceProvider.GetService<AccionaCovidContext>();
                    database.Database.Migrate();
                }
            }

            if (seedEnable)
            {
                string path = System.IO.Directory.GetCurrentDirectory();
                string rawJson = System.IO.File.ReadAllText(Path.Combine(path, $"seed.{environment.EnvironmentName}.json"));
                JObject parsed = JObject.Parse(rawJson);

                container.GetService<DataSeeder>().Seed(parsed);
            }
        }

        /// <summary>
        /// Custom configuration for Settings.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static InfrastructureSettings ConfigureSettings(this IServiceCollection services, IConfiguration config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            InfrastructureSettings infrastructureSettings = config
                .GetSection(nameof(InfrastructureSettings))
                .Get<InfrastructureSettings>();

            services.Configure<InfrastructureSettings>(config
                .GetSection(nameof(InfrastructureSettings)));

            services.AddHttpClient()
                .AddSingleton<HttpProxy, HttpProxy>();

            return infrastructureSettings;
        }

        /// <summary>
        /// Custom configuration for Settings.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static TSettings GetSettings<TSettings>(this IConfiguration config) => config.GetSection(typeof(TSettings).Name).Get<TSettings>();

        /// <summary>
        /// Custom configuration for hosted services
        /// Advanced Dependency Injection
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureHostedServices(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetSection("CronExpressions").Exists())
            {
                if (configuration.GetValue<bool>("InfrastructureSettings:IntegrationSettings:Enable"))
                {
                    services.AddCronJob<IntegrationFileStorageCronWorker>(c =>
                    {
                        c.CronHandler = new CronHandlerProvider();
                        c.TimeZoneInfo = TimeZoneInfo.Local;
                        c.CronExpression = configuration.GetValue<string>("CronExpressions:IntegrationJob");
                    });
                }
            }
            return services;
        }

        /// <summary>
        /// Custom configuration for Azure Application Insights Telemetry
        /// Advanced Dependency Injection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureAppInsights(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationInsightsTelemetry(configuration.GetValue<string>("InfrastructureSettings:AppInsightsInstrumentationKey"));
            return services;
        }
    }
}
