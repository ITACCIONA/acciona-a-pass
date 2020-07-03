using System;
using System.Collections.Generic;

namespace Acciona.Domain.Model.Employee
{
    public class Ficha
    {
        public int IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; }
        public string ApellidosEmpleado { get; set; }
        public string InicialesEmpleado { get; set; }
        public string DNI { get; set; }
        public int EdadEmpleado { get; set; }
        public int NumEmpleado { get; set; }
        public string TelefonoEmpleado { get; set; }
        public string MailEmpleado { get; set; }
        public IEnumerable<RiskFactorValue> ValoracionFactorRiesgos { get; set; }
        public long? IdLocalizacion { get; set; }
        public string Localizacion { get; set; }
    }
}
