using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccionaCovid.WebApi.Core
{
    /// <summary>
    /// Clase de ayuda para realizar llamadas HTTP.
    /// </summary>
    public class HttpProxy
    {
        /// <summary>
        /// HttpClient.
        /// </summary>
        private readonly IHttpClientFactory clientFactory;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="clientFactory"></param>
        public HttpProxy(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        /// <summary>
        /// Llamada a la petición.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="originalRequest">Petición original.</param>
        /// <returns></returns>
        public async Task<HttpResponseMessageResult> SendCustomAsync(string url, HttpRequest originalRequest)
        {
            try
            {
                HttpClient client = clientFactory.CreateClient();
                HttpRequestMessage requestMessage = CreateProxyHttpRequest(new Uri(url), originalRequest);
                HttpResponseMessage response = await client.SendAsync(requestMessage).ConfigureAwait(false);

                return new HttpResponseMessageResult(response);
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    throw new Exception($"response :{responseContent}", ex);
                }
                throw;
            }
        }

        /// <summary>
        /// Llamada a la petición.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> SendCustomAsync(HttpRequestMessage requestMessage)
        {
            try
            {
                HttpClient client = clientFactory.CreateClient();
                HttpResponseMessage response = await client.SendAsync(requestMessage).ConfigureAwait(false);

                return response;
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    throw new Exception($"response :{responseContent}", ex);
                }
                throw;
            }
        }

        /// <summary>
        /// Crear la petición HTTP.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="original"></param>
        /// <returns></returns>
        private HttpRequestMessage CreateProxyHttpRequest(Uri uri, HttpRequest original)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage();
            string requestMethod = original.Method;

            if (!HttpMethods.IsGet(requestMethod) && !HttpMethods.IsHead(requestMethod) && !HttpMethods.IsDelete(requestMethod) && !HttpMethods.IsTrace(requestMethod))
            {
                try
                {
                    original.Body.Position = 0;
                    StreamContent streamContent = new StreamContent(original.Body);
                    requestMessage.Content = streamContent;
                }
                catch (Exception)
                {

                }
            }

            // Copy the request headers
            //foreach (var header in original.Headers)
            //{
            //    if (!requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray()) && requestMessage.Content != null)
            //    {
            //        requestMessage.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
            //    }
            //}

            requestMessage.Headers.Host = uri.Authority;
            requestMessage.RequestUri = uri;
            requestMessage.Method = new HttpMethod(original.Method);

            return requestMessage;
        }
    }
}
