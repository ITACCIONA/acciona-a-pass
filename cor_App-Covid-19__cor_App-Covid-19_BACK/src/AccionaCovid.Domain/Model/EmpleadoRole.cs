using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class EmpleadoRole : Entity<EmpleadoRole>
    {
        public int IdEmpleado { get; set; }
        public int IdRole { get; set; }

        public virtual Empleado IdEmpleadoNavigation { get; set; }
        public virtual Role IdRoleNavigation { get; set; }
    }
}
