using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class TipoParametroMedico : Entity<TipoParametroMedico>
    {
        public TipoParametroMedico()
        {
            ParametroMedico = new HashSet<ParametroMedico>();
        }

        public string Nombre { get; set; }

        public virtual ICollection<ParametroMedico> ParametroMedico { get; set; }
    }
}
