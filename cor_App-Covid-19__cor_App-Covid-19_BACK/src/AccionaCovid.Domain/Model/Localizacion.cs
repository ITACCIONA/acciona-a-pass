using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AccionaCovid.Domain.Model
{
    public partial class Localizacion : Entity<Localizacion>
    {
        public Localizacion()
        {
            FichaLaboral = new HashSet<FichaLaboral>();
            LocalizacionEmpleados = new HashSet<LocalizacionEmpleados>();
        }

        public string Nombre { get; set; }
        public string Ciudad { get; set; }
        public string CodigoPostal { get; set; }
        public string Direccion1 { get; set; }
        public string Pais { get; set; }
        public int? IdArea { get; set; }

        public virtual ICollection<FichaLaboral> FichaLaboral { get; set; }
        public virtual ICollection<LocalizacionEmpleados> LocalizacionEmpleados { get; set; }
        public virtual Area Area { get; set; }
    }
}
