using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccionaCovid.WebApi.Core
{
    /// <summary>
    /// Clase base para los controllers que hacen de proxy
    /// </summary>
    public class ApiControllerBaseProxy : ControllerBase
    {
        /// <summary>
        /// Propiedad que representa los metodos de comunicacion con otros apis
        /// </summary>
        private HttpProxy proxy;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpProxy">Propiedad que representa los metodos de comunicacion con otros apis</param>
        public ApiControllerBaseProxy(HttpProxy httpProxy)
        {
            proxy = httpProxy;
        }

        /// <summary>
        /// Envía las peticiones del controlador.
        /// NOTA: Este metodo tiene que ser protected debido a que si es publico swagger se piensa que es un metodo mas del Api y da error de api definition
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected async Task<IActionResult> ForwardProxy(string url)
        {
            try
            {
                return await proxy.SendCustomAsync(url, Request).ConfigureAwait(false);
            }
            catch (HttpRequestException ex)
            {
                return new HttpResponseMessageResult(ex);
            }
            catch (Exception ex)
            {
                return new HttpResponseMessageResult(ex);
            }
        }
    }
}
