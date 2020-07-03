using System;
namespace Acciona.Data.Model.Employee
{
    public class PassportData
    {
        public int idEmpleado { get; set; }
        public int idPassport { get; set; }
        public string nombreEmpleado { get; set; }
        public string inicialesEmpleado { get; set; }
        public int edadEmpleado { get; set; }
        public int numEmpleado { get; set; }
        public string departamento { get; set; }
        public string nameLocalizacion { get; set; }
        public string pais { get; set; }
        public string direccion1 { get; set; }
        public string ciudad { get; set; }
        public string codigoPostal { get; set; }
        public string division { get; set; }
        public string estadoPasaporte { get; set; }
        public string colorPasaporte { get; set; }        
        public int numTest { get; set; }
        public bool hasMessage { get; set; }
        public int estadoId { get; set; }
        public string fechaCreacion { get; set; }
        public string fechaExpiracion { get; set; }
    }
}
