using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class AsignacionAdjudicacion : Entity<AsignacionAdjudicacion>
    {
        public AsignacionAdjudicacion()
        {
        }

        public int IdFichaLaboral { get; set; }
        public int IdAdjudicacionTrabajoObra { get; set; }

        public virtual FichaLaboral FichaLaboral { get; set; }
        public virtual AdjudicacionTrabajoObra AdjudicacionTrabajoObra { get; set; }
    }
}
