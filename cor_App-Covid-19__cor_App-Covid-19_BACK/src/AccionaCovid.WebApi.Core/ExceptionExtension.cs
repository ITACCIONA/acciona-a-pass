using System;
using System.Collections.Generic;
using System.Text;

namespace AccionaCovid.WebApi.Core
{
    /// <summary>
    /// Clase para extender compartamiento de objetos tipo <see cref="Exception"/>.
    /// </summary>
    public static class ExceptionExtension
    {
        private const string ErrMessage = "An error occurred while updating the entries. See the inner exception for details.";

        /// <summary>
        /// Método que devuelve la cadena : "An error occurred while updating the entries. See the inner exception for details.".
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetGenericMessage(this Exception e)
        {
            return ErrMessage;
        }

        /// <summary>
        /// Metodo para ampliar el mensaje de error profundizando en la instancia InnerException.
        /// </summary>
        /// <param name="e"></param>        
        public static string GetInnerExceptionMessages(this Exception e)
        {
            var response = new StringBuilder();

            if (!string.Equals(e.Message, ErrMessage, StringComparison.InvariantCultureIgnoreCase))
                response.AppendLine(e.Message);

            var next = true;

            var e2 = e.InnerException;

            while (next)
            {
                if (string.IsNullOrWhiteSpace(e2?.Message))
                    next = false;
                else
                {
                    if (!string.Equals(e2.Message, ErrMessage, StringComparison.InvariantCultureIgnoreCase))
                    {
                        response.AppendLine(e2.Message);

                        var exception = e2;

                        if (exception != null)
                        {
                            response = new StringBuilder();
                            response.Append(exception.Message);
                            break;
                        }
                    }

                    e2 = e2.InnerException;
                }
            }

            return response.ToString();
        }
    }
}
