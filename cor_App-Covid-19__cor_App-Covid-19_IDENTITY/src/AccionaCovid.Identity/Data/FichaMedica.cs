using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccionaCovid.Domain.Model
{
    public partial class FichaMedica
    {
        public FichaMedica()
        {
            Empleado = new HashSet<Empleado>();
            ResultadoEncuestaSintomas = new HashSet<ResultadoEncuestaSintomas>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }

        public DateTime FechaAlta { get; set; }

        public virtual ICollection<Empleado> Empleado { get; set; }
        public virtual ICollection<ResultadoEncuestaSintomas> ResultadoEncuestaSintomas { get; set; }
    }
}
