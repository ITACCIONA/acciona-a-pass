using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class Tecnologia : Entity<Tecnologia>
    {
        public Tecnologia()
        {
            FichaLaboral = new HashSet<FichaLaboral>();
        }

        public string Nombre { get; set; }

        public virtual ICollection<FichaLaboral> FichaLaboral { get; set; }
    }
}
