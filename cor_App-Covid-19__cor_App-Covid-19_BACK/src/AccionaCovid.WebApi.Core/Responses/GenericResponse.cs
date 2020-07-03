using AccionaCovid.Application.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccionaCovid.WebApi.Core
{
    /// <summary>
    /// Clase para encapsular la comunicación con frontEnd.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericResponse<T> where T : class
    {
        /// <summary>
        /// Información.
        /// </summary>
        public List<ErrorMessage> Info { get; set; }

        /// <summary>
        /// Enumerable de objetos de tipo T.
        /// </summary>
        public List<T> Data { get; set; }

        /// <summary>
        /// Indica si la respuesta es satisfactoria o se ha producido algún error.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public GenericResponse() : this(true)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="succesfull">La respuesta.</param>
        public GenericResponse(bool succesfull)
        {
            Data = new List<T>();
            Info = null;
            Success = succesfull;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="response">La respuesta.</param>
        public GenericResponse(T response) : this(true)
        {
            if (response != null)
                Data = new List<T> { response };
            else
                Data = new List<T>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="listResponse">La lista que es la respuesta.</param>
        public GenericResponse(List<T> listResponse) : this(true)
        {
            if (listResponse != null && listResponse.Any())
                Data = listResponse;
            else
                Data = new List<T>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ex">La excepción.</param>
        public GenericResponse(MultiMessageValidationException ex) : this(false)
        {
            Info = ex.Messages.ToList();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ex">La excepción.</param>
        public GenericResponse(MultiMessageLogValidationException ex) : this(false)
        {
            Info = ex.Messages.ToList();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ex">La excepción a incluir en la respuesta.</param>
        public GenericResponse(Exception ex) : this(false)
        {
            var errorList = new List<string>();

            if (!string.Equals(ex.GetGenericMessage(), ex.Message, StringComparison.InvariantCultureIgnoreCase))
                errorList.Add(ex.Message);

            if (ex.InnerException != null)
                errorList.Add(ex.GetInnerExceptionMessages());

            /* 
             * Si al final de recorrer completamente el objeto exception no encontramos
             * más que el mensaje génerico de error devuelvo el mensage de la excepción
             */
            Info = errorList.Any() ? errorList.Select(e => new ErrorMessage { Code = "NO_CODE", Message = e }).ToList()
                : new List<ErrorMessage> { new ErrorMessage { Code = "NO_CODE", Message = ex.Message } };
        }
    }
}
