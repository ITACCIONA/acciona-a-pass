using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccionaCovid.WebApi.Core
{
    /// <summary>
    /// Clase IActionResult generica que transforma un HTTPMessageResponse a IActionResult
    /// </summary>
    public class HttpResponseMessageResult : IActionResult
    {
        /// <summary>
        /// HttpResponseMessage a transformar
        /// </summary>
        private readonly HttpResponseMessage _responseMessage;

        /// <summary>
        /// Crea el mensaje de respuesta.
        /// </summary>
        /// <param name="ex"></param>
        public HttpResponseMessageResult(Exception ex)
        {
            HttpResponseMessage messageError = new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                Content = new StringContent(ex.GetBaseException().Message)
            };

            _responseMessage = messageError; // could add throw if null
        }

        /// <summary>
        /// Crea el mensaje de respuesta.
        /// </summary>
        /// <param name="responseMessage"></param>
        public HttpResponseMessageResult(HttpResponseMessage responseMessage)
        {
            _responseMessage = responseMessage; // could add throw if null
        }

        /// <summary>
        /// Crea el mensaje de respuesta.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)_responseMessage.StatusCode;
            context.HttpContext.Response.ContentType = "application/json";

            var s = _responseMessage.Content.ReadAsStreamAsync().Result;

            var objectResult = new ObjectResult(s);

            await objectResult.ExecuteResultAsync(context);
        }
    }
}
