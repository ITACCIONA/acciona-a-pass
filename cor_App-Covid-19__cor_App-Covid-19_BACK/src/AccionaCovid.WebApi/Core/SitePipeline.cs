using AccionaCovid.WebApi.Utils;
using Microsoft.AspNetCore.Builder;
using System.Collections.Generic;

namespace AccionaCovid.WebApi.Core
{
    /// <summary>
    /// Clase estática que controla el flujo de pipeline del startup.
    /// </summary>
    public static class SitePipeline
    {
        /// <summary>
        /// Configura el pipeline de swagger.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSwaggerPipeline(this IApplicationBuilder app, InfrastructureSettings settings)
        {

            SwaggerOptions swaggerClient = settings.Authentication.AzureADSwaggerClient;
            if (swaggerClient.Enabled)
            {
                app.UseSwagger(c => c.RouteTemplate = "docs/{documentName}/swagger.json")
                   .UseSwaggerUI(c =>
                   {
                       c.SwaggerEndpoint("/docs/v1/swagger.json", "Acciona Covid-19 WebApi");
                       if (settings.Authentication.Enabled)
                       {
                           c.OAuthClientId(swaggerClient.ClientId);
                           c.OAuthClientSecret(swaggerClient.ClientSecret);
                           c.OAuthAdditionalQueryStringParams(new Dictionary<string, string>
                            {
                                { "resource", settings.Authentication.AzureADApiAuthentication.Backend.ClientId }
                            });
                       }
                   });
            }

            return app;
        }
    }
}
