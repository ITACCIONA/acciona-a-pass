using System;
using System.Linq;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;

namespace AccionaCovid.WebApi.Core
{
    /// <summary>
    /// Añade el tipo de respuesta al Swagger
    /// </summary>
    public class ResponseTypeOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Añade el tipo de respuesta cuando es un IRequest
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            Type requestInterface = context.ApiDescription.ParameterDescriptions
                .SelectMany(p => p.Type.GetInterfaces()
                    .Where(i => i.Name.StartsWith("IRequest", StringComparison.InvariantCulture)
                        && i.IsGenericType))
                    .FirstOrDefault();

            if (requestInterface != null)
            {
                Type responseType = requestInterface.GetGenericArguments()[0];
                const string key = "200";

                if (!operation.Responses.TryGetValue(key, out OpenApiResponse response))
                {
                    response = new OpenApiResponse();
                }

                context.SchemaGenerator.GenerateSchema(responseType, context.SchemaRepository);

                operation.Responses[key] = response;
            }
        }
    }
}
