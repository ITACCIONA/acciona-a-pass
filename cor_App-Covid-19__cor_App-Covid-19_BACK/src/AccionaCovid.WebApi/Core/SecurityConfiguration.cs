using AccionaCovid.Data;
using AccionaCovid.Domain.Model;
using AccionaCovid.WebApi.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace AccionaCovid.WebApi.Core
{
    public static class SecurityConfiguration
    {
        /// <summary>
        /// Añade la autorización personalizada para la web API de Covid-19
        /// </summary>
        /// <param name="services"></param>
        public static void AddCovid19Authorization(this IServiceCollection services)
        {
            services.AddAuthorization();
        }

        /// <summary>
        /// Añade la autenticación personalizada para la web API de Covid-19
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="infrastructureSettings"></param>
        public static void AddCovid19Authentication(this IServiceCollection services, IConfiguration configuration, InfrastructureSettings infrastructureSettings, IConfiguration config, Serilog.ILogger logger)
        {
            services.AddAuthentication()
                .AddIdentityServerAuthentication("IdentityServer", options =>
                {
                    configuration.Bind("InfrastructureSettings:Authentication:IdentityServerApiAuthentication", options);
                    options.JwtBearerEvents = new JwtBearerEvents
                    {
                        OnTokenValidated = async context =>
                        {
                            var claimIdentity = (ClaimsIdentity)context.Principal.Identity;

                            var userClaim = context.Principal.Claims.FirstOrDefault(c => c.Type == "IdUser");
                            bool isAdmin = context.Principal.Claims.Any(c => c.Type == "role" && c.Value == "Administrator");

                            /* If its an admin, it doesn't have an employee */
                            if (isAdmin)
                            {
                                logger.Information("{User} is Admin", userClaim);
                                return;
                            }

                            if (userClaim == null || userClaim.Value == "-1")
                            {
                                logger.Error("IDENTITY SERVER FAIL AUTHENTICATION -> UPN CLAIM not found in System");
                                context.Fail("Usuario no encontrado en el sistema");
                            }
                            else
                            {
                                var optionsBuilder = new DbContextOptionsBuilder<AccionaCovidContext>();
                                optionsBuilder.UseSqlServer(config.GetConnectionString("AccionaCovidConnection"));

                                using (ServiceProvider serviceProvider = services.BuildServiceProvider())
                                using (var dbContext = new AccionaCovidContext(optionsBuilder.Options, new UserInfoAccesor(), serviceProvider.GetService<IHttpContextAccessor>()))
                                {
                                    Empleado emp = await dbContext.Empleado
                                        .Include(c => c.EmpleadoRole)
                                            .ThenInclude(c => c.IdRoleNavigation)
                                    .FirstOrDefaultAsync(c => c.Id == int.Parse(userClaim.Value)).ConfigureAwait(false);

                                    if (emp != null)
                                    {
                                        logger.Information($"IDENTITY SERVER SUCCESFULL AUTHENTICATION -> IdEmpleado {userClaim.Value}");
                                        // añado los claims de roles
                                        foreach (var role in emp.EmpleadoRole)
                                        {
                                            claimIdentity.AddClaim(new Claim(claimIdentity.RoleClaimType, role.IdRoleNavigation.Nombre));
                                        }
                                    }
                                    else
                                    {
                                        logger.Error($"IDENTITY SERVER FAIL AUTHENTICATION -> IdEmpleado {userClaim.Value} not found in System");
                                        context.Fail("Usuario no econtrado en BBDD");
                                    }
                                }
                            }
                        },
                    };
                })
                .AddAzureADAuthentication("AzureAD", infrastructureSettings, config, services, logger)
                .AddApiKeySupport("ApiKey", infrastructureSettings.Authentication.ApiSecret);
        }

        /// <summary>
        /// Añade autenticación por Azure AD a partir de las settins de infraestructura.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="authenticationSchema"></param>
        /// <param name="infrastructureSettings"></param>
        public static AuthenticationBuilder AddAzureADAuthentication(this AuthenticationBuilder builder, string authenticationSchema, InfrastructureSettings infrastructureSettings, IConfiguration config, IServiceCollection services, Serilog.ILogger logger)
        {
            builder.AddJwtBearer(authenticationSchema, o =>
            {
                o.Authority = infrastructureSettings.Authentication.AzureADApiAuthentication.Authority;
                o.SaveToken = true;

                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidAudiences = new List<string> { infrastructureSettings.Authentication.AzureADApiAuthentication.Backend.ClientId, infrastructureSettings.Authentication.AzureADApiAuthentication.Backend.AppIdUri },
                    ValidateIssuer = false
                };

                o.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var claimIdentity = (ClaimsIdentity)context.Principal.Identity;

                        // obtengo info del empleado en la BBDD
                        var optionsBuilder = new DbContextOptionsBuilder<AccionaCovidContext>();
                        optionsBuilder.UseSqlServer(config.GetConnectionString("AccionaCovidConnection"));

                        using (ServiceProvider serviceProvider = services.BuildServiceProvider())
                        using (var dbContext = new AccionaCovidContext(optionsBuilder.Options, new UserInfoAccesor(), serviceProvider.GetService<IHttpContextAccessor>()))
                        {
                            var upnClaim = context.Principal.FindFirst(c => c.Type == ClaimTypes.Upn) ?? (context.Principal.FindFirst(c => c.Type == ClaimTypes.Email) ?? context.Principal.FindFirst(c => c.Type == "preferred_username"));

                            if (upnClaim != null)
                            {
                                Empleado emp = await dbContext.Empleado
                                    .Include(c => c.EmpleadoRole)
                                        .ThenInclude(c => c.IdRoleNavigation)
                                    .FirstOrDefaultAsync(c => c.Upn.Trim().ToUpper() == upnClaim.Value.Trim().ToUpper()).ConfigureAwait(false);

                                if (emp != null)
                                {
                                    // añado los claims internos
                                    claimIdentity.AddClaim(new Claim("IdUser", emp.Id.ToString()));
                                    claimIdentity.AddClaim(new Claim("UserFullName", emp.NombreCompleto));

                                    // añado los claims de roles
                                    foreach (var role in emp.EmpleadoRole)
                                    {
                                        claimIdentity.AddClaim(new Claim(claimIdentity.RoleClaimType, role.IdRoleNavigation.Nombre));
                                    }
                                }
                                else
                                {
                                    logger.Error($"AZURE AD FAIL AUTENTICATION -> UPN {upnClaim} not found in System");
                                    context.Fail("Usuario no encontrado en workday");
                                }
                            }
                            else
                            {
                                logger.Error($"AZURE AD FAIL AUTENTICATION -> UPN IS NULL {string.Join(",", context.Principal.Claims.Select(c => $"{c.Type} - {c.Value}"))}");
                                context.Fail("Usuario no encontrado en el sistema");
                            }
                        }
                    },
                };
            });

            return builder;
        }

        public static AuthenticationBuilder AddApiKeySupport(this AuthenticationBuilder builder, string authenticationSchema, string apiSecret)
        {
            return builder.AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(authenticationSchema, o =>
            {
                o.ApiSecret = apiSecret;
            });
        }
    }
}
