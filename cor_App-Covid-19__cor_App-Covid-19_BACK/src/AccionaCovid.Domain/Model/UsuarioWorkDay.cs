using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class UsuarioWorkDay : Entity<UsuarioWorkDay>
    {
        public UsuarioWorkDay()
        {
            Empleado = new HashSet<Empleado>();
        }

        public long IdWorkDay { get; set; }
        public string Nombre { get; set; }
        public long? NumEmpleado { get; set; }
        public int? Genero { get; set; }
        public DateTime UltimaModif { get; set; }
        public string Nif { get; set; }
        public string Mail { get; set; }
        public string Telefono { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public long Departamento { get; set; }
        public long Division { get; set; }
        public bool? EsServicioMedico { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public long? IdResponsable { get; set; }
        public string Localizacion { get; set; }
        public string MailCorporativo { get; set; }
        public string TelefonoCorporativo { get; set; }
        public string Upn { get; set; }
        public bool? InterAcciona { get; set; }
        public string Tecnologia { get; set; }
        public virtual ICollection<Empleado> Empleado { get; set; }
    }
}
