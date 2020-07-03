using AccionaCovid.Domain.Core;
using System.Collections.Generic;

namespace AccionaCovid.WebApi.Utils
{
    /// <summary>
    /// Opciones de configuración de la aplicación
    /// </summary>
    public class InfrastructureSettings : InfrastructureCoreSettings
    {
        /// <summary>
        /// Opciones de configuración de autenticación JWT
        /// </summary>
        public ApiAuthentication Authentication { get; set; } = new ApiAuthentication { Enabled = false };
    }

    /// <summary>
    /// Opciones de configuración de la autenticación JWT
    /// </summary>
    public class ApiAuthentication
    {
        /// <summary>
        /// Establece el estado de actividad de la seguridad de la aplicación (con fines de desarrollo)
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Api secret for Security App
        /// </summary>
        public string ApiSecret { get; set; }

        public AzureADApiAuthentication AzureADApiAuthentication { get; set; } = new AzureADApiAuthentication { Enabled = false };

        /// <summary>
        /// Opciones de Swagger
        /// </summary>
        public SwaggerOptions IdentityServerSwaggerClient { get; set; } = new SwaggerOptions() { Enabled = true };

        /// <summary>
        /// Opciones de Swagger
        /// </summary>
        public SwaggerOptions AzureADSwaggerClient { get; set; } = new SwaggerOptions() { Enabled = true };
    }

    public class AzureADApiAuthentication
    {
        /// <summary>
        /// Establece el estado de actividad de la seguridad de la aplicación (con fines de desarrollo)
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// URL del Login AAD
        /// </summary>
        public string AadInstance { get; set; }

        /// <summary>
        /// Id. de directorio
        /// </summary>
        public string Tenant { get; set; }

        /// <summary>
        /// Id. de directorio para Graph
        /// </summary>
        public string AssertionType { get; set; }

        /// <summary>
        /// Id. de directorio para Graph
        /// </summary>
        public string ResourceGraph { get; set; }

        /// <summary>
        /// URL de autorizacion
        /// </summary>
        public string Authority => AadInstance + Tenant;

        /// <summary>
        /// Opciones del Backend
        /// </summary>
        public AuthenticationOptionsApi Backend { get; set; }
    }

    /// <summary>
    /// Opciones del Backend
    /// </summary>
    public class AuthenticationOptionsApi
    {
        /// <summary>
        /// Id. de aplicación (cliente)
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// URI de id. de aplicación
        /// </summary>
        public string AppIdUri { get; set; }

        /// <summary>
        /// Secreto del cliente a usar para consultar a Graph
        /// </summary>
        public string ClientSecret { get; set; }
    }

    /// <summary>
    /// Opciones del Frontal
    /// </summary>
    public class AuthenticationOptionsFront
    {
        /// <summary>
        /// Id. de aplicación (cliente)
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Secreto del cliente a usar
        /// </summary>
        public string ClientSecret { get; set; }
    }

    /// <summary>
    /// Opciones del Frontal
    /// </summary>
    public class SwaggerOptions
    {
        /// <summary>
        /// Establece el estado del uso de Swagger
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// ClientId de swagger
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// ClientSecret de swagger
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Id. de aplicación (cliente)
        /// </summary>
        public string AuthorizationUrl { get; set; }

        /// <summary>
        /// Secreto del cliente a usar
        /// </summary>
        public string TokenUrl { get; set; }

        /// <summary>
        /// Secreto del cliente a usar
        /// </summary>
        public Dictionary<string, string> Scopes { get; set; }
    }
}
