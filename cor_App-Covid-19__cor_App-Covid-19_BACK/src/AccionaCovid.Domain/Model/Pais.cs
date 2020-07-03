using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class Pais : Entity<Pais>
    {
        public Pais()
        {
            AmbitoAccesoEmpleadoPais = new HashSet<AmbitoAccesoEmpleadoPais>();
            Region = new HashSet<Region>();
        }

        public string Nombre { get; set; }

        public virtual ICollection<AmbitoAccesoEmpleadoPais> AmbitoAccesoEmpleadoPais { get; set; }

        public virtual ICollection<Region> Region { get; set; }
    }
}
