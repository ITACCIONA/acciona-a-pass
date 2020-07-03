using AccionaCovid.Domain.Core;
using AccionaCovid.WebApi.Core;
using AccionaCovid.WebApi.Utils;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;


namespace AccionaCovid.WebApi
{
    /// <summary>
    /// Clase de arranque de la app
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// IConfigurationeee
        /// </summary>
        private IConfiguration Configuration { get; }

        /// <summary>
        /// IWebHostEnvironment
        /// </summary>
        private IWebHostEnvironment Environment { get; }

        /// <summary>
        /// IWebHostEnvironment
        /// </summary>
        private WebHostBuilderContext HostingContext { get; }

        /// <summary>
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        /// <param name="hostingContext"></param>
        public Startup(IConfiguration configuration, IWebHostEnvironment environment, WebHostBuilderContext hostingContext)
        {
            Configuration = configuration;
            Environment = environment;
            HostingContext = hostingContext;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            InfrastructureSettings infrastructureSettings = services.ConfigureSettings(Configuration);

            IdentityModelEventSource.ShowPII = true;

            services.AddControllers();

            services.RegisterDataBaseContext(Configuration);

            services.AddCovid19Authorization();
            services.AddCovid19Authentication(Configuration, infrastructureSettings, Configuration, Log.Logger);

            services.ConfigureMvc();
            services.ConfigureResponses();
            services.ConfigureSwagger(Environment, infrastructureSettings);
            services.ConfigureHostedServices(Configuration); // Custom
            services.ConfigureAppInsights(Configuration); //custom

            return services.ConfigureAutofac(Environment, Configuration, infrastructureSettings.LogicalRemove);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                // TODO: Cambiar a un IStartupFilter
                app.ConfigureMigration(HostingContext, scope.ServiceProvider, env);
            }

            InfrastructureSettings settings = Configuration.GetSettings<InfrastructureSettings>();

            app.UseDeveloperExceptionPage();

            // configuramos swagger si esta permitido
            var configSectionSwagger = HostingContext.Configuration.GetSection("Swagger");
            bool swaggerEnable = configSectionSwagger.GetValue<bool>("Enable");

            if (swaggerEnable)
            {
                app.UseSwaggerPipeline(settings);
            }

            app.UseHttpsRedirection();
            app.UseResponseCompression();
            app.UseStaticFiles();

            app.UseSerilogRequestLogging();

            app.UseRouting();

            if (settings.Authentication.Enabled)
            {
                app.UseAuthentication();
                // TODO: Enriquecer el log con el usuario autenticado.
                app.UseAuthorization();
                app.UseFillUserInfoMiddleware();
            }

            // TODO: Mover a ConfigureServices
            // permitimos localizacion
            var supportedCultures = new[]
            {
                new CultureInfo("es"),
                new CultureInfo("en"),
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("es"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });

            AllowedLanguages.Instance.Languages = new List<CultureInfo>(supportedCultures);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
