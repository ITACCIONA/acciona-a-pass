using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acciona.Domain.Model.Base
{
    public class TaskGenericResponse<T>
    {
        public T Data { get; set; }
        public int ErrorCode{ get; set; }
        public string Message { get; set; }
    }

    public class TaskGenericResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
    }
}
