using System;
using System.Collections.Generic;
using System.Resources;

namespace AccionaCovid.Application.Core
{
    /// <summary>
    /// Excepcion para mensajes de validación, produce un BadRequest en ved de un InternalServerError
    /// </summary>
    public class MultiMessageValidationException : MultiMessageException
    {
        /// <summary>
        /// Contructor por defecto para herencia
        /// </summary>
        protected MultiMessageValidationException() : base() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Mensaje que se quiere incluir en la excepción</param>
        public MultiMessageValidationException(ErrorMessage message) : base(message) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Mensaje que se quiere incluir en la excepción</param>
        public MultiMessageValidationException(ResourceManager manager, string code) : base(manager, code) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="exToFlat">Lista de mensajes que se quieren incluir en la excepción</param>
        public MultiMessageValidationException(Exception exToFlat) : base(exToFlat) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="list">Lista de mensajes que se quieren incluir en la excepción</param>
        public MultiMessageValidationException(ResourceManager manager, List<String> codes) : base(manager, codes) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="list">Lista de mensajes que se quieren incluir en la excepción</param>
        public MultiMessageValidationException(List<ErrorMessage> list) : base(list) { }
    }
}
