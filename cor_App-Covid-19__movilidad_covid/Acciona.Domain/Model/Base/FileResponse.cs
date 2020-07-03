using System;
using System.Collections.Generic;
using System.Text;

namespace Acciona.Domain.Model.Base
{
    public class FileResponse
    {
        public byte[] Data { get; set; }
        public string ContentType { get; set; }
    }
}
