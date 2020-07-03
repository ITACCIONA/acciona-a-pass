using System;
using System.Collections.Generic;

namespace Acciona.Domain.Model.Master
{
    public class MedicalMonitor
    {
        public int IdParameterType { get; set; }
        public string Name { get; set; }
        public IEnumerable<Parameter> Parameters { get; set; }
    }
}
