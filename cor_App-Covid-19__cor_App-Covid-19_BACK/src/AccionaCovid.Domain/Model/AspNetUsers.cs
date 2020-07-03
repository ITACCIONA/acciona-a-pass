using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class AspNetUsers
    {
        public AspNetUsers()
        {
        }

        public string Id { get; set; }

        public int? IdEmpleado { get; set; }
        public virtual Empleado Empleado { get; set; }
    }
}
