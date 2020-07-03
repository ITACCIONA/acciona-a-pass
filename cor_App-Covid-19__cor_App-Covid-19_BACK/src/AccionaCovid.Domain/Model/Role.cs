using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class Role : Entity<Role>
    {
        public Role()
        {
            EmpleadoRole = new HashSet<EmpleadoRole>();
        }

        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<EmpleadoRole> EmpleadoRole { get; set; }
    }
}
