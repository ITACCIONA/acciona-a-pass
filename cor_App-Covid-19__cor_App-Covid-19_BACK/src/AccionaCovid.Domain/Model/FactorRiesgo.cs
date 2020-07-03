using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class FactorRiesgo : Entity<FactorRiesgo>
    {
        public FactorRiesgo()
        {
            ValoracionFactorRiesgo = new HashSet<ValoracionFactorRiesgo>();
        }

        public string Nombre { get; set; }

        public virtual ICollection<ValoracionFactorRiesgo> ValoracionFactorRiesgo { get; set; }
    }
}
