using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class Subcontrata : Entity<Subcontrata>
    {
        public Subcontrata()
        {
            AdjudicacionTrabajoObra = new HashSet<AdjudicacionTrabajoObra>();
        }

        public string Cif { get; set; }
        public string Nombre { get; set; }
        public virtual ICollection<AdjudicacionTrabajoObra> AdjudicacionTrabajoObra { get; set; }

    }
}
