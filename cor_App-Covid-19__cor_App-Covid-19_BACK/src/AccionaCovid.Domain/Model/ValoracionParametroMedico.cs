using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class ValoracionParametroMedico : Entity<ValoracionParametroMedico>
    {
        public int IdParametroMedico { get; set; }
        public int IdSegumientoMedico { get; set; }
        public bool Valor { get; set; }

        public virtual ParametroMedico IdParametroMedicoNavigation { get; set; }
        public virtual SeguimientoMedico IdSegumientoMedicoNavigation { get; set; }
    }
}
