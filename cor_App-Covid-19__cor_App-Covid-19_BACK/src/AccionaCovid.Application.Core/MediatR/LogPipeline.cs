using AccionaCovid.Crosscutting;
using AccionaCovid.Domain.Core;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccionaCovid.Application.Core
{
    /// <summary>
    /// Clase decoradora mediar para generar los logs necesario antes de llamar a la capa de apliacion.
    /// Clase interceptora
    /// </summary>
    /// <typeparam name="TRequest">Clase de solicitud</typeparam>
    /// <typeparam name="TResponse">Clase de respuesta</typeparam>
    public class LogPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        /// <summary>
        /// Campo que representa la instancia del handler
        /// </summary>
        private readonly IRequestHandler<TRequest, TResponse> inner;

        /// <summary>
        /// Logger usado
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// HttpContext para obtener el idioma enviado en el Header
        /// </summary>
        private readonly IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// Instancia del usuario de acceso
        /// </summary>
        private readonly IUserInfoAccesor userInfoAccesor;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="inner">Campo que representa la instancia del handler</param>
        /// <param name="logger"></param>
        public LogPipeline(IRequestHandler<TRequest, TResponse> inner, ILogger<IRequestHandler<TRequest, TResponse>> logger, IHttpContextAccessor httpContextAccessor, IUserInfoAccesor userInfoAccesor)
        {
            this.inner = inner ?? throw new ArgumentNullException(nameof(inner));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.userInfoAccesor = userInfoAccesor ?? throw new ArgumentNullException(nameof(userInfoAccesor));
        }

        /// <summary>
        /// Metodo manejadora de la clase. Este metodo sera invocado antes de llama a la funcion handler de la clase de aplicacion 
        /// </summary>
        /// <param name="request">Instancia de la clase de solicitud de la capa de aplicacion</param>
        /// <param name="next">Delegado que representa el siguiente decorador</param>
        /// <returns>Respuesta de la funcion</returns>
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            logger.LogInformation($"Executing command {inner.GetType().Name} {new { Info = inner.GetType().Name, Request = request }}");

            try
            {
                // añado el lenguaje si el handler hereda de BaseCommandHandler
                if (inner is BaseCommandHandler<TRequest, TResponse> handler)
                {
                    if (httpContextAccessor.HttpContext != null)
                    {
                        // añado el lenguaje
                        var languages = httpContextAccessor.HttpContext.Request.GetTypedHeaders()
                           .AcceptLanguage
                           ?.OrderByDescending(x => x.Quality ?? 1)
                           .Select(x => x.Value.ToString())
                           .ToArray() ?? Array.Empty<string>();

                        List<string> langs = languages.Intersect(AllowedLanguages.Instance.Languages.Select(c => c.Name)).ToList();
                        handler.Idioma = langs != null && langs.Any() ? langs.First() : AllowedLanguages.Instance.Languages.First().Name;

                        ((BaseCommandHandler<TRequest, TResponse>)inner).Idioma = langs != null && langs.Any() ? langs.First() : AllowedLanguages.Instance.Languages.First().Name;
                    }
                    else
                    {
                        ((BaseCommandHandler<TRequest, TResponse>)inner).Idioma = AllowedLanguages.Instance.Languages.First().Name;
                    }

                    // añado datos del usuario
                    ((BaseCommandHandler<TRequest, TResponse>)inner).IdUser = userInfoAccesor.IdUser;
                    //((BaseCommandHandler<TRequest, TResponse>)inner).RoleName = userInfoAccesor.RoleName;
                    ((BaseCommandHandler<TRequest, TResponse>)inner).Logger = logger;
                    ((BaseCommandHandler<TRequest, TResponse>)inner).Roles = userInfoAccesor.Roles = userInfoAccesor.Roles;
                }

                var response = await next().ConfigureAwait(false);

                logger.LogInformation($"Command {inner.GetType().Name} executed successfully, with response: {response}");

                return response;
            }
            catch (Exception ex)
            {
                if (!(ex is MultiMessageValidationException) && !(ex is MultiMessageLogValidationException))
                {
                    logger.LogError(ex, $"Error captured in command {inner.GetType().Name}");
                }

                throw;
            }
        }
    }
}
