using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class Pasaporte : Entity<Pasaporte>
    {
        public int IdEstadoPasaporte { get; set; }
        public int IdAccion { get; set; }
        public int IdEmpleado { get; set; }
        public DateTimeOffset FechaCreacion { get; set; }
        public DateTimeOffset? FechaExpiracion { get; set; }
        public bool? Activo { get; set; }
        public bool IsManual { get; set; }

        public virtual Empleado IdEmpleadoNavigation { get; set; }
        public virtual EstadoPasaporte IdEstadoPasaporteNavigation { get; set; }
    }
}
