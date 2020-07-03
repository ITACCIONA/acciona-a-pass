using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class TipoSintomas : Entity<TipoSintomas>
    {
        public TipoSintomas()
        {
            ResultadoEncuestaSintomas = new HashSet<ResultadoEncuestaSintomas>();
        }

        public string Nombre { get; set; }

        public virtual ICollection<ResultadoEncuestaSintomas> ResultadoEncuestaSintomas { get; set; }
    }
}
