using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class EstadoPasaporteIdioma : Entity<EstadoPasaporteIdioma>
    {
        public string Nombre { get; set; }
        public string Idioma { get; set; }
        public int IdEstadoPasaporte { get; set; }

        public virtual EstadoPasaporte IdEstadoPasaporteNavigation { get; set; }
    }
}
