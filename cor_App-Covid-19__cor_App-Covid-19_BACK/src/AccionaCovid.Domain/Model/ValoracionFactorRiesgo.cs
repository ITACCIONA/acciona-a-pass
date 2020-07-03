using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class ValoracionFactorRiesgo : Entity<ValoracionFactorRiesgo>
    {
        public int IdFactorRiesgo { get; set; }
        public int IdFichaMedica { get; set; }
        public bool? Valor { get; set; }
        public DateTimeOffset FechaFactor { get; set; }

        public virtual FactorRiesgo IdFactorRiesgoNavigation { get; set; }
        public virtual FichaMedica IdFichaMedicaNavigation { get; set; }
    }
}
