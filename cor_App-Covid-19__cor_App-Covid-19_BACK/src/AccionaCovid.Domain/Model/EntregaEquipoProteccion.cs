using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class EntregaEquipoProteccion : Entity<EntregaEquipoProteccion>
    {
        public int IdEquipoProteccion { get; set; }
        public int IdEmpleado { get; set; }

        public virtual Empleado IdEmpleadoNavigation { get; set; }
        public virtual EquipoProteccion IdEquipoProteccionNavigation { get; set; }
    }
}
