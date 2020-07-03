using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class ResultadoTestMedico : Entity<ResultadoTestMedico>
    {
        public int IdFichaMedica { get; set; }
        public bool Control { get; set; }
        public bool Igg { get; set; }
        public bool Igm { get; set; }
        public DateTimeOffset FechaTest { get; set; }

        public virtual FichaMedica IdFichaMedicaNavigation { get; set; }
    }
}
