using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace Acciona.Data.Model
{
    public class BaseResponse
    {
        //Response headers
        public HttpHeaders HttpResponseHeaders { get; set; }

        //Error control
        public bool HasError = false;
        public int status { get; set; }
        public string Message { get; set; }

        public bool success { get; set; }
        public IEnumerable<InfoData> info { get; set; }        
    }

    public class InfoData
    {
        public string code { get; set; }
        public string message { get; set; }
    }

    public class BaseResponse<T>:BaseResponse
    {
        
        public IEnumerable<T> data { get; set; }
    }
}
