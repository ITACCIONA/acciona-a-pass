using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class AmbitoAccesoEmpleadoPais : Entity<AmbitoAccesoEmpleadoPais>
    {
        public int IdEmpleado { get; set; }

        public int IdPais { get; set; }

        public virtual Empleado Empleado { get; set; }

        public virtual Pais Pais { get; set; }
    }
}
