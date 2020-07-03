using System;
using System.Collections.Generic;

namespace Acciona.Data.Model.Employee
{
    public class RequestSymptoms
    {
        public int idEmployee { get; set; }
        public string currentDeviceDateTime { get; set; }
        public IEnumerable<RequestValueData> values { get; set; }
    }
}
