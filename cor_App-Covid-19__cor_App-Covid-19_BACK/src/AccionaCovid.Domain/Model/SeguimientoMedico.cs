using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class SeguimientoMedico : Entity<SeguimientoMedico>
    {
        public SeguimientoMedico()
        {
            ValoracionParametroMedico = new HashSet<ValoracionParametroMedico>();
        }

        public int IdFichaMedica { get; set; }
        public string Comentarios { get; set; }
        public bool? Activo { get; set; }
        public DateTimeOffset FechaSeguimiento { get; set; }

        public virtual FichaMedica IdFichaMedicaNavigation { get; set; }
        public virtual ICollection<ValoracionParametroMedico> ValoracionParametroMedico { get; set; }
    }
}
