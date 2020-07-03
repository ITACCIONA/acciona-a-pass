using System;
namespace Acciona.Data.Model.Employee
{
    public class RequestModifyFicha
    {
        public int idEmployee { get; set; }
        public string telefonoEmpleado { get; set; }
        public long? idLocalizacion { get; set; }
    }
}
