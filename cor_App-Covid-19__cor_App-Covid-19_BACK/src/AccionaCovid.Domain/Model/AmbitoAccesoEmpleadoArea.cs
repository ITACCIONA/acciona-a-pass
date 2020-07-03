using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class AmbitoAccesoEmpleadoArea : Entity<AmbitoAccesoEmpleadoArea>
    {
        public int IdEmpleado { get; set; }

        public int IdArea { get; set; }

        public virtual Empleado Empleado { get; set; }

        public virtual Area Area { get; set; }
    }
}
