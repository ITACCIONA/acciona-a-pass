using AccionaCovid.Application.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccionaCovid.WebApi.Core
{
    /// <summary>
    /// Respresenta una excepción
    /// </summary>
    public class ExceptionResponse
    {
        /// <summary>
        /// El mensaje de la excecpión
        /// </summary>
        public IEnumerable<ErrorMessage> Messages { get; set; }

        /// <summary>
        /// Cualquier información extra que se quiera añadir
        /// </summary>
        public object ExtraInfo { get; set; }
    }
}
