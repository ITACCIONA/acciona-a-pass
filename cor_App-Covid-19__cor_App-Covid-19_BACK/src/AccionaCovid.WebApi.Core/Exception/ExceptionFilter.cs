using AccionaCovid.Application.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace AccionaCovid.WebApi.Core
{
    /// <summary>
    /// Handler global de excepciones
    /// </summary>
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> logger;

        /// <summary>
        /// Constructor
        /// </summary>
        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Método que captura las excepciones no controlada
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            Exception ex = context.Exception;

            HttpResponse response = context.HttpContext.Response;

            if (ex is MessageOfflineValidationException)
            {
                response.StatusCode = (int)HttpStatusCode.OK;

                MessageOfflineValidationException multi = ex as MessageOfflineValidationException;
                var exceptionResponse = new GenericResponse<object>(multi);

                context.Result = new JsonResult(exceptionResponse);
            }
            else if (ex is MultiMessageValidationException)
            {
                response.StatusCode = (int)HttpStatusCode.BadRequest;

                MultiMessageValidationException multi = ex as MultiMessageValidationException;
                var exceptionResponse = new GenericResponse<object>(multi);

                context.Result = new JsonResult(exceptionResponse);
            }
            else if (ex is MultiMessageLogValidationException)
            {
                response.StatusCode = (int)HttpStatusCode.BadRequest;

                MultiMessageLogValidationException multi = ex as MultiMessageLogValidationException;
                var exceptionResponse = new GenericResponse<object>(multi);

                context.Result = new JsonResult(exceptionResponse);

                logger.LogWarning(ex, $"[ERR_VAL] Error de Validación: {string.Join(", ", multi.Messages.Select(c => c.Message).ToList())}");
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;

                MultiMessageException multi = ex as MultiMessageException ?? new MultiMessageException(ex);

                var exceptionResponse = new GenericResponse<object>(multi);

                context.Result = new JsonResult(exceptionResponse);

                logger.LogError(ex, $"Error de aplicación: {string.Join(", ", multi.Messages.Select(c => c.Message).ToList())}");
            }

            context.ExceptionHandled = true;
        }
    }
}
