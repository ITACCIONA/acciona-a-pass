using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class FichaMedica : Entity<FichaMedica>
    {
        public FichaMedica()
        {
            Empleado = new HashSet<Empleado>();
            ResultadoEncuestaSintomas = new HashSet<ResultadoEncuestaSintomas>();
            ResultadoTestMedico = new HashSet<ResultadoTestMedico>();
            ResultadoTestPcr = new HashSet<ResultadoTestPcr>();
            SeguimientoMedico = new HashSet<SeguimientoMedico>();
            ValoracionFactorRiesgo = new HashSet<ValoracionFactorRiesgo>();
        }

        public DateTime FechaAlta { get; set; }

        public virtual ICollection<Empleado> Empleado { get; set; }
        public virtual ICollection<ResultadoEncuestaSintomas> ResultadoEncuestaSintomas { get; set; }
        public virtual ICollection<ResultadoTestMedico> ResultadoTestMedico { get; set; }
        public virtual ICollection<ResultadoTestPcr> ResultadoTestPcr { get; set; }
        public virtual ICollection<SeguimientoMedico> SeguimientoMedico { get; set; }
        public virtual ICollection<ValoracionFactorRiesgo> ValoracionFactorRiesgo { get; set; }
    }
}
