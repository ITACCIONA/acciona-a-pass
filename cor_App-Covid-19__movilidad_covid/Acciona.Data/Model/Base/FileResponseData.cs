using System;
using System.Collections.Generic;
using System.Text;

namespace Acciona.Data.Model
{
    public class FileResponseData : BaseResponse
    {
        public byte[] Data { get; set; }
        public string ContentType { get; set; }
    }
}
