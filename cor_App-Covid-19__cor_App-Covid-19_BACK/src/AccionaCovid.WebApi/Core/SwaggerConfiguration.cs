using AccionaCovid.WebApi.Core;
using AccionaCovid.WebApi.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Linq;

namespace AccionaCovid.WebApi
{
    /// <summary>
    /// Clase que representa la configuracion de swagger
    /// </summary>
    public static class SwaggerConfiguration
    {
        /// <summary>
        /// Custom configuration for Swagger para mock (cuando se desea añader manualmente la header) (No hay AAD)
        /// </summary>
        /// <param name="services"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureSwaggerMock(this IServiceCollection services, ApiAuthentication settings)
        {
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            IWebHostEnvironment env = serviceProvider.GetRequiredService<IWebHostEnvironment>();

            services.AddSwaggerGen(c =>
            {
                var dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                foreach (var fi in dir.EnumerateFiles("*.xml"))
                {
                    c.IncludeXmlComments(fi.FullName);
                }

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Acciona Covid-19", Version = $"v1.{env.EnvironmentName}", Description = "WEB API" });
                c.OperationFilter<ResponseTypeOperationFilter>();
                c.DocumentFilter<SwaggerAddEnumDescriptions>();
            });

            return services;
        }

        /// <summary>
        /// Custom configuration for Swagger
        /// </summary>
        /// <param name="services"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services, IWebHostEnvironment environment, InfrastructureSettings infrastructureSettings)
        {
            SwaggerOptions identityServerSwaggerClient = infrastructureSettings.Authentication.IdentityServerSwaggerClient;
            SwaggerOptions azureADSwaggerClient = infrastructureSettings.Authentication.AzureADSwaggerClient;
            if (identityServerSwaggerClient.Enabled)
            {
                services.AddSwaggerGen(c =>
                {
                    var dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                    foreach (var fi in dir.EnumerateFiles("*.xml"))
                    {
                        c.IncludeXmlComments(fi.FullName);
                    }

                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Acciona Covid-19", Version = $"v1.{environment.EnvironmentName}", Description = "WEB API" });
                    c.OperationFilter<ResponseTypeOperationFilter>();
                    c.DocumentFilter<SwaggerAddEnumDescriptions>();

                    if (infrastructureSettings.Authentication.Enabled)
                    {
                        //*************************************************************************************************
                        //************** AUTENTICACION SWAGGER ************************************************************
                        //https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/master/README.md
                        //https://joonasw.net/view/testing-azure-ad-protected-apis-part-1-swagger-ui

                        c.AddSecurityDefinition("oauth2_IdentityServer", new OpenApiSecurityScheme
                        {
                            Type = SecuritySchemeType.OAuth2,
                            Flows = new OpenApiOAuthFlows
                            {
                                Implicit = new OpenApiOAuthFlow
                                {
                                    AuthorizationUrl = new Uri(identityServerSwaggerClient.AuthorizationUrl),
                                    TokenUrl = new Uri(identityServerSwaggerClient.TokenUrl),
                                    Scopes = identityServerSwaggerClient.Scopes
                                }
                            }
                        });

                        c.AddSecurityDefinition("oauth2_AzureAD", new OpenApiSecurityScheme
                        {
                            Type = SecuritySchemeType.OAuth2,
                            Flows = new OpenApiOAuthFlows
                            {
                                Implicit = new OpenApiOAuthFlow
                                {
                                    AuthorizationUrl = new Uri($"https://foo-login.bar/{infrastructureSettings.Authentication.AzureADApiAuthentication.Tenant}/oauth2/authorize", UriKind.Absolute),
                                    Scopes = azureADSwaggerClient.Scopes,
                                },
                            },
                        });

                        c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme()
                        {
                            Type = SecuritySchemeType.ApiKey,
                            In = ParameterLocation.Header,
                            Name = "X-Api-Key",
                        });

                        c.AddSecurityRequirement(new OpenApiSecurityRequirement
                        {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2_IdentityServer" }
                                },
                                identityServerSwaggerClient.Scopes.Keys.ToArray()
                            },
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2_AzureAD" }
                                },
                                azureADSwaggerClient.Scopes.Keys.ToArray()
                            },
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ApiKey" }
                                },
                                new string[] { }
                            }
                        });
                    }
                });
            }

            return services;
        }
    }
}
