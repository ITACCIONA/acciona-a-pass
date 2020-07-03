using AccionaCovid.Application.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccionaCovid.WebApi.Core
{
    /// <summary>
    /// Metodos de ayuda para las respuestas.
    /// </summary>
    public static class ResponseHelper
    {
        /// <summary>
        /// Crea una respuesta genérica.
        /// </summary>
        /// <typeparam name="T">Tipo de la respuesta</typeparam>
        /// <param name="response">Valor de las respuesta.</param>
        /// <returns>Respuesta de tipo IActionResult.</returns>
        public static IActionResult CreateResponse<T>(T response) where T : class
        {
            var genericResponse = new GenericResponse<T>(response);
            return new OkObjectResult(genericResponse);
        }

        /// <summary>
        /// Crea una respuesta genérica paginada.
        /// </summary>
        /// <typeparam name="T">Tipo de la respuesta.</typeparam>
        /// <param name="response">Valor de las respuesta en forma de lista.</param>
        /// <returns>Respuesta de tipo IActionResult.</returns>
        public static IActionResult CreateResponse<T>(IList<T> response) where T : class
        {
            return CreateResponse(response.ToList());
        }

        /// <summary>
        /// Crea una respuesta genérica paginada.
        /// </summary>
        /// <typeparam name="T">Tipo de la respuesta.</typeparam>
        /// <param name="response">Valor de las respuesta en forma de lista.</param>
        /// <returns>Respuesta de tipo IActionResult.</returns>
        public static IActionResult CreateResponse<T>(List<T> response) where T : class
        {
            var genericResponse = new GenericResponse<T>(response);
            return new OkObjectResult(genericResponse);
        }

        ///// <summary>
        ///// Crea una respuesta genérica paginada.
        ///// </summary>
        ///// <typeparam name="T">Tipo de la respuesta.</typeparam>
        ///// <param name="response">Valor de las respuesta en forma de lista.</param>
        ///// <returns>Respuesta de tipo IActionResult.</returns>
        //public static IActionResult CreateResponse<T>(PagedList<T> response) where T : class
        //{
        //    var genericResponse = new GenericResponse<T>(response);
        //    return new OkObjectResult(genericResponse);
        //}

        /// <summary>
        /// Crea una respuesta booleana.
        /// </summary>
        /// <param name="response">Valor de las respuesta booleana.</param>
        /// <returns>Respuesta de tipo HttpResponseMessage.</returns>
        public static IActionResult CreateResponse(bool response)
        {
            var genericResponse = new GenericResponse<string>(response);
            return new OkObjectResult(genericResponse);
        }

        /// <summary>
        /// Crea un objeto <see cref="GenericResponse{T}"/> con lista de mensajes y con respuesta pasada como parametro.
        /// </summary>
        /// <param name="success">Indica si la respuesta es satisfactoria o no.</param>
        /// <param name="info">Mensajes a mostrar por pantalla.</param>
        /// <returns>Respuesta de tipo IActionResult</returns>
        public static IActionResult CreateResponse(bool success, List<ErrorMessage> info)
        {

            var genericResponse = new GenericResponse<object> { Data = null, Info = info, Success = success };
            return new OkObjectResult(genericResponse);
        }

        /// <summary>
        /// Crea una respuesta genérica.
        /// </summary>
        /// <typeparam name="T">Tipo de la respuesta.</typeparam>
        /// <param name="response">Valor de las respuesta.</param>
        /// <param name="info"></param>
        /// <returns>Respuesta de tipo IActionResult</returns>
        public static IActionResult CreateResponse<T>(T response, string info) where T : class
        {
            var genericResponse = new GenericResponse<T>(response);

            if (!string.IsNullOrWhiteSpace(info))
                genericResponse.Info = new List<ErrorMessage> { new ErrorMessage() { Code = "NO_CODE", Message = info } };

            return new OkObjectResult(genericResponse);
        }

        /// <summary>
        /// Crea un objeto <see cref="GenericResponse{T}"/> con lista de mensajes y con respuesta pasada como parametro.
        /// </summary>
        /// <param name="ex">Indica si la respuesta es satisfactoria o no.</param>
        /// <returns>Respuesta de tipo IActionResult</returns>
        public static IActionResult CreateResponse(MultiMessageValidationException ex)
        {

            var genericResponse = new GenericResponse<object>(ex);
            return new OkObjectResult(genericResponse);
        }

        /// <summary>
        /// Crea una respuesta de devolución de fichero
        /// </summary>
        /// <param name="file">Array de bytes del fichero</param>
        /// <param name="fileName">Nombre del fichero</param>
        /// <returns></returns>
        public static IActionResult CreateFileResponse(byte[] file, string fileName)
        {
            var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }

            return new FileContentResult(file, contentType)
            {
                FileDownloadName = fileName,
                LastModified = DateTime.Now
            };
        }
    }
}
