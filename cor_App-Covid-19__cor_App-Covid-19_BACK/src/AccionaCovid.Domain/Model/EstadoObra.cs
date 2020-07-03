using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class EstadoObra : Entity<EstadoObra>
    {
        public EstadoObra()
        {
            Obra = new HashSet<Obra>();
        }

        public string Nombre { get; set; }

        public virtual ICollection<Obra> Obra { get; set; }
    }
}
