using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class LocalizacionEmpleados : Entity<LocalizacionEmpleados>
    {
        public int IdEmpleado { get; set; }
        public int IdLocalizacion { get; set; }
        public DateTimeOffset Fecha { get; set; }

        public virtual Empleado IdEmpleadoNavigation { get; set; }
        public virtual Localizacion IdLocalizacionNavigation { get; set; }
    }
}
