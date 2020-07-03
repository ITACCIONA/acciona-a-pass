using AccionaCovid.Crosscutting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;

namespace AccionaCovid.Application.Core
{
    /// <summary>
    /// Excepción para incluir lista de mensajes
    /// </summary>
    public class MultiMessageException : Exception
    {
        public static ResourceManager DefaultApplicationResource { get; set; }

        public static void Throw(string code) => throw new MultiMessageException(DefaultApplicationResource, code);

        /// <summary>
        /// La lista de mensajes internos de la excepción
        /// </summary>
        private readonly List<ErrorMessage> messages;

        /// <summary>
        /// Obtiene o establece la lista de mensajes internos de la excepción
        /// </summary>
        public IEnumerable<ErrorMessage> Messages => messages;

        /// <summary>
        /// Obtiene el mensaje concatenando los mensajes de Messages
        /// </summary>
        private readonly Lazy<string> message;

        /// <summary>
        /// Obtiene el mensaje concatenando los mensajes de Messages
        /// </summary>
        public override string Message => message.Value;

        /// <summary>
        /// Contructor por defecto para herencia
        /// </summary>
        protected MultiMessageException() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Mensaje que se quiere incluir en la excepción</param>
        public MultiMessageException(ErrorMessage message) : this(new List<ErrorMessage>() { message }) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Mensaje que se quiere incluir en la excepción</param>
        public MultiMessageException(ResourceManager manager, string code) : this(manager, new List<string>() { code }) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="exToFlat">Lista de mensajes que se quieren incluir en la excepción</param>
        public MultiMessageException(Exception exToFlat) : this(new List<ErrorMessage>())
        {
            AddInnerExceptions(exToFlat);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="list">Lista de mensajes que se quieren incluir en la excepción</param>
        public MultiMessageException(ResourceManager manager, List<String> codes)
        {
            codes.ThrowIfNull(nameof(codes));
            manager.ThrowIfNull(nameof(manager));

            this.messages = codes.Select(c => new ErrorMessage()
            {
                Code = c,
                Message = manager.GetString(c, CultureInfo.CurrentCulture) ?? "NO_MESSAGE"
            }).ToList();

            this.message = new Lazy<string>(() => this.messages.Aggregate(new StringBuilder(50 * this.messages.Count),
                    (sb, m) => sb.Append(m.Code).Append(": ").Append(m.Message).AppendLine(","))
                .Modify(sb => sb.Length--).ToString());
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="list">Lista de mensajes que se quieren incluir en la excepción</param>
        public MultiMessageException(List<ErrorMessage> list)
        {
            list.ThrowIfNull(nameof(list));

            this.messages = list;
            this.message = new Lazy<string>(() => this.messages.Aggregate(new StringBuilder(50 * this.messages.Count),
                    (sb, m) => sb.Append(m.Code).Append(": ").Append(m.Message).AppendLine(","))
                .Modify(sb => sb.Length--).ToString());
        }

        private void AddInnerExceptions(Exception ex)
        {
            if (ex == null) return;
            this.messages.Add(new ErrorMessage() { Code = "UNEXPECTED_EXCEPTION", Message = ex.Message });
            AddInnerExceptions(ex.InnerException);
        }
    }

    public class ErrorMessage
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
