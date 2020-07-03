using System;
using System.Collections.Generic;
using System.Text;

namespace Acciona.Domain.Model.Base
{
    public class ApiException : Exception
    {
        public int Code { get; set; }
        public string Error { get; set; }
    }
}
