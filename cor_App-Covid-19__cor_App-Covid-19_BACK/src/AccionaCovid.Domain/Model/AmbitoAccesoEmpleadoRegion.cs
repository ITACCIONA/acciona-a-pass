using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class AmbitoAccesoEmpleadoRegion : Entity<AmbitoAccesoEmpleadoRegion>
    {
        public int IdEmpleado { get; set; }

        public int IdRegion { get; set; }

        public virtual Empleado Empleado { get; set; }

        public virtual Region Region { get; set; }
    }
}
