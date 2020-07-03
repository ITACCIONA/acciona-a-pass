using System;

namespace AccionaCovid.WebApi.Core
{
    /// <summary>
    /// Manejador de expressiones cron
    /// </summary>
    public interface ICronHandlerProvider
    {
        /// <summary>
        /// Expression cron
        /// </summary>
        string Expression { set; }

        /// <summary>
        /// Obtiene la siguiente marca temporal a partir del momento dado
        /// </summary>
        /// <param name="timeInstance">momento en el tiempo a partir del cual se realizará el cálculo de la siguiente marca temporal</param>
        /// <param name="timeZoneInfo">Zona horaria para el cálculo</param>
        /// <returns></returns>
        DateTimeOffset? GetNextOccurrence(DateTimeOffset timeInstance, TimeZoneInfo timeZoneInfo);
    }
}