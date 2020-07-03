using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class ParametroMedico : Entity<ParametroMedico>
    {
        public ParametroMedico()
        {
            ValoracionParametroMedico = new HashSet<ValoracionParametroMedico>();
        }

        public int IdTipoParametro { get; set; }
        public string Nombre { get; set; }

        public virtual TipoParametroMedico IdTipoParametroNavigation { get; set; }
        public virtual ICollection<ValoracionParametroMedico> ValoracionParametroMedico { get; set; }
    }
}
