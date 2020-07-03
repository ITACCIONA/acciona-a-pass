using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class ResultadoEncuestaSintomas : Entity<ResultadoEncuestaSintomas>
    {
        public int IdFichaMedica { get; set; }
        public int IdTipoSintoma { get; set; }
        public bool Valor { get; set; }
        public Guid? GrupoRespuestas { get; set; }

        public virtual FichaMedica IdFichaMedicaNavigation { get; set; }
        public virtual TipoSintomas IdTipoSintomaNavigation { get; set; }
    }
}
