using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class Obra : Entity<Obra>
    {
        public Obra()
        {
            AdjudicacionTrabajoObra = new HashSet<AdjudicacionTrabajoObra>();
        }

        public string CodigoObra { get; set; }

        public string Nombre { get; set; }

        public int? IdEstadoObra { get; set; }

        public virtual EstadoObra EstadoObra { get; set; }
        public virtual ICollection<AdjudicacionTrabajoObra> AdjudicacionTrabajoObra { get; set; }
        
    }
}
