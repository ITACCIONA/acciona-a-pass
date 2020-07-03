using AccionaCovid.Identity.Data;
using AccionaCovid.Identity.Infrastructure;
using IdentityServer4.EntityFramework.Options;
using IdentityServer4.EntityFramework.Storage;
using IdentityServer4.EntityFramework.Stores;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace AccionaCovid.Identity
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }

        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            this.Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

            services.AddDbContext<IdentityDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("AccionaCovidConnection")));

            services.AddScoped(ctx => new Lazy<IdentityDbContext>(() => ctx.GetRequiredService<IdentityDbContext>()));

            services.AddDefaultIdentity<ApplicationUser>(options =>
                    Configuration.Bind("AspNetIdentity", options))
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<IdentityDbContext>();

            services.AddAuthentication()
                .AddAzureAD(options => Configuration.Bind("AzureAd", options))
                //.AddAzureADBearer(options => Configuration.Bind("AzureAd", options));
                .AddJwtBearer("AzureADBearer", o =>
                {
                    AzureADOptions options = new AzureADOptions();
                    Configuration.Bind("AzureAd", options);

                    o.Authority = $"{options.Instance}/{options.TenantId}";
                    
                    o.SaveToken = true;
                    string backClientId = Configuration.GetValue<string>("AzureAd:BackClientId");
                    o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {

                        ValidAudiences = new List<string> { backClientId, $"api://{backClientId}" },
                        
                    };
                });



            services.AddScoped<IEmailSender>(ctx =>
    {
        var sendGrid = new SendGridClient(Configuration.GetSection("SendGrid").Get<SendGridClientOptions>());
        var from = new EmailAddress(Configuration.GetSection("SendGrid")["EmailFrom"], Configuration.GetSection("SendGrid")["NameFrom"]);
        return new SendGridEmailSender(sendGrid, from, ctx.GetRequiredService<ILogger<SendGridEmailSender>>());
    });

            services.Configure<OpenIdConnectOptions>(AzureADDefaults.OpenIdScheme, options =>
            {
                options.SignInScheme = IdentityConstants.ExternalScheme;
            });

            var identityResources = Configuration.GetSection("IdentityResource").Get<List<IdentityResource>>();
            var apis = Configuration.GetSection("ApiResource").Get<List<ApiResource>>();
            var clients = Configuration.GetSection("Client").Get<List<Client>>();

            services.AddTransient<IProfileService, ProfileService>();

            // Adds a PersistedGrantDbContext
            services.AddOperationalDbContext<PersistentStoreDbContext>(options =>
            {
                options.EnableTokenCleanup = true;
                options.PersistedGrants = new TableConfiguration("IdentityServerPersistedGrants");
                options.DeviceFlowCodes = new TableConfiguration("IdentityServerDeviceFlowCodes");
                options.ConfigureDbContext = db => db.UseSqlServer(Configuration.GetConnectionString("AccionaCovidConnection"));
            });
            services.AddTransient<IDeviceFlowStore, DeviceFlowStore>();
            services.AddTransient<IPersistedGrantStore, PersistedGrantStore>();

            var builder = services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                    options.UserInteraction.LoginUrl = "/Account/Login";
                    options.UserInteraction.LogoutUrl = "/Account/Logout";
                    options.Authentication = new IdentityServer4.Configuration.AuthenticationOptions()
                    {
                        CookieLifetime = TimeSpan.FromHours(20), // Less than one day.
                        CookieSlidingExpiration = true
                    };
                })
                .AddInMemoryIdentityResources(identityResources)
                .AddInMemoryApiResources(apis)
                .AddInMemoryClients(clients)
                .AddPersistedGrantStore<PersistedGrantStore>()
                .AddDeviceFlowStore<DeviceFlowStore>()
                .AddAspNetIdentity<ApplicationUser>()
                .AddProfileService<ProfileService>();

            builder.AddDeveloperSigningCredential();
        }

        public void Configure(IApplicationBuilder app)
        {
            
            
            using (IServiceScope scope = app.ApplicationServices.CreateScope())
            {
                IdentityDbContext database = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
                database.Database.Migrate();

                PersistentStoreDbContext persistentDb = scope.ServiceProvider.GetRequiredService<PersistentStoreDbContext>();
                persistentDb.Database.Migrate();

                RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var role = roleManager.FindByIdAsync("ADMIN").Result;
                if (role == null)
                {
                    var result = roleManager.CreateAsync(new IdentityRole()
                    {
                        Id = "ADMIN",
                        Name = "Administrator"
                    }).Result;
                }

                UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                ApplicationUser foundUser = userManager.FindByIdAsync("ADMIN-ID").Result;
                if (foundUser == null)
                {
                    AdminUserSettings settings = Configuration.GetSection(nameof(AdminUserSettings)).Get<AdminUserSettings>();

                    if (settings?.Email == null || settings?.Password == null)
                        throw new Exception("Must provide a AdminUserSettings to create de Admin user on first run.");

                    ApplicationUser user = new ApplicationUser()
                    {
                        Id = "ADMIN-ID",
                        Email = settings.Email,
                        EmailConfirmed = settings.EmailConfirmed,
                        LockoutEnabled = settings.LockoutEnabled,
                        UserName = settings.UserName,
                    };
                    IdentityResult result = userManager.CreateAsync(user, settings.Password).Result;

                    if (result.Succeeded)
                    {
                        IdentityResult roleResult = userManager.AddToRoleAsync(user, "Administrator").Result;
                    }
                }
            }

            if (Environment.IsEnvironment("Local") || Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var supportedCultures = new[]
            {
                new CultureInfo("en"),
                new CultureInfo("es"),
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("es"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            // uncomment if you want to add MVC
            app.UseStaticFiles();
            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }
    }
}
