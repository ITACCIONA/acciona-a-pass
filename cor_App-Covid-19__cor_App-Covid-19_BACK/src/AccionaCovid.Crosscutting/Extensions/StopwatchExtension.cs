using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AccionaCovid.Crosscutting
{
    /// <summary>
    /// Clase extension de Stopwatch
    /// </summary>
    public static class StopwatchExtension
    {
        /// <summary>
        /// Envia al log el tiempo de ejecucion de una operacion
        /// </summary>
        /// <param name="stopWatch"></param>
        /// <param name="operationName"></param>
        /// <param name="logger"></param>
        public static void SendTimeOperation(this Stopwatch stopWatch, string operationName, ILogger logger)
        {
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);

            logger.LogInformation($"TIME OPERATION [{operationName}] -> {elapsedTime} -> {stopWatch.ElapsedMilliseconds} ms");
            stopWatch.Restart();
        }
    }
}
