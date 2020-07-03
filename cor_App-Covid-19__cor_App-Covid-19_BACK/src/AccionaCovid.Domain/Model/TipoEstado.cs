using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class TipoEstado : Entity<TipoEstado>
    {
        public TipoEstado()
        {
            EstadoPasaporte = new HashSet<EstadoPasaporte>();
        }

        public string Nombre { get; set; }
        public int Prioridad { get; set; }

        public virtual ICollection<EstadoPasaporte> EstadoPasaporte { get; set; }
    }
}
