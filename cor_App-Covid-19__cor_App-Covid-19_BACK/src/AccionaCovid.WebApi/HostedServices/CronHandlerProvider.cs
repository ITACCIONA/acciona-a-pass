using Cronos;
using AccionaCovid.WebApi.Core;
using System;

namespace AccionaCovid.WebApi.HostedServices
{
    /// <summary>
    /// Manejador de expresiones cron
    /// </summary>
    public class CronHandlerProvider : ICronHandlerProvider
    {
        CronExpression _cronExpression;

        /// <summary>
        /// Expression cron a utilizar
        /// </summary>
        public string Expression
        {
            set
            {
                _cronExpression = CronExpression.Parse(value);
            }
        }

        /// <summary>
        /// Obtiene la siguiente marca temporal a partir del momento dado
        /// </summary>
        /// <param name="timeInstance">momento en el tiempo a partir del cual se realizará el cálculo de la siguiente marca temporal</param>
        /// <param name="timeZoneInfo">Zona horaria para el cálculo</param>
        /// <returns></returns>
        public DateTimeOffset? GetNextOccurrence(DateTimeOffset timeInstance, TimeZoneInfo timeZoneInfo)
            => _cronExpression?.GetNextOccurrence(timeInstance, timeZoneInfo);
    }
}
