using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class AlertaServiciosMedicos : Entity<AlertaServiciosMedicos>
    {
        public int IdEmpleado { get; set; }
        public string Comentario { get; set; }
        public DateTimeOffset FechaNotificacion { get; set; }
        public string Titulo { get; set; }
        public bool? Leido { get; set; }

        public virtual Empleado IdEmpleadoNavigation { get; set; }
    }
}
