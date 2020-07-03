using System;
using System.Collections.Generic;
using System.Resources;

namespace AccionaCovid.Application.Core
{
    /// <summary>
    /// Excepcion para mensajes de validación, produce un BadRequest en ved de un InternalServerError
    /// </summary>
    public class MultiMessageLogValidationException : MultiMessageException
    {
        /// <summary>
        /// Contructor por defecto para herencia
        /// </summary>
        protected MultiMessageLogValidationException() : base() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Mensaje que se quiere incluir en la excepción</param>
        public MultiMessageLogValidationException(ErrorMessage message) : base(message) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Mensaje que se quiere incluir en la excepción</param>
        public MultiMessageLogValidationException(ResourceManager manager, string code) : base(manager, code) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="exToFlat">Lista de mensajes que se quieren incluir en la excepción</param>
        public MultiMessageLogValidationException(Exception exToFlat) : base(exToFlat) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="list">Lista de mensajes que se quieren incluir en la excepción</param>
        public MultiMessageLogValidationException(ResourceManager manager, List<String> codes) : base(manager, codes) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="list">Lista de mensajes que se quieren incluir en la excepción</param>
        public MultiMessageLogValidationException(List<ErrorMessage> list) : base(list) { }
    }
}
