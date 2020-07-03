using System;
namespace Acciona.Domain.Model.Employee
{
    public class Passport
    {
        public int IdEmpleado { get; set; }
        public int IdPassport { get; set; }
        public string NombreEmpleado { get; set; }
        public string InicialesEmpleado { get; set; }
        public int EdadEmpleado { get; set; }
        public int NumEmpleado { get; set; }
        public string Departamento { get; set; }
        public string NameLocalizacion { get; set; }
        public string Pais { get; set; }
        public string Direccion1 { get; set; }
        public string Ciudad { get; set; }
        public string CodigoPostal { get; set; }
        public string Division { get; set; }
        public string EstadoPasaporte { get; set; }
        public string ColorPasaporte { get; set; }
        public bool HasMessage { get; set; }
        public int EstadoId { get; set; }
        public int NumTest { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaExpiracion { get; set; }
    }
}
