using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class Region : Entity<Region>
    {
        public Region()
        {
            AmbitoAccesoEmpleadoRegion = new HashSet<AmbitoAccesoEmpleadoRegion>();
            Area = new HashSet<Area>();
        }

        public string Nombre { get; set; }

        public int IdPais { get; set; }

        public virtual Pais Pais { get; set; }

        public virtual ICollection<AmbitoAccesoEmpleadoRegion> AmbitoAccesoEmpleadoRegion { get; set; }

        public virtual ICollection<Area> Area { get; set; }
    }
}
