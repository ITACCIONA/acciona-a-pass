using Messenger;
using Newtonsoft.Json;
using Acciona.Data.Model;
using Acciona.Domain.Model;
using ServiceLocator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Acciona.Data.Repository.Base
{
    public abstract class ApiBaseService
    {

        public static bool DebugTraces = false;

        public enum HttpVerbMethod
        {
            Get,
            Post,
            Delete,
            Put
        }

        private readonly static HttpClient HttpClient = new HttpClient()// new NativeMessageHandler())
        {
            Timeout = DataConstants.ApiTimeout
        };

        protected IMessenger messenger;


        protected ApiBaseService()
        {            
            messenger = Locator.Current.GetService<IMessenger>();
        }

        protected void HandleSessionError(BaseResponse response)
        {

        }

        public string Version { get; set; }
        

        private void AddHeaders(HttpRequestMessage req,bool noCache)
        {
            String language = Locator.Current.GetService<AppSession>().Language;
            req.Headers.Add("Accept-Language", language);
            if (noCache)
            {
                HttpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
                {
                    NoCache = true
                };
            }
        }
      

        private void AddSessionHeaders(HttpRequestMessage req)
        {
            var appSession = Locator.Current.GetService<AppSession>();
            req.Headers.Add("Authorization", appSession.AccesToken);
        }

        private void AddXkeyHeaders(HttpRequestMessage req)
        {
            var appSession = Locator.Current.GetService<AppSession>();
            req.Headers.Add("x-api-key", DataConstants.XapiKey);
        }

        protected async Task<TOutput> MakeSessionHttpCall<TOutput, TInput>(string endpoint,
            HttpVerbMethod verbMethod,
            TInput input) where TOutput : BaseResponse
        {
            return await MakeHttpCall<TOutput, TInput>(endpoint, verbMethod, input, true);
        }

        protected async Task<TOutput> MakeApiKeyHttpCall<TOutput, TInput>(string endpoint,
            HttpVerbMethod verbMethod,
            TInput input) where TOutput : BaseResponse
        {
            return await MakeHttpCall<TOutput, TInput>(endpoint, verbMethod, input, apiKeyHeaders: true);
        }


        protected async Task<TOutput> MakeHttpCall<TOutput, TInput>(
            string endpoint,
            HttpVerbMethod verbMethod,
            TInput input,            
            bool sessionHeaders = false, bool apiKeyHeaders = false,
            string contentType = "application/json",bool xmlResponse=false, bool noCache = false) where TOutput : BaseResponse
        {

            HttpResponseMessage response = null;
            var responseText = string.Empty;

            try
            {
                var client = HttpClient;

                HttpContent content = null;
                var url = endpoint;
                if (DebugTraces)
                    Console.WriteLine("\r\n//////////////\r\nUrl:\r\n" + verbMethod.ToString()+" " +url);

                if (!Equals(input, default(TInput)))
                {
                    string stringContent=null;
                    if(contentType.Equals("application/json"))
                        stringContent= JsonConvert.SerializeObject(input);
                    else if (contentType.Equals("application/x-www-form-urlencoded"))
                        stringContent = input as string;
                    content = new StringContent(stringContent, Encoding.UTF8, contentType);
                    if (DebugTraces)
                        Console.WriteLine("Request:\r\n" + stringContent);
                }
                switch (verbMethod)
                {
                    case HttpVerbMethod.Get:
                        {
                            var rq = new HttpRequestMessage(HttpMethod.Get, url);
                            AddHeaders(rq,noCache);
                            if(sessionHeaders)
                                AddSessionHeaders(rq);
                            if (apiKeyHeaders)
                                AddXkeyHeaders(rq);
                            response = await client.SendAsync(rq);
                        }
                        break;
                    case HttpVerbMethod.Post:
                        {
                            if (content == null)
                                content = new StringContent("", Encoding.UTF8, "application/json");
                            var rq = new HttpRequestMessage(HttpMethod.Post, url)
                            {
                                Content = content
                            };
                            AddHeaders(rq,noCache);
                            if (sessionHeaders)
                                AddSessionHeaders(rq);
                            if (apiKeyHeaders)
                                AddXkeyHeaders(rq);
                            response = await client.SendAsync(rq);
                        }
                        break;
                    case HttpVerbMethod.Put:
                        {
                            if (content == null)
                                content = new StringContent("", Encoding.UTF8, "application/json");
                            var rq = new HttpRequestMessage(HttpMethod.Put, url)
                            {
                                Content = content
                            };
                            AddHeaders(rq,noCache);
                            if (sessionHeaders)
                                AddSessionHeaders(rq);
                            if (apiKeyHeaders)
                                AddXkeyHeaders(rq);
                            response = await client.SendAsync(rq);
                        }
                        break;
                    case HttpVerbMethod.Delete:
                        {
                            var rq = new HttpRequestMessage(HttpMethod.Delete, url)
                            {
                                Content = content
                            };
                            AddHeaders(rq,noCache);
                            if (sessionHeaders)
                                AddSessionHeaders(rq);
                            if (apiKeyHeaders)
                                AddXkeyHeaders(rq);
                            response = await client.SendAsync(rq);

                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(verbMethod), verbMethod, null);
                }

                responseText = await response.Content.ReadAsStringAsync();
                if (xmlResponse)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(responseText);
                    responseText = JsonConvert.SerializeXmlNode(doc.LastChild, Newtonsoft.Json.Formatting.None, true); //Soap el primer nodo es definicion xml
                }
                //response.EnsureSuccessStatusCode();     
                if (DebugTraces)
                    Console.WriteLine("Response:\r\n" + responseText+"\r\n//////////////\r\n");
                TOutput output;
                if (!responseText.Trim().StartsWith("{"))
                    responseText="{}";
                if (responseText.Trim().Length == 0)
                {
                    output = (TOutput)Activator.CreateInstance(typeof(TOutput));
                    output.status = (int)response.StatusCode;
                    output.Message = "Code " + (int)response.StatusCode + " from Server.";
                }
                else
                    output = JsonConvert.DeserializeObject<TOutput>(responseText);
                if (output.Message != null)
                    output.Message = HttpUtility.HtmlDecode(output.Message);
                if ((response.StatusCode != System.Net.HttpStatusCode.OK) && (response.StatusCode != System.Net.HttpStatusCode.Created) && (response.StatusCode != System.Net.HttpStatusCode.NoContent))
                {
                    output.HasError = true;
                    output.status = (int)response.StatusCode;
                }
                if (output.status == 401)
                {
                    if (sessionHeaders)
                    {
                        output.Message = "error_401";
                        var appSession = Locator.Current.GetService<AppSession>();
                        //appSession.AccesToken = null;
                        //appSession.TokenType = null;
                        //appSession.User = null;
                        //messenger.Publish(new InvalidTokenSessionMesage(this));
                    }
                    if (apiKeyHeaders)
                    {
                        output.Message = "error_401_key";
                    }
                }
                output.HttpResponseHeaders = response.Headers;
                return output;
            }
            catch (Exception exception)
            {
                if (exception.Message.Contains("Unable to resolve host"))
                    throw new Exception("no_connection");
                if (exception.Message.Contains("A task was canceled"))
                    throw new Exception("no_connection");
                else
                    throw exception;
            }
        }

        protected async Task<FileResponseData> MakeSessionHttpBinaryCall<TInput>(
            string endpoint,
            HttpVerbMethod verbMethod,
            TInput input,
            bool dontParse = false)
        {
            return await MakeHttpBinaryCall<TInput>(endpoint, verbMethod, input, dontParse, true);
        }


        protected async Task<FileResponseData> MakeHttpBinaryCall<TInput>(
           string endpoint,
           HttpVerbMethod verbMethod,
           TInput input,
           bool dontParse = false,
           bool sessionHeaders = false, bool noCache = false)
        {

            HttpResponseMessage response = null;

            try
            {
                var client = HttpClient;

                HttpContent content = null;
                var url = endpoint;


                if (!Equals(input, default(TInput)))
                {
                    content = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");
             
                }
                switch (verbMethod)
                {
                    case HttpVerbMethod.Get:
                        //response = await client.GetAsync(url);
                        {
                            var rq = new HttpRequestMessage(HttpMethod.Get, url);
                            AddHeaders(rq,noCache);
                            if (sessionHeaders)
                                AddSessionHeaders(rq);
                            response = await client.SendAsync(rq);
                        }
                        break;
                    case HttpVerbMethod.Post:
                        {
                            if (content == null)
                                content = new StringContent("", Encoding.UTF8, "application/json");
                            //response = await client.PostAsync(url, content);
                            var rq = new HttpRequestMessage(HttpMethod.Post, url)
                            {
                                Content = content
                            };
                            AddHeaders(rq,noCache);
                            if (sessionHeaders)
                                AddSessionHeaders(rq);
                            response = await client.SendAsync(rq);
                        }
                        break;
                    case HttpVerbMethod.Put:
                        //response = await client.PutAsync(url, content);
                        {
                            if (content == null)
                                content = new StringContent("", Encoding.UTF8, "application/json");
                            var rq = new HttpRequestMessage(HttpMethod.Put, url)
                            {
                                Content = content
                            };
                            AddHeaders(rq,noCache);
                            if (sessionHeaders)
                                AddSessionHeaders(rq);
                            response = await client.SendAsync(rq);
                        }
                        break;
                    case HttpVerbMethod.Delete:
                        {
                            if (content == null)
                                response = await client.DeleteAsync(url);
                            else
                            {
                                var rq = new HttpRequestMessage(HttpMethod.Delete, url)
                                {
                                    Content = content
                                };
                                AddHeaders(rq,noCache);
                                if (sessionHeaders)
                                    AddSessionHeaders(rq);
                                response = await client.SendAsync(rq);
                            }
                        }
                        break;
                    /*case HttpVerbMethod.Options:
                        var request = new HttpRequestMessage(HttpMethod.Options, url);                                            
                        response = await client.SendAsync(request);
                        break;*/
                    default:
                        throw new ArgumentOutOfRangeException(nameof(verbMethod), verbMethod, null);
                }

                var responseBytes = await response.Content.ReadAsByteArrayAsync();
                var responseContentType = response.Content.Headers.GetValues("Content-Type").FirstOrDefault();
                response.EnsureSuccessStatusCode();

                return new FileResponseData() { Data = responseBytes, ContentType = responseContentType };
            }
            catch (Exception exception)
            {
                throw exception;
            }
            //Control error 500 login with format
        }

        protected async Task<TOutput> MakeHttpMultipartCall<TOutput, TInput>(
            string endpoint,
            HttpVerbMethod verbMethod,
            TInput input,
            byte[] file,
            string fileName,
            string fileExtension,
            bool sessionHeaders = false, bool noCache = false) where TOutput : BaseResponse
        {

            HttpResponseMessage response = null;
            var responseText = string.Empty;

            try
            {
                var client = HttpClient;

                MultipartFormDataContent content = null;
                var url = endpoint;

                if (!Equals(input, default(TInput)))
                {
                    content = new MultipartFormDataContent();
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");
                    content.Add(jsonContent, "json");
                    var stremContent = new StreamContent(new MemoryStream(file));
                    content.Add(stremContent, "file", fileName + "." + fileExtension);
                }
                switch (verbMethod)
                {
                    case HttpVerbMethod.Get:
                        {
                            var rq = new HttpRequestMessage(HttpMethod.Get, url);
                            AddHeaders(rq,noCache);
                            if (sessionHeaders)
                                AddSessionHeaders(rq);
                            response = await client.SendAsync(rq);
                        }
                        break;
                    case HttpVerbMethod.Post:
                        {
                            var rq = new HttpRequestMessage(HttpMethod.Post, url)
                            {
                                Content = content
                            };
                            AddHeaders(rq,noCache);
                            if (sessionHeaders)
                                AddSessionHeaders(rq);
                            response = await client.SendAsync(rq);
                        }
                        break;
                    case HttpVerbMethod.Put:
                        {
                            var rq = new HttpRequestMessage(HttpMethod.Put, url)
                            {
                                Content = content
                            };
                            AddHeaders(rq,noCache);
                            if (sessionHeaders)
                                AddSessionHeaders(rq);
                            response = await client.SendAsync(rq);
                        }
                        break;
                    case HttpVerbMethod.Delete:
                        {
                            if (content == null)
                                response = await client.DeleteAsync(url);
                            else
                            {
                                var rq = new HttpRequestMessage(HttpMethod.Delete, url)
                                {
                                    Content = content
                                };
                                AddHeaders(rq,noCache);
                                if (sessionHeaders)
                                    AddSessionHeaders(rq);
                                response = await client.SendAsync(rq);
                            }
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(verbMethod), verbMethod, null);
                }

                responseText = await response.Content.ReadAsStringAsync();
                //response.EnsureSuccessStatusCode();         
                TOutput output;
                if (responseText.Trim().Length == 0)
                {
                    output = (TOutput)Activator.CreateInstance(typeof(TOutput));
                    output.status = (int)response.StatusCode;
                    output.Message = "Code " + (int)response.StatusCode + " from Server.";
                }
                else
                    output = JsonConvert.DeserializeObject<TOutput>(responseText);
                if ((response.StatusCode != System.Net.HttpStatusCode.OK) && (response.StatusCode != System.Net.HttpStatusCode.Created))
                {
                    output.HasError = true;
                }
                output.HttpResponseHeaders = response.Headers;
                return output;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }


      

    }
}
