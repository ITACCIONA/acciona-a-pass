using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class IntegracionExternos : Entity<IntegracionExternos>
    {
        public IntegracionExternos()
        {
            Empleado = new HashSet<Empleado>();
            //AdjudicacionTrabajoObra = new HashSet<AdjudicacionTrabajoObra>();
        }
        public string Nif { get; set; }
        public string Nombre { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public string Origen { get; set; }
        public string NifOriginal { get; set; }    
        public DateTime UltimaModif { get; set; }
        public virtual ICollection<Empleado> Empleado { get; set; }
        //public virtual ICollection<AdjudicacionTrabajoObra> AdjudicacionTrabajoObra { get; set; }
    }
}
