using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class EstadoPasaporte : Entity<EstadoPasaporte>
    {
        public EstadoPasaporte()
        {
            EstadoPasaporteIdioma = new HashSet<EstadoPasaporteIdioma>();
            Pasaporte = new HashSet<Pasaporte>();
        }

        public string Nombre { get; set; }
        public int? DiasValidez { get; set; }
        public bool? Pcrant { get; set; }
        public bool? Pcrult { get; set; }
        public bool? TestInmuneIgG { get; set; }
        public bool? TestInmuneIgM { get; set; }
        public bool? Comment { get; set; }
        public int IdTipoEstado { get; set; }
        public int IdColorEstado { get; set; }
        public int EstadoId { get; set; }

        public virtual ColorEstado IdColorEstadoNavigation { get; set; }
        public virtual TipoEstado IdTipoEstadoNavigation { get; set; }
        public virtual ICollection<EstadoPasaporteIdioma> EstadoPasaporteIdioma { get; set; }
        public virtual ICollection<Pasaporte> Pasaporte { get; set; }
    }
}
