using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class FichaLaboral : Entity<FichaLaboral>
    {
        public FichaLaboral()
        {
            Empleado = new HashSet<Empleado>();
            AsignacionAdjudicacion = new HashSet<AsignacionAdjudicacion>();
        }

        public int? IdDivision { get; set; }
        public int? IdDepartamento { get; set; }
        public int? IdResponsableDirecto { get; set; }
        public int? IdLocalizacion { get; set; }
        public int? IdLocalizacionAlter { get; set; }
        public string MailProf { get; set; }
        public string Obra { get; set; }
        public string TelefonoCorp { get; set; }
        public bool IsExternal { get; set; }
        public int? IdTecnologia { get; set; }

        public virtual Departamento IdDepartamentoNavigation { get; set; }
        public virtual Division IdDivisionNavigation { get; set; }
        public virtual Localizacion IdLocalizacionNavigation { get; set; }
        public virtual Empleado IdResponsableDirectoNavigation { get; set; }
        public virtual ICollection<Empleado> Empleado { get; set; }
        public virtual Tecnologia Tecnologia { get; set; }
        public virtual ICollection<AsignacionAdjudicacion> AsignacionAdjudicacion { get; set; }
    }
}
