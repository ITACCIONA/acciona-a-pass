using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class ResultadoTestPcr : Entity<ResultadoTestPcr>
    {
        public int IdFichaMedica { get; set; }
        public bool Positivo { get; set; }
        public DateTimeOffset FechaTest { get; set; }

        public virtual FichaMedica IdFichaMedicaNavigation { get; set; }
    }
}
