using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class EquipoProteccion : Entity<EquipoProteccion>
    {
        public EquipoProteccion()
        {
            EntregaEquipoProteccion = new HashSet<EntregaEquipoProteccion>();
        }


        public virtual ICollection<EntregaEquipoProteccion> EntregaEquipoProteccion { get; set; }
    }
}
