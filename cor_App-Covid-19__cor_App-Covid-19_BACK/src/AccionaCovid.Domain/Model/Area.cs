using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class Area : Entity<Area>
    {
        public Area()
        {
            AmbitoAccesoEmpleadoArea = new HashSet<AmbitoAccesoEmpleadoArea>();
            Localizacion = new HashSet<Localizacion>();
        }

        public string Nombre { get; set; }

        public int IdRegion { get; set; }

        public virtual Region Region { get; set; }

        public virtual ICollection<AmbitoAccesoEmpleadoArea> AmbitoAccesoEmpleadoArea { get; set; }

        public virtual ICollection<Localizacion> Localizacion { get; set; }
    }
}
