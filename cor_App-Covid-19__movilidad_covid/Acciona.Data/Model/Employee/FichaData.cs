using System;
using System.Collections.Generic;

namespace Acciona.Data.Model.Employee
{
    public class FichaData
    {
        public int idEmpleado { get; set; }
        public string nombreEmpleado { get; set; }
        public string apellidosEmpleado { get; set; }
        public string inicialesEmpleado { get; set; }
        public string dni { get; set; }
        public int edadEmpleado { get; set; }
        public int numEmpleado { get; set; }
        public string telefonoEmpleado { get; set; }
        public string mailEmpleado { get; set; }
        public IEnumerable<RiskFactorValueData> valoracionFactorRiesgos { get; set; }
        public long? idLocalizacion { get; set; }
        public string localizacion { get; set; }
    }
}
