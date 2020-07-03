using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccionaCovid.Domain.Model
{
    public partial class FichaLaboral
    {
        public FichaLaboral()
        {
            Empleado = new HashSet<Empleado>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }
        public bool IsExternal { get; set; }

        public int? IdResponsableDirecto { get; set; }

        public virtual ICollection<Empleado> Empleado { get; set; }
    }
}
