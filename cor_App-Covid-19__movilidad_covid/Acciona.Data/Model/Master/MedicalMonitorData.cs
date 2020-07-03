using System;
using System.Collections.Generic;

namespace Acciona.Data.Model.Master
{
    public class MedicalMonitorData
    {
        public int idParameterType { get; set; }
        public string name { get; set; }
        public IEnumerable<ParameterData> parameters { get; set; }
    }
}
