using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class AdjudicacionTrabajoObra : Entity<AdjudicacionTrabajoObra>
    {
        public AdjudicacionTrabajoObra()
        {
            AsignacionAdjudicacion = new HashSet<AsignacionAdjudicacion>();
        }

        public int IdObra { get; set; }
        public int IdSubcontrata { get; set; }
        public int? IdEmpleadoResponsable { get; set; }

        public virtual Obra Obra { get; set; }
        public virtual Subcontrata Subcontrata { get; set; }
        public virtual Empleado EmpleadoResponsable { get; set; }

        public virtual ICollection<AsignacionAdjudicacion> AsignacionAdjudicacion { get; set; }
    }
}
