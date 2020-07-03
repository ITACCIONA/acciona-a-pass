using AccionaCovid.Application.Core;
using System;
using System.Collections.Generic;
using System.Resources;

namespace AccionaCovid.Application.Core
{
    /// <summary>
    /// Excepcion para mensajes de validación en offline, produce un 200 y el mensaje de error
    /// </summary>
    public class MessageOfflineValidationException : MultiMessageException
    {
        /// <summary>
        /// Contructor por defecto para herencia
        /// </summary>
        protected MessageOfflineValidationException() : base() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Mensaje que se quiere incluir en la excepción</param>
        public MessageOfflineValidationException(ErrorMessage message) : base(message) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Mensaje que se quiere incluir en la excepción</param>
        public MessageOfflineValidationException(ResourceManager manager, string code) : base(manager, code) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="exToFlat">Lista de mensajes que se quieren incluir en la excepción</param>
        public MessageOfflineValidationException(Exception exToFlat) : base(exToFlat) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="list">Lista de mensajes que se quieren incluir en la excepción</param>
        public MessageOfflineValidationException(ResourceManager manager, List<String> codes) : base(manager, codes) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="list">Lista de mensajes que se quieren incluir en la excepción</param>
        public MessageOfflineValidationException(List<ErrorMessage> list) : base(list) { }
    }
}
